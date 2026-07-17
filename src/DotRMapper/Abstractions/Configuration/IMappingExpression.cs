using System.Linq.Expressions;

namespace DotRMapper.Abstractions.Configuration;

/// <summary>
/// Defines the contract for configuring a single type mapping.
/// </summary>
/// <remarks>
/// Returned by <see cref="IMapperConfigurationExpression.CreateMap{TSource, TDestination}"/>.
/// Supports fluent chaining for member overrides and lifecycle callbacks.
/// </remarks>
/// <typeparam name="TSource">The source type.</typeparam>
/// <typeparam name="TDestination">The destination type.</typeparam>
public interface IMappingExpression<TSource, TDestination>
{
    /// <summary>
    /// Configures a custom mapping for an individual destination member.
    /// </summary>
    /// <remarks>
    /// Overrides convention-based matching for the specified member.
    /// </remarks>
    /// <typeparam name="TMember">The destination member type.</typeparam>
    /// <param name="destinationMember">An expression identifying the destination member.</param>
    /// <param name="memberOptions">Configuration actions for the member.</param>
    /// <returns>The same mapping expression for chaining.</returns>
    IMappingExpression<TSource, TDestination> ForMember<TMember>(
        Expression<Func<TDestination, TMember>> destinationMember,
        Action<IMemberConfigurationExpression<TSource, TDestination, TMember>> memberOptions
    );

    /// <summary>
    /// Configures a callback invoked before the mapping is executed.
    /// </summary>
    /// <remarks>
    /// Runs after the destination instance is created but before members are assigned.
    /// </remarks>
    /// <param name="beforeFunction">The callback to invoke.</param>
    /// <returns>The same mapping expression for chaining.</returns>
    IMappingExpression<TSource, TDestination> BeforeMap(
        Action<TSource, TDestination> beforeFunction
    );

    /// <summary>
    /// Configures a callback invoked before the mapping is executed with access to the resolution context.
    /// </summary>
    /// <remarks>
    /// Runs after the destination instance is created but before members are assigned.
    /// </remarks>
    /// <param name="beforeFunction">The callback to invoke.</param>
    /// <returns>The same mapping expression for chaining.</returns>
    IMappingExpression<TSource, TDestination> BeforeMap(
        Action<TSource, TDestination, ResolutionContext> beforeFunction
    );

    /// <summary>
    /// Configures a callback invoked after the mapping is executed.
    /// </summary>
    /// <remarks>
    /// Runs after all member assignments and nested mappings complete.
    /// </remarks>
    /// <param name="afterFunction">The callback to invoke.</param>
    /// <returns>The same mapping expression for chaining.</returns>
    IMappingExpression<TSource, TDestination> AfterMap(Action<TSource, TDestination> afterFunction);

    /// <summary>
    /// Configures a callback invoked after the mapping is executed with access to the resolution context.
    /// </summary>
    /// <remarks>
    /// Runs after all member assignments and nested mappings complete.
    /// </remarks>
    /// <param name="afterFunction">The callback to invoke.</param>
    /// <returns>The same mapping expression for chaining.</returns>
    IMappingExpression<TSource, TDestination> AfterMap(
        Action<TSource, TDestination, ResolutionContext> afterFunction
    );

    /// <summary>
    /// Creates a reverse mapping from <typeparamref name="TDestination"/> to <typeparamref name="TSource"/>.
    /// </summary>
    /// <remarks>
    /// Copies non-ignored member mappings by matching property names case-insensitively.
    /// </remarks>
    /// <returns>The reverse mapping expression.</returns>
    IMappingExpression<TDestination, TSource> ReverseMap();
}
