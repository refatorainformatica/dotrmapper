namespace DotRMapper.Abstractions;

/// <summary>
/// Provides contextual information during a mapping operation.
/// </summary>
/// <remarks>
/// Passed to value resolvers, type converters, and BeforeMap/AfterMap callbacks that accept context.
/// Holds the root source and destination objects for the current mapping invocation.
/// </remarks>
public sealed class ResolutionContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResolutionContext"/> class.
    /// </summary>
    /// <remarks>
    /// Called internally by the mapping engine when a mapping operation starts.
    /// </remarks>
    /// <param name="mapper">The mapper performing the operation.</param>
    /// <param name="source">The root source object.</param>
    /// <param name="destination">The root destination object.</param>
    internal ResolutionContext(IMapper mapper, object? source, object? destination)
    {
        Mapper = mapper;
        Source = source;
        Destination = destination;
    }

    /// <summary>
    /// Gets the mapper instance performing the current operation.
    /// </summary>
    /// <remarks>
    /// Enables nested mapping from custom resolvers and callbacks.
    /// </remarks>
    public IMapper Mapper { get; }

    /// <summary>
    /// Gets the root source object of the current mapping operation.
    /// </summary>
    /// <remarks>
    /// Refers to the top-level source, not intermediate nested objects.
    /// </remarks>
    public object? Source { get; }

    /// <summary>
    /// Gets the root destination object of the current mapping operation.
    /// </summary>
    /// <remarks>
    /// Refers to the top-level destination, not intermediate nested objects.
    /// </remarks>
    public object? Destination { get; }
}
