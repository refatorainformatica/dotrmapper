# Configuration Guide

Complete reference for configuring **DotRMapper** mappings.

---

## MapperConfiguration

The entry point for all mapping configuration.

```csharp
var config = new MapperConfiguration(Action<IMapperConfigurationExpression> configure);
var config = new MapperConfiguration(params Profile[] profiles);
```

| Method | Description |
|--------|-------------|
| `CreateMapper()` | Returns a configured `IMapper` instance |
| `AssertConfigurationIsValid()` | Validates all registered type maps |

---

## IMapperConfigurationExpression

| Method | Description |
|--------|-------------|
| `CreateMap<TSource, TDestination>()` | Registers or updates a type map |
| `AddProfile<TProfile>()` | Registers a profile by type |
| `AddProfile(Profile profile)` | Registers a profile instance |

---

## IMappingExpression\<TSource, TDestination\>

Returned by `CreateMap`. Supports method chaining.

### ForMember

Configure an individual destination member:

```csharp
.ForMember(dest => dest.Property, opt => { /* member options */ })
```

### BeforeMap / AfterMap

Execute callbacks around the mapping operation:

```csharp
.BeforeMap((src, dest) => { /* ... */ })
.BeforeMap((src, dest, context) => { /* ... */ })
.AfterMap((src, dest) => { /* ... */ })
.AfterMap((src, dest, context) => { /* ... */ })
```

Callbacks receive a `ResolutionContext` when the three-argument overload is used, providing access to the root source, destination, and the `IMapper` instance.

### ReverseMap

Creates an inverse mapping from destination to source:

```csharp
cfg.CreateMap<Source, Destination>()
    .ForMember(dest => dest.Computed, opt => opt.Ignore())
    .ReverseMap();
```

Convention-mapped properties are reversed automatically. Custom or ignored members must be configured on the reverse map separately.

---

## IMemberConfigurationExpression\<TSource, TDestination, TMember\>

Configure how a single destination member is populated.

| Method | Description |
|--------|-------------|
| `MapFrom<TSourceMember>(Expression<Func<TSource, TSourceMember>>)` | Map from a source member expression |
| `MapFrom(Func<TSource, TDestination, TMember>)` | Resolve using a delegate with source and destination |
| `MapFrom(Func<TSource, TDestination, TMember, ResolutionContext, TMember>)` | Resolve with full context |
| `MapFrom<TValueResolver>()` | Use a custom `IValueResolver` |
| `Ignore()` | Skip this member during mapping |
| `ConvertUsing<TConverter>()` | Apply an `ITypeConverter` |

---

## IValueResolver\<TSource, TDestination, TDestMember\>

Defined in `DotRMapper.Abstractions.Resolvers`.

Implement custom resolution logic:

```csharp
public class FullNameResolver : IValueResolver<PersonSource, PersonDestination, string>
{
    public string Resolve(
        PersonSource source,
        PersonDestination destination,
        string destMember,
        ResolutionContext context)
    {
        return $"{source.FirstName} {source.LastName}";
    }
}

// Configuration:
.ForMember(dest => dest.FullName, opt => opt.MapFrom<FullNameResolver>());
```

---

## ITypeConverter

Defined in `DotRMapper.Abstractions.Converters`.

Convert source values to a different destination type:

```csharp
public class DecimalToStringConverter : ITypeConverter
{
    public object? Convert(object? source, Type destinationType, ResolutionContext context)
    {
        return source is decimal value ? value.ToString("F2") : null;
    }
}

// Configuration:
.ForMember(dest => dest.Amount, opt => opt.ConvertUsing<DecimalToStringConverter>());
```

---

## Profile

Defined in `DotRMapper.Abstractions.Configuration`.

Base class for grouping mappings:

```csharp
public class OrderProfile : Profile
{
    protected override void ConfigureMappings()
    {
        CreateMap<OrderSource, OrderDto>();
        CreateMap<OrderLineSource, OrderLineDto>();
    }
}
```

---

## IMapper

Defined in `DotRMapper.Abstractions`.

The runtime mapping interface.

| Method | Description |
|--------|-------------|
| `Map<TDestination>(object source)` | Map to destination type inferred from generic parameter |
| `Map<TSource, TDestination>(TSource source)` | Map source to new destination instance |
| `Map(object source, Type destinationType)` | Map using runtime types |
| `Map<TSource, TDestination>(TSource source, TDestination destination)` | Map onto existing instance |
| `Map<TSource, TDestination>(IEnumerable<TSource> source)` | Map a collection |

---

## ResolutionContext

Available during mapping callbacks and resolvers.

| Property | Description |
|----------|-------------|
| `Mapper` | The active `IMapper` instance (useful for nested manual mapping) |
| `Source` | Root source object |
| `Destination` | Root destination object |

---

## Supported Type Conversions

DotRMapper handles the following conversions automatically:

- Same-type assignment
- Primitive type coercion via `Convert.ChangeType`
- Enum to enum (by name)
- Enum to string and string to enum
- Nullable value types
- Nested object mapping (when a type map exists)
- Collection mapping (`IEnumerable<T>`, arrays, `List<T>`)

---

## Error Handling

| Exception | When |
|-----------|------|
| `DotRMapperConfigurationException` | Thrown by `AssertConfigurationIsValid()` when mappings are incomplete |
| `InvalidOperationException` | Thrown when a destination type lacks a parameterless constructor |
| `ArgumentNullException` | Thrown when required arguments are null |

---

## Best Practices

1. **Validate at startup** — Call `AssertConfigurationIsValid()` once during application initialization.
2. **Use profiles** — Keep mapping configuration organized by domain or feature area.
3. **Register nested maps** — Always create maps for nested types before the parent map.
4. **Ignore computed properties** — Use `Ignore()` or provide a `MapFrom` for members without a source counterpart.
5. **Singleton mapper** — Create one `IMapper` instance and reuse it; the mapper is thread-safe after configuration.
