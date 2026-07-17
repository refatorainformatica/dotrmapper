using DotRMapper.Tests.Models;
using DotRMapper.Tests.Profiles;
using FluentAssertions;

namespace DotRMapper.Tests;

/// <summary>
/// Tests convention-based property mapping behavior.
/// </summary>
/// <remarks>
/// Verifies name-based matching, in-place mapping, and null source handling.
/// </remarks>
public class ConventionMappingTests
{
    /// <summary>
    /// Verifies that properties with matching names are copied by convention.
    /// </summary>
    /// <remarks>
    /// Computed destination members are ignored so only convention matches are asserted.
    /// </remarks>
    [Fact]
    public void Map_ShouldCopyMatchingPropertiesByConvention()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<PersonSource, PersonDestination>()
                .ForMember(dest => dest.FullName, opt => opt.Ignore())
                .ForMember(dest => dest.Age, opt => opt.Ignore());
        });

        var mapper = config.CreateMapper();
        var source = new PersonSource
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            BirthDate = new DateTime(1990, 5, 10),
        };

        var result = mapper.Map<PersonSource, PersonDestination>(source);

        result.Id.Should().Be(1);
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
    }

    /// <summary>
    /// Verifies that mapping onto an existing destination updates that instance.
    /// </summary>
    /// <remarks>
    /// The returned reference must be the same object passed as destination.
    /// </remarks>
    [Fact]
    public void MapOnto_ShouldUpdateExistingDestinationInstance()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<PersonSource, PersonDestination>()
                .ForMember(dest => dest.FullName, opt => opt.Ignore())
                .ForMember(dest => dest.Age, opt => opt.Ignore());
        });

        var mapper = config.CreateMapper();
        var source = new PersonSource
        {
            Id = 2,
            FirstName = "Jane",
            LastName = "Smith",
        };
        var destination = new PersonDestination
        {
            Id = 99,
            FirstName = "Old",
            LastName = "Name",
        };

        var result = mapper.Map(source, destination);

        result.Should().BeSameAs(destination);
        result.Id.Should().Be(2);
        result.FirstName.Should().Be("Jane");
        result.LastName.Should().Be("Smith");
    }

    /// <summary>
    /// Verifies that a null source returns the default destination value.
    /// </summary>
    /// <remarks>
    /// Applies to the generic two-type Map overload with a reference type destination.
    /// </remarks>
    [Fact]
    public void Map_WithNullSource_ShouldReturnDefault()
    {
        var config = new MapperConfiguration(cfg =>
            cfg.CreateMap<PersonSource, PersonDestination>()
        );
        var mapper = config.CreateMapper();

        var result = mapper.Map<PersonSource, PersonDestination>((PersonSource?)null!);

        result.Should().BeNull();
    }
}
