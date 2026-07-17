using DotRMapper.Abstractions.Configuration;
using DotRMapper.Internal;

namespace DotRMapper.Configuration;

/// <summary>
/// Default implementation of <see cref="IMapperConfigurationExpression"/>.
/// </summary>
/// <remarks>
/// Created internally when building a <see cref="MapperConfiguration"/>.
/// Owns the <see cref="TypeMapRegistry"/> populated by mapping configuration calls.
/// </remarks>
internal sealed class MapperConfigurationExpression : IMapperConfigurationExpression
{
    /// <summary>
    /// Gets the type map registry populated during configuration.
    /// </summary>
    /// <remarks>
    /// Passed to <see cref="Mapper"/> when the configuration is finalized.
    /// </remarks>
    internal TypeMapRegistry Registry { get; } = new();

    /// <summary>
    /// Creates or updates a mapping between <typeparamref name="TSource"/> and <typeparamref name="TDestination"/>.
    /// </summary>
    /// <remarks>
    /// Convention-based property matching is applied for writable destination members.
    /// </remarks>
    /// <typeparam name="TSource">The source type.</typeparam>
    /// <typeparam name="TDestination">The destination type.</typeparam>
    /// <returns>The mapping configuration expression.</returns>
    public IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>()
    {
        var typeMap = Registry.GetOrAdd(typeof(TSource), typeof(TDestination));
        return new MappingExpression<TSource, TDestination>(Registry, typeMap);
    }

    /// <summary>
    /// Adds a profile that groups related mappings.
    /// </summary>
    /// <remarks>
    /// Instantiates <typeparamref name="TProfile"/> with a parameterless constructor.
    /// </remarks>
    /// <typeparam name="TProfile">The profile type.</typeparam>
    public void AddProfile<TProfile>()
        where TProfile : Profile, new()
    {
        AddProfile(new TProfile());
    }

    /// <summary>
    /// Adds a profile instance that groups related mappings.
    /// </summary>
    /// <remarks>
    /// Useful when the profile requires constructor arguments or shared state.
    /// </remarks>
    /// <param name="profile">The profile instance.</param>
    public void AddProfile(Profile profile)
    {
        profile.Configure(this);
    }
}
