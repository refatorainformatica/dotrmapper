# Examples

Practical recipes for common **DotRMapper** scenarios.

---

## Example 1: DTO Mapping in a Web API

```csharp
// Domain entity
public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Sku { get; set; } = string.Empty;
}

// API response DTO
public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PriceFormatted { get; set; } = string.Empty;
}

// Profile
public class ProductProfile : Profile
{
    protected override void ConfigureMappings()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(d => d.PriceFormatted, o => o.MapFrom(s => s.Price.ToString("C")));
    }
}

// Usage in a controller
public class ProductsController : ControllerBase
{
    private readonly IMapper _mapper;

    public ProductsController(IMapper mapper) => _mapper = mapper;

    [HttpGet("{id}")]
    public ActionResult<ProductDto> Get(Product product)
        => Ok(_mapper.Map<Product, ProductDto>(product));
}
```

---

## Example 2: Entity Update (Map onto Existing Instance)

```csharp
public class UpdateOrderHandler
{
    private readonly IMapper _mapper;

    public UpdateOrderHandler(IMapper mapper) => _mapper = mapper;

    public void Handle(UpdateOrderCommand command, Order existingOrder)
    {
        _mapper.Map(command, existingOrder);
        // existingOrder is updated in place — ideal for EF Core tracked entities
    }
}
```

---

## Example 3: Reverse Mapping

```csharp
var config = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<Employee, EmployeeDto>()
        .ForMember(d => d.FullName, o => o.MapFrom(s => $"{s.FirstName} {s.LastName}"))
        .ReverseMap()
        .ForMember(s => s.FirstName, o => o.Ignore())  // Cannot reverse computed members
        .ForMember(s => s.LastName, o => o.Ignore());
});

var mapper = config.CreateMapper();

// DTO → Entity
var employee = mapper.Map<EmployeeDto, Employee>(dto);
```

---

## Example 4: Custom Value Resolver

```csharp
public class OrderTotalResolver : IValueResolver<OrderSource, OrderDto, decimal>
{
    public decimal Resolve(OrderSource source, OrderDto destination, decimal destMember, ResolutionContext context)
        => source.Items.Sum(i => i.Quantity * i.UnitPrice);
}

public class OrderProfile : Profile
{
    protected override void ConfigureMappings()
    {
        CreateMap<OrderItemSource, OrderItemDto>();
        CreateMap<OrderSource, OrderDto>()
            .ForMember(d => d.Total, o => o.MapFrom<OrderTotalResolver>());
    }
}
```

---

## Example 5: BeforeMap / AfterMap Hooks

```csharp
cfg.CreateMap<CreateUserCommand, User>()
    .BeforeMap((cmd, user) =>
    {
        // Normalize input before mapping
        cmd.Email = cmd.Email.Trim().ToLowerInvariant();
    })
    .AfterMap((cmd, user) =>
    {
        // Set audit fields after mapping
        user.CreatedAt = DateTime.UtcNow;
        user.CreatedBy = "system";
    });
```

---

## Example 6: Collection and Nested Mapping

```csharp
public class CustomerProfile : Profile
{
    protected override void ConfigureMappings()
    {
        CreateMap<Address, AddressDto>();
        CreateMap<Customer, CustomerDto>();
    }
}

var customer = new Customer
{
    Name = "Contoso",
    Address = new Address { City = "Seattle", Country = "US" },
    Orders = new List<Order> { /* ... */ }
};

var dto = mapper.Map<Customer, CustomerDto>(customer);
// dto.Address.City == "Seattle"
// dto.Orders is mapped automatically when Order → OrderDto is registered
```

---

## Example 7: Mapping Enums Across Types

```csharp
public enum SourceStatus { Active, Inactive, Pending }
public enum DestStatus   { Active, Inactive, Pending }

cfg.CreateMap<SourceEntity, DestEntity>();
// SourceEntity.Status (SourceStatus) → DestEntity.Status (DestStatus)
// Mapped by enum name automatically
```

---

## Example 8: Startup Configuration with Validation

```csharp
public static class MappingConfig
{
    public static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductProfile>();
            cfg.AddProfile<OrderProfile>();
            cfg.AddProfile<CustomerProfile>();
        });

        config.AssertConfigurationIsValid();
        return config.CreateMapper();
    }
}
```

Call `CreateMapper()` once at application startup and register the result as a singleton in your DI container.

---

## Example 9: Ignoring Sensitive Fields

```csharp
cfg.CreateMap<User, UserDto>()
    .ForMember(d => d.PasswordHash, o => o.Ignore());

cfg.CreateMap<UserEntity, UserResponse>()
    .ForMember(d => d.InternalNotes, o => o.Ignore());
```

Always ignore security-sensitive or internal-only properties explicitly rather than relying on convention mapping.

---

## Example 10: Type Converter for Custom Formatting

```csharp
public class UtcDateConverter : ITypeConverter
{
    public object? Convert(object? source, Type destinationType, ResolutionContext context)
    {
        if (source is DateTime date)
            return date.ToUniversalTime().ToString("O");

        return null;
    }
}

cfg.CreateMap<Event, EventDto>()
    .ForMember(d => d.StartDateUtc, o => o.ConvertUsing<UtcDateConverter>());
```
