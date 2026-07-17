# Getting Started

This guide walks you through the core concepts of **DotRMapper** and shows how to integrate it into your .NET application.

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download) or later
- Basic familiarity with C# and LINQ expressions

---

## Basic Mapping

DotRMapper uses **convention-based mapping** by default. If the source and destination types have properties with the same name, they are mapped automatically.

```csharp
var config = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<Source, Destination>();
});

var mapper = config.CreateMapper();
var result = mapper.Map<Source, Destination>(source);
```

Properties are matched **case-insensitively**. Only public instance properties with a public setter on the destination are considered.

---

## Custom Member Mapping

When destination properties do not exist on the source, configure them explicitly:

```csharp
cfg.CreateMap<Source, Destination>()
    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
    .ForMember(dest => dest.Age, opt => opt.MapFrom(src => DateTime.UtcNow.Year - src.BirthDate.Year));
```

### Ignoring Properties

Skip destination members that should not be populated:

```csharp
cfg.CreateMap<Source, Destination>()
    .ForMember(dest => dest.InternalId, opt => opt.Ignore());
```

---

## Mapping onto Existing Objects

Use the two-argument `Map` overload to populate an existing destination instance:

```csharp
var destination = new Destination();
mapper.Map(source, destination);
```

This is useful when updating tracked entities in an ORM context.

---

## Collection Mapping

Map entire collections with a single call:

```csharp
var config = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<OrderItemSource, OrderItemDestination>();
});

var mapper = config.CreateMapper();
var items = mapper.Map<OrderItemSource, OrderItemDestination>(sourceItems);
```

DotRMapper also supports mapping arrays to lists and nested collections automatically.

---

## Nested Objects

When both source and destination contain complex types, register mappings for each level:

```csharp
cfg.CreateMap<AddressSource, AddressDestination>();
cfg.CreateMap<CustomerSource, CustomerDestination>();
```

DotRMapper recursively maps nested objects when a matching type map exists.

---

## Profiles

Organize mappings into reusable profile classes:

```csharp
using DotRMapper.Abstractions.Configuration;

public class ApplicationProfile : Profile
{
    protected override void ConfigureMappings()
    {
        CreateMap<Source, Destination>();
        CreateMap<AddressSource, AddressDestination>();
    }
}
```

Register profiles during configuration:

```csharp
var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<ApplicationProfile>();
});
```

---

## Validation

Validate your configuration at startup to catch unmapped members early:

```csharp
var config = new MapperConfiguration(cfg => { /* ... */ });
config.AssertConfigurationIsValid();
```

This throws a `DotRMapperConfigurationException` listing every destination member that lacks a source mapping or explicit configuration.

---

## Dependency Injection

While DotRMapper does not ship a DI package, integration is straightforward:

```csharp
// Program.cs / Startup.cs
builder.Services.AddSingleton<IMapper>(_ =>
{
    var config = new MapperConfiguration(cfg =>
    {
        cfg.AddProfile<ApplicationProfile>();
    });

    config.AssertConfigurationIsValid();
    return config.CreateMapper();
});
```

Inject `IMapper` wherever mapping is needed.

---

## Next Steps

- [Configuration Guide](configuration-guide.md) — full API reference
- [Examples](examples.md) — advanced patterns and recipes
