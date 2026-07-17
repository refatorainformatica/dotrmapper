using DotRMapper.Exceptions;
using DotRMapper.Tests.Models;
using DotRMapper.Tests.Profiles;
using FluentAssertions;

namespace DotRMapper.Tests;

/// <summary>
/// Tests reverse mapping configuration.
/// </summary>
/// <remarks>
/// Verifies that ReverseMap creates a usable inverse type map.
/// </remarks>
public class ReverseMapTests
{
    /// <summary>
    /// Verifies that ReverseMap enables mapping from destination back to source.
    /// </summary>
    /// <remarks>
    /// Ignored computed members are excluded from the reverse configuration.
    /// </remarks>
    [Fact]
    public void ReverseMap_ShouldCreateInverseMapping()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<PersonSource, PersonDestination>()
                .ForMember(dest => dest.FullName, opt => opt.Ignore())
                .ForMember(dest => dest.Age, opt => opt.Ignore())
                .ReverseMap();
        });

        var mapper = config.CreateMapper();
        var destination = new PersonDestination
        {
            Id = 7,
            FirstName = "Reverse",
            LastName = "Test",
        };

        var result = mapper.Map<PersonDestination, PersonSource>(destination);

        result.Id.Should().Be(7);
        result.FirstName.Should().Be("Reverse");
        result.LastName.Should().Be("Test");
    }
}

/// <summary>
/// Tests profile registration through configuration APIs.
/// </summary>
/// <remarks>
/// Covers AddProfile and the profiles constructor overload.
/// </remarks>
public class ProfileTests
{
    /// <summary>
    /// Verifies that AddProfile registers mappings defined in the profile.
    /// </summary>
    /// <remarks>
    /// Uses <see cref="PersonProfile"/> to validate computed member mapping.
    /// </remarks>
    [Fact]
    public void AddProfile_ShouldRegisterProfileMappings()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<PersonProfile>());
        var mapper = config.CreateMapper();

        var result = mapper.Map<PersonSource, PersonDestination>(
            new PersonSource
            {
                FirstName = "Profile",
                LastName = "Test",
                BirthDate = new DateTime(1985, 3, 15),
            }
        );

        result.FullName.Should().Be("Profile Test");
    }

    /// <summary>
    /// Verifies that passing profile instances to the constructor registers their mappings.
    /// </summary>
    /// <remarks>
    /// Multiple profiles are applied in constructor argument order.
    /// </remarks>
    [Fact]
    public void Constructor_WithProfiles_ShouldRegisterMappings()
    {
        var config = new MapperConfiguration(new CustomerProfile(), new OrderProfile());
        var mapper = config.CreateMapper();

        var customer = mapper.Map<CustomerSource, CustomerDestination>(
            new CustomerSource
            {
                Name = "Contoso",
                Address = new AddressSource { City = "Seattle" },
            }
        );

        customer.Name.Should().Be("Contoso");
        customer.Address.City.Should().Be("Seattle");
    }
}

/// <summary>
/// Tests configuration validation behavior.
/// </summary>
/// <remarks>
/// Covers successful validation, unmapped member failures, and ignored members.
/// </remarks>
public class ValidationTests
{
    /// <summary>
    /// Verifies that complete mappings pass configuration validation.
    /// </summary>
    /// <remarks>
    /// All writable destination members must be mapped or explicitly configured.
    /// </remarks>
    [Fact]
    public void AssertConfigurationIsValid_ShouldPassForCompleteMapping()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<PersonSource, PersonDestination>()
                .ForMember(
                    dest => dest.FullName,
                    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}")
                )
                .ForMember(
                    dest => dest.Age,
                    opt => opt.MapFrom(src => DateTime.UtcNow.Year - src.BirthDate.Year)
                );
        });

        var action = () => config.AssertConfigurationIsValid();

        action.Should().NotThrow();
    }

    /// <summary>
    /// Verifies that validation fails when destination members lack source mappings.
    /// </summary>
    /// <remarks>
    /// Unconfigured computed members on <see cref="PersonDestination"/> trigger errors.
    /// </remarks>
    [Fact]
    public void AssertConfigurationIsValid_ShouldThrowForUnmappedMember()
    {
        var config = new MapperConfiguration(cfg =>
            cfg.CreateMap<PersonSource, PersonDestination>()
        );

        var action = () => config.AssertConfigurationIsValid();

        action
            .Should()
            .Throw<DotRMapperConfigurationException>()
            .WithMessage("*FullName*")
            .WithMessage("*Age*");
    }

    /// <summary>
    /// Verifies that ignored members do not cause validation failures.
    /// </summary>
    /// <remarks>
    /// Uses <see cref="EmployeeProfile"/> which ignores badge identifier mapping.
    /// </remarks>
    [Fact]
    public void AssertConfigurationIsValid_ShouldIgnoreIgnoredMembers()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<EmployeeProfile>());

        var action = () => config.AssertConfigurationIsValid();

        action.Should().NotThrow();
    }
}

/// <summary>
/// Tests enum value mapping between source and destination types.
/// </summary>
/// <remarks>
/// Verifies name-based enum conversion during member assignment.
/// </remarks>
public class EnumMappingTests
{
    /// <summary>
    /// Verifies that enums with matching member names map correctly.
    /// </summary>
    /// <remarks>
    /// Maps <see cref="StatusSource"/> to <see cref="StatusDestination"/> by name.
    /// </remarks>
    [Fact]
    public void Map_ShouldMapMatchingEnumValues()
    {
        var config = new MapperConfiguration(cfg =>
            cfg.CreateMap<StatusEntitySource, StatusEntityDestination>()
        );
        var mapper = config.CreateMapper();

        var result = mapper.Map<StatusEntitySource, StatusEntityDestination>(
            new StatusEntitySource { Status = StatusSource.Active }
        );

        result.Status.Should().Be(StatusDestination.Active);
    }
}
