using DotRMapper.Abstractions.Converters;
using DotRMapper.Abstractions.Resolvers;
using DotRMapper.Tests.Models;
using DotRMapper.Tests.Profiles;
using FluentAssertions;

namespace DotRMapper.Tests;

/// <summary>
/// Tests custom member configuration, callbacks, resolvers, and type converters.
/// </summary>
/// <remarks>
/// Covers ForMember, Ignore, BeforeMap, AfterMap, value resolvers, and ConvertUsing.
/// </remarks>
public class CustomMappingTests
{
    /// <summary>
    /// Verifies that ForMember MapFrom expressions populate custom destination members.
    /// </summary>
    /// <remarks>
    /// Uses <see cref="PersonProfile"/> to compute full name and age.
    /// </remarks>
    [Fact]
    public void ForMember_MapFrom_ShouldUseCustomExpression()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<PersonProfile>());
        var mapper = config.CreateMapper();

        var source = new PersonSource
        {
            FirstName = "Alice",
            LastName = "Wonder",
            BirthDate = new DateTime(2000, 1, 1),
        };

        var result = mapper.Map<PersonSource, PersonDestination>(source);

        result.FullName.Should().Be("Alice Wonder");
        result.Age.Should().Be(DateTime.UtcNow.Year - 2000);
    }

    /// <summary>
    /// Verifies that ignored destination members retain their default values.
    /// </summary>
    /// <remarks>
    /// Uses <see cref="EmployeeProfile"/> which ignores badge identifier mapping.
    /// </remarks>
    [Fact]
    public void ForMember_Ignore_ShouldSkipDestinationMember()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<EmployeeProfile>());
        var mapper = config.CreateMapper();

        var source = new EmployeeSource
        {
            FirstName = "Bob",
            LastName = "Builder",
            Department = "Engineering",
        };

        var result = mapper.Map<EmployeeSource, EmployeeDestination>(source);

        result.FirstName.Should().Be("Bob");
        result.LastName.Should().Be("Builder");
        result.Department.Should().Be("Engineering");
        result.BadgeId.Should().BeEmpty();
    }

    /// <summary>
    /// Verifies that AfterMap callbacks run after member assignment completes.
    /// </summary>
    /// <remarks>
    /// The callback sets a destination-only audit field.
    /// </remarks>
    [Fact]
    public void AfterMap_ShouldExecuteCallbackAfterMapping()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<AuditSource, AuditDestination>()
                .AfterMap((src, dest) => dest.CreatedBy = "system");
        });

        var mapper = config.CreateMapper();
        var result = mapper.Map<AuditSource, AuditDestination>(new AuditSource { Name = "Record" });

        result.Name.Should().Be("Record");
        result.CreatedBy.Should().Be("system");
    }

    /// <summary>
    /// Verifies that BeforeMap callbacks run before member assignment begins.
    /// </summary>
    /// <remarks>
    /// The callback mutates the source password before it is copied to the destination.
    /// </remarks>
    [Fact]
    public void BeforeMap_ShouldExecuteCallbackBeforeMapping()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserSource, UserDestination>()
                .BeforeMap((src, dest) => src.Password = "masked");
        });

        var mapper = config.CreateMapper();
        var source = new UserSource { Username = "admin", Password = "secret" };
        var result = mapper.Map<UserSource, UserDestination>(source);

        result.Username.Should().Be("admin");
        result.Password.Should().Be("masked");
    }

    /// <summary>
    /// Verifies that a registered value resolver supplies the destination member value.
    /// </summary>
    /// <remarks>
    /// Uses <see cref="PriceDisplayResolver"/> to format price as currency text.
    /// </remarks>
    [Fact]
    public void MapFrom_ValueResolver_ShouldResolveMemberValue()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ProductSource, ProductDestination>()
                .ForMember(dest => dest.DisplayPrice, opt => opt.MapFrom<PriceDisplayResolver>());
        });

        var mapper = config.CreateMapper();
        var result = mapper.Map<ProductSource, ProductDestination>(
            new ProductSource { Name = "Widget", Price = 19.99m }
        );

        result.DisplayPrice.Should().Be("$19.99");
    }

    /// <summary>
    /// Verifies that a type converter transforms the source value to the destination member type.
    /// </summary>
    /// <remarks>
    /// Uses <see cref="DecimalToCurrencyConverter"/> to format decimal amounts as strings.
    /// </remarks>
    [Fact]
    public void ConvertUsing_ShouldApplyTypeConverter()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<AmountSource, AmountDestination>()
                .ForMember(
                    dest => dest.Amount,
                    opt => opt.ConvertUsing<DecimalToCurrencyConverter>()
                );
        });

        var mapper = config.CreateMapper();
        var result = mapper.Map<AmountSource, AmountDestination>(
            new AmountSource { Amount = 42.5m }
        );

        result.Amount.Should().Be("42.50");
    }

    /// <summary>
    /// Source model with a decimal amount for converter tests.
    /// </summary>
    /// <remarks>
    /// Used only within <see cref="CustomMappingTests"/>.
    /// </remarks>
    private sealed class AmountSource
    {
        /// <summary>
        /// Gets or sets the monetary amount.
        /// </summary>
        public decimal Amount { get; set; }
    }

    /// <summary>
    /// Destination model with a string amount for converter tests.
    /// </summary>
    /// <remarks>
    /// Used only within <see cref="CustomMappingTests"/>.
    /// </remarks>
    private sealed class AmountDestination
    {
        /// <summary>
        /// Gets or sets the formatted amount text.
        /// </summary>
        public string Amount { get; set; } = string.Empty;
    }

    /// <summary>
    /// Resolves product display price from the source price value.
    /// </summary>
    /// <remarks>
    /// Formats the price with two decimal places and a leading currency symbol.
    /// </remarks>
    private sealed class PriceDisplayResolver
        : IValueResolver<ProductSource, ProductDestination, string>
    {
        /// <summary>
        /// Returns a currency-formatted display price for the product.
        /// </summary>
        /// <remarks>
        /// Uses standard two-decimal formatting prefixed with a dollar sign.
        /// </remarks>
        /// <param name="source">The source product.</param>
        /// <param name="destination">The destination product.</param>
        /// <param name="destMember">The current display price value.</param>
        /// <param name="context">The resolution context.</param>
        /// <returns>The formatted display price.</returns>
        public string Resolve(
            ProductSource source,
            ProductDestination destination,
            string destMember,
            ResolutionContext context
        ) => $"${source.Price:F2}";
    }

    /// <summary>
    /// Converts decimal source values to fixed-point currency strings.
    /// </summary>
    /// <remarks>
    /// Returns null when the source value is not a decimal.
    /// </remarks>
    private sealed class DecimalToCurrencyConverter : ITypeConverter
    {
        /// <summary>
        /// Converts a decimal value to a two-decimal string representation.
        /// </summary>
        /// <remarks>
        /// Non-decimal source values yield null.
        /// </remarks>
        /// <param name="source">The source value.</param>
        /// <param name="destinationType">The destination member type.</param>
        /// <param name="context">The resolution context.</param>
        /// <returns>The formatted string, or null.</returns>
        public object? Convert(object? source, Type destinationType, ResolutionContext context)
        {
            return source is decimal value ? value.ToString("F2") : null;
        }
    }
}
