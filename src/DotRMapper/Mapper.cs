using DotRMapper.Abstractions;
using DotRMapper.Internal;

namespace DotRMapper;

/// <summary>
/// Default implementation of <see cref="IMapper"/>.
/// </summary>
/// <remarks>
/// Created by <see cref="MapperConfiguration.CreateMapper"/>. Delegates mapping work to
/// <see cref="MappingEngine"/>.
/// </remarks>
public sealed class Mapper : IMapper
{
    /// <summary>
    /// The engine that executes configured and convention-based mappings.
    /// </summary>
    /// <remarks>
    /// Holds a reference to this mapper for nested resolution and callbacks.
    /// </remarks>
    private readonly MappingEngine _engine;

    /// <summary>
    /// Initializes a new instance of the <see cref="Mapper"/> class.
    /// </summary>
    /// <remarks>
    /// Called internally by <see cref="MapperConfiguration.CreateMapper"/>.
    /// </remarks>
    /// <param name="registry">The type map registry built during configuration.</param>
    internal Mapper(TypeMapRegistry registry)
    {
        _engine = new MappingEngine(registry, this);
    }

    /// <summary>
    /// Maps the source object to a new instance of <typeparamref name="TDestination"/>.
    /// </summary>
    /// <remarks>
    /// Infers the source type at runtime. Throws when <paramref name="source"/> is null.
    /// </remarks>
    /// <typeparam name="TDestination">The destination type.</typeparam>
    /// <param name="source">The source object.</param>
    /// <returns>A new instance of <typeparamref name="TDestination"/>.</returns>
    public TDestination Map<TDestination>(object source)
    {
        ArgumentNullException.ThrowIfNull(source);
        return (TDestination)_engine.Map(source, typeof(TDestination));
    }

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
    public TDestination Map<TSource, TDestination>(TSource source)
    {
        if (source is null)
        {
            return default!;
        }

        return (TDestination)_engine.Map(source, typeof(TDestination));
    }

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
    public object Map(object source, Type destinationType)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destinationType);
        return _engine.Map(source, destinationType);
    }

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
    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        if (source is null)
        {
            return destination;
        }

        _engine.MapOnto(source, destination!);
        return destination;
    }

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
    public IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source)
    {
        ArgumentNullException.ThrowIfNull(source);
        return source.Select(item => Map<TSource, TDestination>(item)).ToList();
    }
}
