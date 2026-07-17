using DotRMapper.Tests.Models;
using DotRMapper.Tests.Profiles;
using FluentAssertions;

namespace DotRMapper.Tests;

/// <summary>
/// Tests nested object and collection mapping behavior.
/// </summary>
/// <remarks>
/// Covers nested graphs, array-to-list conversion, and enumerable batch mapping.
/// </remarks>
public class CollectionMappingTests
{
    /// <summary>
    /// Verifies that nested objects and primitive collections are mapped recursively.
    /// </summary>
    /// <remarks>
    /// Uses <see cref="CustomerProfile"/> for address and customer configuration.
    /// </remarks>
    [Fact]
    public void Map_ShouldMapNestedObjects()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CustomerProfile>());
        var mapper = config.CreateMapper();

        var source = new CustomerSource
        {
            Id = 10,
            Name = "Acme Corp",
            Address = new AddressSource { Street = "123 Main St", City = "Springfield" },
            Tags = ["vip", "wholesale"],
        };

        var result = mapper.Map<CustomerSource, CustomerDestination>(source);

        result.Id.Should().Be(10);
        result.Name.Should().Be("Acme Corp");
        result.Address.Street.Should().Be("123 Main St");
        result.Address.City.Should().Be("Springfield");
        result.Tags.Should().BeEquivalentTo(["vip", "wholesale"]);
    }

    /// <summary>
    /// Verifies that source arrays map to destination lists element by element.
    /// </summary>
    /// <remarks>
    /// Uses <see cref="OrderProfile"/> which registers item and order mappings.
    /// </remarks>
    [Fact]
    public void Map_ShouldMapArrayToList()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<OrderProfile>());
        var mapper = config.CreateMapper();

        var source = new OrderSource
        {
            OrderId = 500,
            Items =
            [
                new OrderItemSource { ProductName = "Pen", Price = 1.5m },
                new OrderItemSource { ProductName = "Notebook", Price = 4.0m },
            ],
        };

        var result = mapper.Map<OrderSource, OrderDestination>(source);

        result.OrderId.Should().Be(500);
        result.Items.Should().HaveCount(2);
        result.Items[0].ProductName.Should().Be("Pen");
        result.Items[1].Price.Should().Be(4.0m);
    }

    /// <summary>
    /// Verifies that the collection Map overload maps each element to the destination type.
    /// </summary>
    /// <remarks>
    /// Returns a materialized list of mapped destination items.
    /// </remarks>
    [Fact]
    public void MapCollection_ShouldMapEnumerableToList()
    {
        var config = new MapperConfiguration(cfg =>
            cfg.CreateMap<OrderItemSource, OrderItemDestination>()
        );
        var mapper = config.CreateMapper();

        var source = new[]
        {
            new OrderItemSource { ProductName = "A", Price = 1m },
            new OrderItemSource { ProductName = "B", Price = 2m },
        };

        var result = mapper.Map<OrderItemSource, OrderItemDestination>(source);

        result.Should().HaveCount(2);
        result.Last().ProductName.Should().Be("B");
    }
}
