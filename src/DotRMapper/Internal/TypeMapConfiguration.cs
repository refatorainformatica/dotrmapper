using System.Linq.Expressions;
using System.Reflection;
using DotRMapper.Abstractions;
using DotRMapper.Abstractions.Converters;
using DotRMapper.Abstractions.Resolvers;

namespace DotRMapper.Internal;

/// <summary>
/// Identifies how a destination member value is resolved during mapping.
/// </summary>
/// <remarks>
/// Stored on <see cref="PropertyMapping.Kind"/> to drive resolution in <see cref="MappingEngine"/>.
/// </remarks>
internal enum MemberMappingKind
{
    /// <summary>
    /// Matches source and destination properties by name using convention rules.
    /// </summary>
    /// <remarks>
    /// Default when no custom configuration overrides the member.
    /// </remarks>
    Convention,

    /// <summary>
    /// Resolves the member from a compiled source member expression.
    /// </summary>
    /// <remarks>
    /// Set by the expression-based <c>MapFrom</c> overload on member configuration.
    /// </remarks>
    Expression,

    /// <summary>
    /// Resolves the member from a delegate that receives only the source instance.
    /// </summary>
    /// <remarks>
    /// Reserved for delegate-based resolvers with a single source parameter.
    /// </remarks>
    Func,

    /// <summary>
    /// Resolves the member from a delegate that receives source and destination instances.
    /// </summary>
    /// <remarks>
    /// Set by the two-parameter <c>MapFrom</c> overload on member configuration.
    /// </remarks>
    FuncWithDestination,

    /// <summary>
    /// Resolves the member from a delegate that also receives the resolution context.
    /// </summary>
    /// <remarks>
    /// Set by the context-aware <c>MapFrom</c> overload on member configuration.
    /// </remarks>
    FuncWithContext,

    /// <summary>
    /// Resolves the member using an <see cref="IValueResolver{TSource, TDestination, TDestMember}"/> type.
    /// </summary>
    /// <remarks>
    /// The resolver type is activated per mapping invocation.
    /// </remarks>
    ValueResolver,

    /// <summary>
    /// Converts the convention-matched source value using an <see cref="ITypeConverter"/>.
    /// </summary>
    /// <remarks>
    /// Set by the <c>ConvertUsing</c> overload on member configuration.
    /// </remarks>
    TypeConverter,

    /// <summary>
    /// Skips mapping for the destination member.
    /// </summary>
    /// <remarks>
    /// Set by the <c>Ignore</c> overload on member configuration.
    /// </remarks>
    Ignored,
}

/// <summary>
/// Stores configuration for mapping a single destination property.
/// </summary>
/// <remarks>
/// Created and updated during mapping configuration and consumed by <see cref="MappingEngine"/>.
/// </remarks>
internal sealed class PropertyMapping
{
    /// <summary>
    /// Gets the destination property being mapped.
    /// </summary>
    public required PropertyInfo DestinationProperty { get; init; }

    /// <summary>
    /// Gets or sets the convention-matched source property, when one exists.
    /// </summary>
    /// <remarks>
    /// Null when no case-insensitive name match is found on the source type.
    /// </remarks>
    public PropertyInfo? SourceProperty { get; set; }

    /// <summary>
    /// Gets or sets the strategy used to resolve the destination member value.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="MemberMappingKind.Convention"/>.
    /// </remarks>
    public MemberMappingKind Kind { get; set; } = MemberMappingKind.Convention;

    /// <summary>
    /// Gets or sets the configured source member expression, when applicable.
    /// </summary>
    /// <remarks>
    /// Populated for <see cref="MemberMappingKind.Expression"/> mappings.
    /// </remarks>
    public LambdaExpression? SourceExpression { get; set; }

    /// <summary>
    /// Gets or sets the compiled delegate used to resolve the member value.
    /// </summary>
    /// <remarks>
    /// Signature depends on <see cref="Kind"/>.
    /// </remarks>
    public Delegate? CustomResolver { get; set; }

