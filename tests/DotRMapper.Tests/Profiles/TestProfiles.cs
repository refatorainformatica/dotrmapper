using DotRMapper.Abstractions.Configuration;
using DotRMapper.Tests.Models;

namespace DotRMapper.Tests.Profiles;

/// <summary>
/// Profile that configures person mappings with computed members.
/// </summary>
/// <remarks>
/// Maps <see cref="FullName"/> and <see cref="PersonDestination.Age"/> from source data.
/// </remarks>
public class PersonProfile : Profile
{
    /// <summary>
    /// Registers person source-to-destination mappings.
    /// </summary>
    /// <remarks>
    /// Called automatically when the profile is added to configuration.
    /// </remarks>
    protected override void ConfigureMappings()
    {
        CreateMap<PersonSource, PersonDestination>()
            .ForMember(
                dest => dest.FullName,
                opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}")
            )
            .ForMember(
                dest => dest.Age,
                opt => opt.MapFrom(src => DateTime.UtcNow.Year - src.BirthDate.Year)
            );
    }
}

/// <summary>
/// Profile that configures customer and address mappings.
/// </summary>
/// <remarks>
/// Relies on convention-based mapping for matching property names.
/// </remarks>
public class CustomerProfile : Profile
{
    /// <summary>
    /// Registers address and customer mappings.
    /// </summary>
    /// <remarks>
    /// Nested address mapping is resolved recursively at runtime.
    /// </remarks>
    protected override void ConfigureMappings()
    {
        CreateMap<AddressSource, AddressDestination>();
        CreateMap<CustomerSource, CustomerDestination>();
    }
}

/// <summary>
/// Profile that configures order and order item mappings.
/// </summary>
/// <remarks>
/// Used to test array-to-list collection mapping.
/// </remarks>
public class OrderProfile : Profile
{
    /// <summary>
    /// Registers order item and order mappings.
    /// </summary>
    /// <remarks>
    /// Item mapping must exist for collection elements to map correctly.
    /// </remarks>
    protected override void ConfigureMappings()
    {
        CreateMap<OrderItemSource, OrderItemDestination>();
        CreateMap<OrderSource, OrderDestination>();
    }
}

/// <summary>
/// Profile that configures product mappings with display formatting.
/// </summary>
/// <remarks>
/// Demonstrates custom member mapping for formatted output.
/// </remarks>
public class ProductProfile : Profile
{
    /// <summary>
    /// Registers product source-to-destination mappings.
    /// </summary>
    /// <remarks>
    /// Maps price explicitly and formats display price as currency text.
    /// </remarks>
    protected override void ConfigureMappings()
    {
        CreateMap<ProductSource, ProductDestination>()
            .ForMember(
                dest => dest.DisplayPrice,
                opt => opt.MapFrom(src => src.Price.ToString("C"))
            )
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));
    }
}

/// <summary>
/// Profile that configures employee mappings with an ignored member.
/// </summary>
/// <remarks>
/// Used in ignore and validation tests for <see cref="EmployeeDestination.BadgeId"/>.
/// </remarks>
public class EmployeeProfile : Profile
{
    /// <summary>
    /// Registers employee source-to-destination mappings.
    /// </summary>
    /// <remarks>
    /// Badge identifier is explicitly ignored during mapping.
    /// </remarks>
    protected override void ConfigureMappings()
    {
        CreateMap<EmployeeSource, EmployeeDestination>()
            .ForMember(dest => dest.BadgeId, opt => opt.Ignore());
    }
}
