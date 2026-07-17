using DotRMapper.Abstractions;
using DotRMapper.Abstractions.Configuration;
using DotRMapper.Configuration;
using DotRMapper.Internal;

namespace DotRMapper;

/// <summary>
/// Stores and validates mapping configuration and creates <see cref="IMapper"/> instances.
/// </summary>
/// <remarks>
/// Build via a configuration delegate or an array of <see cref="Profile"/> instances.
/// Call <see cref="AssertConfigurationIsValid"/> before production use to detect unmapped members.
/// </remarks>
public sealed class MapperConfiguration
{
    /// <summary>
    /// The registry of configured type maps built during initialization.
    /// </summary>
    /// <remarks>
    /// Shared by all mappers created from this configuration instance.
    /// </remarks>
    private readonly TypeMapRegistry _registry;

    /// <summary>
    /// Initializes a new instance of the <see cref="MapperConfiguration"/> class using a configuration delegate.
    /// </summary>
    /// <remarks>
    /// The delegate receives an <see cref="IMapperConfigurationExpression"/> to register mappings and profiles.
    /// </remarks>
    /// <param name="configure">The configuration delegate.</param>
    public MapperConfiguration(Action<IMapperConfigurationExpression> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);

        var expression = new MapperConfigurationExpression();
        configure(expression);
        _registry = expression.Registry;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MapperConfiguration"/> class using a profile collection.
    /// </summary>
    /// <remarks>
    /// Each profile is registered in the order provided.
    /// </remarks>
    /// <param name="profiles">The profiles to register.</param>
    public MapperConfiguration(params Profile[] profiles)
    {
        ArgumentNullException.ThrowIfNull(profiles);

        var expression = new MapperConfigurationExpression();
        foreach (var profile in profiles)
        {
            expression.AddProfile(profile);
        }

        _registry = expression.Registry;
    }

    /// <summary>
    /// Creates an <see cref="IMapper"/> instance based on this configuration.
    /// </summary>
    /// <remarks>
    /// Each call returns a new mapper sharing the same underlying type map registry.
    /// </remarks>
    /// <returns>A configured mapper instance.</returns>
    public IMapper CreateMapper()
    {
        var mapper = new Mapper(_registry);
        return mapper;
    }

    /// <summary>
    /// Validates that all configured mappings are complete and consistent.
    /// </summary>
    /// <remarks>
    /// Checks that every writable destination member has a source or is explicitly ignored.
    /// </remarks>
    /// <exception cref="Exceptions.DotRMapperConfigurationException">
    /// Thrown when one or more mappings are invalid.
    /// </exception>
    public void AssertConfigurationIsValid()
    {
        _registry.AssertConfigurationIsValid();
    }
}
