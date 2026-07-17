namespace DotRMapper.Abstractions.Configuration;

/// <summary>
/// Base class for grouping related mapping configurations.
/// </summary>
/// <remarks>
/// Override <see cref="ConfigureMappings"/> and call <see cref="CreateMap{TSource, TDestination}"/>
/// to define mappings. Register profiles via <see cref="IMapperConfigurationExpression.AddProfile{TProfile}"/>.
/// </remarks>
public abstract class Profile
{
    /// <summary>
    /// Holds the configuration expression set during profile registration.
    /// </summary>
    /// <remarks>
    /// Null until <see cref="Configure"/> is invoked by the configuration pipeline.
    /// </remarks>
    private IMapperConfigurationExpression? _configurationExpression;

    /// <summary>
    /// Initializes the profile and invokes <see cref="ConfigureMappings"/>.
    /// </summary>
    /// <remarks>
    /// Called internally when the profile is added to a <see cref="MapperConfiguration"/>.
    /// </remarks>
    /// <param name="configurationExpression">The parent configuration expression.</param>
    internal void Configure(IMapperConfigurationExpression configurationExpression)
    {
        _configurationExpression = configurationExpression;
        ConfigureMappings();
    }

    /// <summary>
    /// Override this method to define mappings for this profile.
    /// </summary>
    /// <remarks>
    /// Default implementation performs no configuration. Called once during profile registration.
    /// </remarks>
    protected virtual void ConfigureMappings() { }

    /// <summary>
    /// Creates or updates a mapping between <typeparamref name="TSource"/> and <typeparamref name="TDestination"/>.
    /// </summary>
    /// <remarks>
    /// Throws <see cref="InvalidOperationException"/> when called before the profile is initialized.
    /// </remarks>
    /// <typeparam name="TSource">The source type.</typeparam>
    /// <typeparam name="TDestination">The destination type.</typeparam>
    /// <returns>The mapping configuration expression.</returns>
    protected IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>() =>
        (
            _configurationExpression
            ?? throw new InvalidOperationException("Profile has not been initialized.")
        ).CreateMap<TSource, TDestination>();
}