    /// <summary>
    /// Gets or sets the value resolver type for <see cref="MemberMappingKind.ValueResolver"/> mappings.
    /// </summary>
    public Type? ValueResolverType { get; set; }

    /// <summary>
    /// Gets or sets the type converter type for <see cref="MemberMappingKind.TypeConverter"/> mappings.
    /// </summary>
    public Type? TypeConverterType { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the destination member is ignored.
    /// </summary>
    /// <remarks>
    /// Ignored members are skipped during mapping and validation.
    /// </remarks>
    public bool IsIgnored { get; set; }
}

/// <summary>
/// Stores the complete mapping configuration for a source-destination type pair.
/// </summary>
/// <remarks>
/// Registered in <see cref="TypeMapRegistry"/> and used by <see cref="MappingEngine"/> at runtime.
/// </remarks>
internal sealed class TypeMapConfiguration
{
    /// <summary>
    /// Gets the configured source type for this mapping.
    /// </summary>
    public required Type SourceType { get; init; }

    /// <summary>
    /// Gets the configured destination type for this mapping.
    /// </summary>
    public required Type DestinationType { get; init; }

    /// <summary>
    /// Gets the configured property mappings for this type pair.
    /// </summary>
    /// <remarks>
    /// One entry exists per configured or convention-matched destination property.
    /// </remarks>
    public List<PropertyMapping> PropertyMappings { get; } = [];

    /// <summary>
    /// Gets callbacks invoked before mapping without resolution context.
    /// </summary>
    public List<Action<object, object>> BeforeMapActions { get; } = [];

    /// <summary>
    /// Gets callbacks invoked before mapping with resolution context.
    /// </summary>
    public List<Action<object, object, ResolutionContext>> BeforeMapContextActions { get; } = [];

    /// <summary>
    /// Gets callbacks invoked after mapping without resolution context.
    /// </summary>
    public List<Action<object, object>> AfterMapActions { get; } = [];

    /// <summary>
    /// Gets callbacks invoked after mapping with resolution context.
    /// </summary>
    public List<Action<object, object, ResolutionContext>> AfterMapContextActions { get; } = [];

    /// <summary>
    /// Gets or sets the reverse type map created by <see cref="Configuration.MappingExpression{TSource, TDestination}.ReverseMap"/>.
    /// </summary>
    /// <remarks>
    /// Null until a reverse mapping is configured.
    /// </remarks>
    public TypeMapConfiguration? ReverseTypeMap { get; set; }

    /// <summary>
    /// Gets an existing property mapping or creates a convention-based mapping for the destination property.
    /// </summary>
    /// <remarks>
    /// Matches existing mappings by destination property name case-insensitively.
    /// </remarks>
    /// <param name="destinationProperty">The destination property to map.</param>
    /// <returns>The existing or newly created property mapping.</returns>
    public PropertyMapping GetOrCreateMapping(PropertyInfo destinationProperty)
    {
        var existing = PropertyMappings.FirstOrDefault(m =>
            m.DestinationProperty.Name.Equals(
                destinationProperty.Name,
                StringComparison.OrdinalIgnoreCase
            )
        );

        if (existing is not null)
        {
            return existing;
        }

        var mapping = new PropertyMapping
        {
            DestinationProperty = destinationProperty,
            SourceProperty = FindSourceProperty(destinationProperty),
        };

        PropertyMappings.Add(mapping);
        return mapping;
    }

    /// <summary>
    /// Finds a readable source property matching the destination property name.
    /// </summary>
    /// <remarks>
    /// Uses case-insensitive name comparison on public instance properties.
    /// </remarks>
    /// <param name="destinationProperty">The destination property to match.</param>
    /// <returns>The matching source property, or null when no match exists.</returns>
    private PropertyInfo? FindSourceProperty(PropertyInfo destinationProperty)
    {
        return SourceType
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .FirstOrDefault(p =>
                p.CanRead
                && p.Name.Equals(destinationProperty.Name, StringComparison.OrdinalIgnoreCase)
            );
    }
}
