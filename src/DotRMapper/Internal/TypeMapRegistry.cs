using DotRMapper.Exceptions;

namespace DotRMapper.Internal;

/// <summary>
/// Stores and validates configured type maps for source-destination pairs.
/// </summary>
/// <remarks>
/// Built during <see cref="MapperConfiguration"/> initialization and shared by mapper instances.
/// </remarks>
internal sealed class TypeMapRegistry
{
    /// <summary>
    /// The configured type maps keyed by source and destination runtime types.
    /// </summary>
    /// <remarks>
    /// Each key pair maps to a single <see cref="TypeMapConfiguration"/> instance.
    /// </remarks>
    private readonly Dictionary<(Type Source, Type Destination), TypeMapConfiguration> _typeMaps =
        new();

    /// <summary>
    /// Gets an existing type map or creates a new one for the specified pair.
    /// </summary>
    /// <remarks>
    /// Subsequent calls with the same types return the same configuration instance.
    /// </remarks>
    /// <param name="sourceType">The source type.</param>
    /// <param name="destinationType">The destination type.</param>
    /// <returns>The type map configuration for the pair.</returns>
    public TypeMapConfiguration GetOrAdd(Type sourceType, Type destinationType)
    {
        var key = (sourceType, destinationType);
        if (_typeMaps.TryGetValue(key, out var existing))
        {
            return existing;
        }

        var typeMap = new TypeMapConfiguration
        {
            SourceType = sourceType,
            DestinationType = destinationType,
        };

        _typeMaps[key] = typeMap;
        return typeMap;
    }

    /// <summary>
    /// Finds a configured type map for the specified source and destination types.
    /// </summary>
    /// <remarks>
    /// Returns null when no explicit configuration exists for the pair.
    /// </remarks>
    /// <param name="sourceType">The source type.</param>
    /// <param name="destinationType">The destination type.</param>
    /// <returns>The matching type map configuration, or null.</returns>
    public TypeMapConfiguration? Find(Type sourceType, Type destinationType)
    {
        _typeMaps.TryGetValue((sourceType, destinationType), out var typeMap);
        return typeMap;
    }

    /// <summary>
    /// Returns all configured type maps.
    /// </summary>
    /// <remarks>
    /// Used during configuration validation.
    /// </remarks>
    /// <returns>An enumerable of all registered type map configurations.</returns>
    public IEnumerable<TypeMapConfiguration> GetAll() => _typeMaps.Values;

    /// <summary>
    /// Validates that all configured mappings are complete and consistent.
    /// </summary>
    /// <remarks>
    /// Reports unmapped writable destination members and convention mappings without a source property.
    /// </remarks>
    /// <exception cref="DotRMapperConfigurationException">
    /// Thrown when one or more validation errors are found.
    /// </exception>
    public void AssertConfigurationIsValid()
    {
        var errors = new List<string>();

        foreach (var typeMap in _typeMaps.Values)
        {
            var writableProperties = typeMap
                .DestinationType.GetProperties(
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public
                )
                .Where(p => p.CanWrite && p.GetSetMethod() is not null)
                .ToList();

            foreach (var property in writableProperties)
            {
                var mapping = typeMap.PropertyMappings.FirstOrDefault(m =>
                    m.DestinationProperty.Name.Equals(
                        property.Name,
                        StringComparison.OrdinalIgnoreCase
                    )
                );

                if (mapping is null)
                {
                    errors.Add(
                        $"Unmapped destination member '{property.Name}' on mapping "
                            + $"{typeMap.SourceType.Name} -> {typeMap.DestinationType.Name}."
                    );
                    continue;
                }

                if (mapping.IsIgnored)
                {
                    continue;
                }

                if (mapping.Kind == MemberMappingKind.Convention && mapping.SourceProperty is null)
                {
                    errors.Add(
                        $"No source member found for destination member '{property.Name}' on mapping "
                            + $"{typeMap.SourceType.Name} -> {typeMap.DestinationType.Name}."
                    );
                }
            }
        }

        if (errors.Count > 0)
        {
            throw new DotRMapperConfigurationException(
                "Mapper configuration is invalid:"
                    + Environment.NewLine
                    + string.Join(Environment.NewLine, errors)
            );
        }
    }
}
