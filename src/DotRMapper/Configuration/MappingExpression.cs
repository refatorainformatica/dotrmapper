using System.Linq.Expressions;
using System.Reflection;
using DotRMapper.Abstractions;
using DotRMapper.Abstractions.Configuration;
using DotRMapper.Internal;

namespace DotRMapper.Configuration;

/// <summary>
/// Default implementation of <see cref="IMappingExpression{TSource, TDestination}"/>.
/// </summary>
/// <remarks>
/// Applies convention mappings for writable destination properties on construction.
/// Mutates the underlying <see cref="TypeMapConfiguration"/> for the configured pair.
/// </remarks>
/// <typeparam name="TSource">The source type.</typeparam>
/// <typeparam name="TDestination">The destination type.</typeparam>
internal sealed class MappingExpression<TSource, TDestination>
    : IMappingExpression<TSource, TDestination>
{
    /// <summary>
    /// The registry used to create or retrieve related type maps.
    /// </summary>
    /// <remarks>
    /// Shared across mapping expressions created from the same configuration.
    /// </remarks>
    private readonly TypeMapRegistry _registry;

    /// <summary>
    /// The type map configuration being built for this source-destination pair.
    /// </summary>
    /// <remarks>
    /// Stores member mappings and BeforeMap/AfterMap callbacks.
    /// </remarks>
    private readonly TypeMapConfiguration _typeMap;

    /// <summary>
    /// Initializes a new instance of the <see cref="MappingExpression{TSource, TDestination}"/> class.
    /// </summary>
    /// <remarks>
    /// Ensures convention mappings exist for all writable destination properties.
    /// </remarks>
    /// <param name="registry">The type map registry.</param>
    /// <param name="typeMap">The type map configuration to mutate.</param>
    public MappingExpression(TypeMapRegistry registry, TypeMapConfiguration typeMap)
    {
        _registry = registry;
        _typeMap = typeMap;
        EnsureConventionMappings();
    }

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
    public IMappingExpression<TSource, TDestination> ForMember<TMember>(
        Expression<Func<TDestination, TMember>> destinationMember,
        Action<IMemberConfigurationExpression<TSource, TDestination, TMember>> memberOptions
    )
    {
        var property = ExpressionHelper.GetPropertyInfo(destinationMember);
        var mapping = _typeMap.GetOrCreateMapping(property);
        var memberExpression = new MemberConfigurationExpression<TSource, TDestination, TMember>(
            mapping
        );
        memberOptions(memberExpression);
        return this;
    }

    /// <summary>
    /// Configures a callback invoked before the mapping is executed.
    /// </summary>
    /// <remarks>
    /// Runs after the destination instance is created but before members are assigned.
    /// </remarks>
    /// <param name="beforeFunction">The callback to invoke.</param>
    /// <returns>The same mapping expression for chaining.</returns>
    public IMappingExpression<TSource, TDestination> BeforeMap(
        Action<TSource, TDestination> beforeFunction
    )
    {
        _typeMap.BeforeMapActions.Add(
            (source, destination) => beforeFunction((TSource)source, (TDestination)destination)
        );
        return this;
    }

    /// <summary>
    /// Configures a callback invoked before the mapping is executed with access to the resolution context.
    /// </summary>
    /// <remarks>
    /// Runs after the destination instance is created but before members are assigned.
    /// </remarks>
    /// <param name="beforeFunction">The callback to invoke.</param>
    /// <returns>The same mapping expression for chaining.</returns>
    public IMappingExpression<TSource, TDestination> BeforeMap(
        Action<TSource, TDestination, ResolutionContext> beforeFunction
    )
    {
        _typeMap.BeforeMapContextActions.Add(
            (source, destination, context) =>
                beforeFunction((TSource)source, (TDestination)destination, context)
        );
        return this;
    }

    /// <summary>
    /// Configures a callback invoked after the mapping is executed.
    /// </summary>
    /// <remarks>
    /// Runs after all member assignments and nested mappings complete.
    /// </remarks>
    /// <param name="afterFunction">The callback to invoke.</param>
    /// <returns>The same mapping expression for chaining.</returns>
    public IMappingExpression<TSource, TDestination> AfterMap(
        Action<TSource, TDestination> afterFunction
    )
    {
        _typeMap.AfterMapActions.Add(
            (source, destination) => afterFunction((TSource)source, (TDestination)destination)
        );
        return this;
    }

    /// <summary>
    /// Configures a callback invoked after the mapping is executed with access to the resolution context.
    /// </summary>
    /// <remarks>
    /// Runs after all member assignments and nested mappings complete.
    /// </remarks>
    /// <param name="afterFunction">The callback to invoke.</param>
    /// <returns>The same mapping expression for chaining.</returns>
    public IMappingExpression<TSource, TDestination> AfterMap(
        Action<TSource, TDestination, ResolutionContext> afterFunction
    )
    {
        _typeMap.AfterMapContextActions.Add(
            (source, destination, context) =>
                afterFunction((TSource)source, (TDestination)destination, context)
        );
        return this;
    }

    /// <summary>
    /// Creates a reverse mapping from <typeparamref name="TDestination"/> to <typeparamref name="TSource"/>.
    /// </summary>
    /// <remarks>
    /// Copies non-ignored member mappings by matching property names case-insensitively.
    /// </remarks>
    /// <returns>The reverse mapping expression.</returns>
    public IMappingExpression<TDestination, TSource> ReverseMap()
    {
        var reverseTypeMap = _registry.GetOrAdd(typeof(TDestination), typeof(TSource));
        _typeMap.ReverseTypeMap = reverseTypeMap;

        foreach (var propertyMapping in _typeMap.PropertyMappings.Where(p => !p.IsIgnored))
        {
            if (propertyMapping.SourceProperty is null)
            {
                continue;
            }

            var reverseDestination = typeof(TSource).GetProperty(
                propertyMapping.SourceProperty.Name,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase
            );

            if (reverseDestination is null || !reverseDestination.CanWrite)
            {
                continue;
            }

            var reverseMapping = reverseTypeMap.GetOrCreateMapping(reverseDestination);
            reverseMapping.SourceProperty = propertyMapping.DestinationProperty;
            reverseMapping.Kind = MemberMappingKind.Convention;
            reverseMapping.IsIgnored = false;
        }

        return new MappingExpression<TDestination, TSource>(_registry, reverseTypeMap);
    }

    /// <summary>
    /// Ensures convention mappings exist for all writable public destination properties.
    /// </summary>
    /// <remarks>
    /// Called once during expression construction before custom configuration is applied.
    /// </remarks>
    private void EnsureConventionMappings()
    {
        var destinationProperties = typeof(TDestination)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.CanWrite && p.GetSetMethod() is not null);

        foreach (var property in destinationProperties)
        {
            _typeMap.GetOrCreateMapping(property);
        }
    }
}
