namespace DotRMapper.Abstractions.Resolvers;

/// <summary>
/// Defines a custom value resolver for a destination member.
/// </summary>
/// <remarks>
/// Register via the generic <c>MapFrom&lt;TValueResolver&gt;()</c> member configuration overload.
/// Implementations must expose a parameterless constructor for activation.
/// </remarks>
/// <typeparam name="TSource">The source type.</typeparam>
/// <typeparam name="TDestination">The destination type.</typeparam>
/// <typeparam name="TDestMember">The destination member type.</typeparam>
public interface IValueResolver<in TSource, in TDestination, TDestMember>
{
    /// <summary>
    /// Resolves the destination member value from the source object.
    /// </summary>
    /// <remarks>
    /// Called once per mapped instance. <paramref name="destMember"/> holds the current destination
    /// property value before assignment.
    /// </remarks>
    /// <param name="source">The source object.</param>
    /// <param name="destination">The destination object.</param>
    /// <param name="destMember">The current destination member value.</param>
    /// <param name="context">The resolution context.</param>
    /// <returns>The resolved destination member value.</returns>
    TDestMember Resolve(
        TSource source,
        TDestination destination,
        TDestMember destMember,
        ResolutionContext context
    );
}
