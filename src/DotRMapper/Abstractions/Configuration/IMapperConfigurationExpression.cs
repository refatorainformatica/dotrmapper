namespace DotRMapper.Abstractions.Configuration;

/// <summary>
/// Defines the contract for configuring type mappings.
/// </summary>
/// <remarks>
/// Passed to the <see cref="MapperConfiguration"/> configuration delegate.
/// Implemented by the internal configuration expression type created by <see cref="MapperConfiguration"/>.
/// </remarks>
public interface IMapperConfigurationExpression
{
    /// <summary>
    /// Creates or updates a mapping between <typeparamref name="TSource"/> and <typeparamref name="TDestination"/>.
    /// </summary>
    /// <remarks>
    /// Convention-based property matching is applied for writable destination members.
    /// </remarks>
    /// <typeparam name="TSource">The source type.</typeparam>
    /// <typeparam name="TDestination">The destination type.</typeparam>
    /// <returns>The mapping configuration expression.</returns>
    IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>();

    /// <summary>
    /// Adds a profile that groups related mappings.
    /// </summary>
    /// <remarks>
    /// Instantiates <typeparamref name="TProfile"/> with a parameterless constructor.
    /// </remarks>
    /// <typeparam name="TProfile">The profile type.</typeparam>
    void AddProfile<TProfile>()
        where TProfile : Profile, new();

    /// <summary>
    /// Adds a profile instance that groups related mappings.
    /// </summary>
    /// <remarks>
    /// Useful when the profile requires constructor arguments or shared state.
    /// </remarks>
    /// <param name="profile">The profile instance.</param>
    void AddProfile(Profile profile);
}
