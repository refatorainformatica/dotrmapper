using System.Collections;
using System.Reflection;
using DotRMapper.Abstractions;
using DotRMapper.Abstractions.Converters;
using DotRMapper.Abstractions.Resolvers;

namespace DotRMapper.Internal;

/// <summary>
/// Executes object-to-object mapping using configured and convention-based type maps.
/// </summary>
/// <remarks>
/// Handles nested objects, collections, enums, and type conversion. Invoked by <see cref="Mapper"/>.
/// </remarks>
internal sealed class MappingEngine
{
    /// <summary>
    /// The registry of configured type maps.
    /// </summary>
    /// <remarks>
    /// Used to resolve explicit mappings and to register convention maps on demand.
    /// </remarks>
    private readonly TypeMapRegistry _registry;

    /// <summary>
    /// The mapper instance associated with the current engine.
    /// </summary>
    /// <remarks>
    /// Passed to resolvers, converters, and nested mapping calls.
    /// </remarks>
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="MappingEngine"/> class.
    /// </summary>
    /// <remarks>
    /// Each <see cref="Mapper"/> owns one engine instance.
    /// </remarks>
    /// <param name="registry">The type map registry.</param>
    /// <param name="mapper">The mapper performing operations.</param>
    public MappingEngine(TypeMapRegistry registry, IMapper mapper)
    {
        _registry = registry;
        _mapper = mapper;
    }

