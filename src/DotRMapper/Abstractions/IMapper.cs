namespace DotRMapper.Abstractions;

/// <summary>
/// Defines the contract for mapping objects from one type to another.
/// </summary>
/// <remarks>
/// Create instances via <see cref="MapperConfiguration.CreateMapper"/>.
/// Mapping behavior depends on registered type maps and convention-based rules.
/// </remarks>
public interface IMapper
{
    /// <summary>
    /// Maps the source object to a new instance of <typeparamref name="TDestination"/>.
    /// </summary>
    /// <remarks>
    /// Infers the source type at runtime. Throws when <paramref name="source"/> is null.
    /// </remarks>
    /// <typeparam name="TDestination">The destination type.</typeparam>
    /// <param name="source">The source object.</param>
    /// <returns>A new instance of <typeparamref name="TDestination"/>.</returns>
    TDestination Map<TDestination>(object source);

    /// <summary>
    /// Maps the source object to a new instance of <typeparamref name="TDestination"/>.
    /// </summary>
    /// <remarks>
    /// Returns the default value of <typeparamref name="TDestination"/> when <paramref name="source"/> is null.
    /// </remarks>
    /// <typeparam name="TSource">The source type.</typeparam>
    /// <typeparam name="TDestination">The destination type.</typeparam>
    /// <param name="source">The source object.</param>
    /// <returns>A new instance of <typeparamref name="TDestination"/>.</returns>
    TDestination Map<TSource, TDestination>(TSource source);

    /// <summary>
    /// Maps the source object to a new instance of the specified destination type.
    /// </summary>
    /// <remarks>
    /// Uses runtime types for mapping resolution. Throws when <paramref name="source"/> or
    /// <paramref name="destinationType"/> is null.
    /// </remarks>
    /// <param name="source">The source object.</param>
    /// <param name="destinationType">The destination type.</param>
    /// <returns>A new instance of <paramref name="destinationType"/>.</returns>
    object Map(object source, Type destinationType);

    /// <summary>
    /// Maps the source object onto an existing destination instance.
    /// </summary>
    /// <remarks>
    /// Returns the existing <paramref name="destination"/> unchanged when <paramref name="source"/> is null.
    /// </remarks>
    /// <typeparam name="TSource">The source type.</typeparam>
    /// <typeparam name="TDestination">The destination type.</typeparam>
    /// <param name="source">The source object.</param>
    /// <param name="destination">The destination object to populate.</param>
    /// <returns>The populated destination instance.</returns>
    TDestination Map<TSource, TDestination>(TSource source, TDestination destination);

    /// <summary>
    /// Maps a collection of source objects to a collection of destination objects.
    /// </summary>
    /// <remarks>
    /// Maps each element individually and returns a materialized list. Throws when
    /// <paramref name="source"/> is null.
    /// </remarks>
    /// <typeparam name="TSource">The source element type.</typeparam>
    /// <typeparam name="TDestination">The destination element type.</typeparam>
    /// <param name="source">The source collection.</param>
    /// <returns>A list containing mapped destination objects.</returns>
    IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source);
}