    /// <summary>
    /// Maps a source object to a new instance of the destination type.
    /// </summary>
    /// <remarks>
    /// Returns the source unchanged when it is directly assignable. Builds a convention type map when
    /// no explicit configuration exists.
    /// </remarks>
    /// <param name="source">The source object.</param>
    /// <param name="destinationType">The destination type.</param>
    /// <returns>The mapped destination instance.</returns>
    public object Map(object source, Type destinationType)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destinationType);

        var sourceType = source.GetType();
        var typeMap = _registry.Find(sourceType, destinationType);

        if (typeMap is null && CanAssignDirectly(sourceType, destinationType))
        {
            return source;
        }

        if (
            ObjectFactory.IsCollectionType(sourceType, out var sourceElementType)
            && ObjectFactory.IsCollectionType(destinationType, out var destinationElementType)
        )
        {
            return MapCollection(
                source,
                sourceType,
                destinationType,
                sourceElementType!,
                destinationElementType!
            );
        }

        typeMap ??= BuildConventionTypeMap(sourceType, destinationType);

        var destination = ObjectFactory.CreateInstance(destinationType);
        MapInternal(source, destination, typeMap);
        return destination;
    }

    /// <summary>
    /// Maps a source object onto an existing destination instance.
    /// </summary>
    /// <remarks>
    /// Uses an explicit or convention-built type map for the runtime source and destination types.
    /// </remarks>
    /// <param name="source">The source object.</param>
    /// <param name="destination">The destination object to populate.</param>
    public void MapOnto(object source, object destination)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        var typeMap =
            _registry.Find(source.GetType(), destination.GetType())
            ?? BuildConventionTypeMap(source.GetType(), destination.GetType());

        MapInternal(source, destination, typeMap);
    }

    /// <summary>
    /// Executes the mapping pipeline for a source-destination pair using the given type map.
    /// </summary>
    /// <remarks>
    /// Runs BeforeMap callbacks, resolves each member, then runs AfterMap callbacks.
    /// </remarks>
    /// <param name="source">The source object.</param>
    /// <param name="destination">The destination object.</param>
    /// <param name="typeMap">The type map configuration to apply.</param>
    private void MapInternal(object source, object destination, TypeMapConfiguration typeMap)
    {
        var context = new ResolutionContext(_mapper, source, destination);

        foreach (var action in typeMap.BeforeMapActions)
        {
            action(source, destination);
        }

        foreach (var action in typeMap.BeforeMapContextActions)
        {
            action(source, destination, context);
        }

        foreach (var propertyMapping in typeMap.PropertyMappings)
        {
            if (propertyMapping.IsIgnored)
            {
                continue;
            }

            var value = ResolveMemberValue(source, destination, propertyMapping, context);
            SetMemberValue(destination, propertyMapping.DestinationProperty, value);
        }

        foreach (var action in typeMap.AfterMapActions)
        {
            action(source, destination);
        }

        foreach (var action in typeMap.AfterMapContextActions)
        {
            action(source, destination, context);
        }
    }

    /// <summary>
    /// Resolves the value for a single destination member according to its mapping kind.
    /// </summary>
    /// <remarks>
    /// Applies type conversion after raw value resolution when the value is non-null.
    /// </remarks>
    /// <param name="source">The source object.</param>
    /// <param name="destination">The destination object.</param>
    /// <param name="mapping">The property mapping configuration.</param>
    /// <param name="context">The resolution context.</param>
    /// <returns>The converted member value, or null.</returns>
    private object? ResolveMemberValue(
        object source,
        object destination,
        PropertyMapping mapping,
        ResolutionContext context
    )
    {
        var destinationType = mapping.DestinationProperty.PropertyType;
        object? rawValue = mapping.Kind switch
        {
            MemberMappingKind.Ignored => null,
            MemberMappingKind.Expression when mapping.SourceExpression is not null =>
                mapping.CustomResolver!.DynamicInvoke(source),
            MemberMappingKind.Func => mapping.CustomResolver!.DynamicInvoke(source),
            MemberMappingKind.FuncWithDestination => mapping.CustomResolver!.DynamicInvoke(
                source,
                destination
            ),
            MemberMappingKind.FuncWithContext => mapping.CustomResolver!.DynamicInvoke(
                source,
                destination,
                context
            ),
            MemberMappingKind.ValueResolver when mapping.ValueResolverType is not null =>
                ResolveWithValueResolver(source, destination, mapping),
            MemberMappingKind.TypeConverter when mapping.TypeConverterType is not null =>
                ResolveWithTypeConverter(source, mapping, context),
            _ => ResolveConventionValue(source, mapping),
        };

        if (rawValue is null)
        {
            return null;
        }

        return ConvertValue(rawValue, destinationType);
    }

    /// <summary>
    /// Reads the convention-matched source property value.
    /// </summary>
    /// <remarks>
    /// Returns null when no source property was matched during configuration.
    /// </remarks>
    /// <param name="source">The source object.</param>
    /// <param name="mapping">The property mapping configuration.</param>
    /// <returns>The source property value, or null.</returns>
    private static object? ResolveConventionValue(object source, PropertyMapping mapping)
    {
        if (mapping.SourceProperty is null)
        {
            return null;
        }

        return mapping.SourceProperty.GetValue(source);
    }

    /// <summary>
    /// Resolves a member value using a configured value resolver type.
    /// </summary>
    /// <remarks>
    /// Activates the resolver and invokes <see cref="IValueResolver{TSource, TDestination, TDestMember}.Resolve"/>.
    /// </remarks>
    /// <param name="source">The source object.</param>
    /// <param name="destination">The destination object.</param>
    /// <param name="mapping">The property mapping configuration.</param>
    /// <returns>The resolved member value.</returns>
    private object? ResolveWithValueResolver(
        object source,
        object destination,
        PropertyMapping mapping
    )
    {
        var resolver = Activator.CreateInstance(mapping.ValueResolverType!)!;
        var destMember = mapping.DestinationProperty.GetValue(destination);
        var method = mapping.ValueResolverType!.GetMethod(
            nameof(IValueResolver<object, object, object>.Resolve)
        )!;

        return method.Invoke(
            resolver,
            [source, destination, destMember, new ResolutionContext(_mapper, source, destination)]
        );
    }

    /// <summary>
    /// Resolves a member value using a configured type converter.
    /// </summary>
    /// <remarks>
    /// Passes the convention-matched source property value to the converter when available.
    /// </remarks>
    /// <param name="source">The source object.</param>
    /// <param name="mapping">The property mapping configuration.</param>
    /// <param name="context">The resolution context.</param>
    /// <returns>The converted member value.</returns>
    private static object? ResolveWithTypeConverter(
        object source,
        PropertyMapping mapping,
        ResolutionContext context
    )
    {
        var converter = (ITypeConverter)Activator.CreateInstance(mapping.TypeConverterType!)!;
        var sourceValue = mapping.SourceProperty?.GetValue(source);
        return converter.Convert(sourceValue, mapping.DestinationProperty.PropertyType, context);
    }

    /// <summary>
    /// Converts a resolved value to the destination member type.
    /// </summary>
    /// <remarks>
    /// Handles enums, nullable types, collections, nested mapping, and primitive type conversion.
    /// </remarks>
    /// <param name="value">The value to convert.</param>
    /// <param name="destinationType">The target member type.</param>
    /// <returns>The converted value.</returns>
    private object? ConvertValue(object value, Type destinationType)
    {
        var valueType = value.GetType();

        if (destinationType.IsInstanceOfType(value))
        {
            return value;
        }

        if (valueType.IsEnum && destinationType.IsEnum)
        {
            var name = Enum.GetName(valueType, value);
            return name is null ? null : Enum.Parse(destinationType, name, ignoreCase: true);
        }

        if (destinationType.IsEnum && value is string enumString)
        {
            return Enum.Parse(destinationType, enumString, ignoreCase: true);
        }

        if (value.GetType().IsEnum && destinationType == typeof(string))
        {
            return value.ToString();
        }

        if (IsNullableType(destinationType, out var underlyingType))
        {
            return ConvertValue(value, underlyingType!);
        }

        if (
            ObjectFactory.IsCollectionType(valueType, out var sourceElementType)
            && ObjectFactory.IsCollectionType(destinationType, out var destinationElementType)
        )
        {
            return MapCollection(
                value,
                valueType,
                destinationType,
                sourceElementType!,
                destinationElementType!
            );
        }

        if (!IsSimpleType(valueType) || !IsSimpleType(destinationType))
        {
            if (
                _registry.Find(valueType, destinationType) is not null
                || HasMatchingProperties(valueType, destinationType)
            )
            {
                return Map(value, destinationType);
            }
        }

        if (destinationType.IsAssignableFrom(valueType))
        {
            return value;
        }

        try
        {
            return Convert.ChangeType(value, destinationType);
        }
        catch (InvalidCastException)
        {
            return Map(value, destinationType);
        }
    }

    /// <summary>
    /// Maps a source collection to a new destination collection instance.
    /// </summary>
    /// <remarks>
    /// Maps each element individually. Arrays are populated by index; other collections use <see cref="ObjectFactory.AddToCollection"/>.
    /// </remarks>
    /// <param name="source">The source collection.</param>
    /// <param name="sourceType">The runtime source collection type.</param>
    /// <param name="destinationType">The destination collection type.</param>
    /// <param name="sourceElementType">The source element type.</param>
    /// <param name="destinationElementType">The destination element type.</param>
    /// <returns>The mapped destination collection.</returns>
    private object MapCollection(
        object source,
        Type sourceType,
        Type destinationType,
        Type sourceElementType,
        Type destinationElementType
    )
    {
        var sourceItems = ((IEnumerable)source).Cast<object>().ToList();
        var collection = ObjectFactory.CreateCollection(
            destinationType,
            destinationElementType,
            sourceItems.Count
        );

        if (collection is Array array)
        {
            for (var i = 0; i < sourceItems.Count; i++)
            {
                var mappedItem = MapElement(
                    sourceItems[i],
                    sourceElementType,
                    destinationElementType
                );
                array.SetValue(mappedItem, i);
            }

            return array;
        }

        foreach (var item in sourceItems)
        {
            var mappedItem = MapElement(item, sourceElementType, destinationElementType);
            ObjectFactory.AddToCollection(collection, mappedItem);
        }

        return collection;
    }

    /// <summary>
    /// Maps a single collection element to the destination element type.
    /// </summary>
    /// <remarks>
    /// Returns null for null items. Returns the item unchanged when already assignable.
    /// </remarks>
    /// <param name="item">The source element.</param>
    /// <param name="sourceElementType">The source element type.</param>
    /// <param name="destinationElementType">The destination element type.</param>
    /// <returns>The mapped element, or null.</returns>
    private object? MapElement(object? item, Type sourceElementType, Type destinationElementType)
    {
        if (item is null)
        {
            return null;
        }

        if (destinationElementType.IsInstanceOfType(item))
        {
            return item;
        }

        return Map(item, destinationElementType);
    }

    /// <summary>
    /// Assigns a value to a writable destination property.
    /// </summary>
    /// <remarks>
    /// No-op when the property cannot be written.
    /// </remarks>
    /// <param name="destination">The destination object.</param>
    /// <param name="property">The destination property.</param>
    /// <param name="value">The value to assign.</param>
    private static void SetMemberValue(object destination, PropertyInfo property, object? value)
    {
        if (!property.CanWrite)
        {
            return;
        }

        property.SetValue(destination, value);
    }

    /// <summary>
    /// Builds and registers a convention-based type map for the specified types.
    /// </summary>
    /// <remarks>
    /// Creates property mappings for all writable public destination properties.
    /// </remarks>
    /// <param name="sourceType">The source type.</param>
    /// <param name="destinationType">The destination type.</param>
    /// <returns>The created type map configuration.</returns>
    private TypeMapConfiguration BuildConventionTypeMap(Type sourceType, Type destinationType)
    {
        var typeMap = _registry.GetOrAdd(sourceType, destinationType);

        var destinationProperties = destinationType
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.CanWrite && p.GetSetMethod() is not null);

        foreach (var destinationProperty in destinationProperties)
        {
            typeMap.GetOrCreateMapping(destinationProperty);
        }

        return typeMap;
    }

    /// <summary>
    /// Determines whether the source type can be assigned directly to the destination type.
    /// </summary>
    /// <remarks>
    /// Used to skip mapping when no transformation is required.
    /// </remarks>
    /// <param name="sourceType">The source type.</param>
    /// <param name="destinationType">The destination type.</param>
    /// <returns><see langword="true"/> when direct assignment is valid.</returns>
    private static bool CanAssignDirectly(Type sourceType, Type destinationType) =>
        destinationType.IsAssignableFrom(sourceType);

    /// <summary>
    /// Determines whether the type is a simple scalar type supported by direct conversion.
    /// </summary>
    /// <remarks>
    /// Includes primitives, common value types, and strings.
    /// </remarks>
    /// <param name="type">The type to inspect.</param>
    /// <returns><see langword="true"/> when the type is considered simple.</returns>
    private static bool IsSimpleType(Type type)
    {
        type = Nullable.GetUnderlyingType(type) ?? type;

        return type.IsPrimitive
            || type == typeof(string)
            || type == typeof(decimal)
            || type == typeof(DateTime)
            || type == typeof(DateTimeOffset)
            || type == typeof(TimeSpan)
            || type == typeof(Guid);
    }

    /// <summary>
    /// Determines whether the type is nullable and returns its underlying type.
    /// </summary>
    /// <remarks>
    /// Only reference types and <see cref="Nullable{T}"/> value types are considered nullable here.
    /// </remarks>
    /// <param name="type">The type to inspect.</param>
    /// <param name="underlyingType">The underlying non-nullable type when applicable.</param>
    /// <returns><see langword="true"/> when <paramref name="type"/> is nullable.</returns>
    private static bool IsNullableType(Type type, out Type? underlyingType)
    {
        underlyingType = Nullable.GetUnderlyingType(type);
        return underlyingType is not null;
    }

    /// <summary>
    /// Determines whether the source and destination types share at least one matching property name.
    /// </summary>
    /// <remarks>
    /// Used to decide whether nested object mapping should be attempted.
    /// </remarks>
    /// <param name="sourceType">The source type.</param>
    /// <param name="destinationType">The destination type.</param>
    /// <returns><see langword="true"/> when a name match exists.</returns>
    private static bool HasMatchingProperties(Type sourceType, Type destinationType)
    {
        var sourceNames = sourceType
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.CanRead)
            .Select(p => p.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        return destinationType
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Any(p => p.CanWrite && sourceNames.Contains(p.Name));
    }
}
