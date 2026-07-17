using System.Collections;
using System.Reflection;

namespace DotRMapper.Internal;

/// <summary>
/// Creates object and collection instances during mapping operations.
/// </summary>
/// <remarks>
/// Requires a parameterless constructor for reference types. Collection creation supports arrays,
/// <see cref="List{T}"/>, and generic <see cref="IEnumerable{T}"/> interfaces.
/// </remarks>
internal static class ObjectFactory
{
    /// <summary>
    /// Creates a new instance of the specified type.
    /// </summary>
    /// <remarks>
    /// Value types are default-initialized. Reference types require a public parameterless constructor.
    /// </remarks>
    /// <param name="type">The type to instantiate.</param>
    /// <returns>A new instance of <paramref name="type"/>.</returns>
    public static object CreateInstance(Type type)
    {
        if (type.IsValueType)
        {
            return Activator.CreateInstance(type)!;
        }

        var constructor = type.GetConstructor(Type.EmptyTypes);
        if (constructor is null)
        {
            throw new InvalidOperationException(
                $"Type '{type.FullName}' does not have a parameterless constructor."
            );
        }

        return Activator.CreateInstance(type)!;
    }

    /// <summary>
    /// Determines whether the type represents a collection and returns its element type.
    /// </summary>
    /// <remarks>
    /// Treats arrays and generic <see cref="IEnumerable{T}"/> implementations as collections.
    /// <see cref="string"/> is excluded.
    /// </remarks>
    /// <param name="type">The type to inspect.</param>
    /// <param name="elementType">The collection element type when the type is a collection.</param>
    /// <returns><see langword="true"/> when <paramref name="type"/> is a collection type.</returns>
    public static bool IsCollectionType(Type type, out Type? elementType)
    {
        elementType = null;

        if (type.IsArray)
        {
            elementType = type.GetElementType();
            return true;
        }

        if (type == typeof(string))
        {
            return false;
        }

        if (type.IsGenericType)
        {
            var definition = type.GetGenericTypeDefinition();
            if (
                definition == typeof(IEnumerable<>)
                || definition == typeof(ICollection<>)
                || definition == typeof(IList<>)
                || definition == typeof(List<>)
            )
            {
                elementType = type.GetGenericArguments()[0];
                return true;
            }
        }

        foreach (var interfaceType in type.GetInterfaces())
        {
            if (
                interfaceType.IsGenericType
                && interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>)
            )
            {
                elementType = interfaceType.GetGenericArguments()[0];
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Creates an empty collection instance with the specified capacity hint.
    /// </summary>
    /// <remarks>
    /// Interface destination types default to <see cref="List{T}"/>. Throws when the destination
    /// type cannot be instantiated as a collection.
    /// </remarks>
    /// <param name="destinationType">The collection type to create.</param>
    /// <param name="elementType">The element type of the collection.</param>
    /// <param name="capacity">The initial capacity for array allocation.</param>
    /// <returns>A new collection instance.</returns>
    public static object CreateCollection(Type destinationType, Type elementType, int capacity)
    {
        if (destinationType.IsArray)
        {
            return Array.CreateInstance(elementType, capacity);
        }

        if (
            destinationType.IsGenericType
            && destinationType.GetGenericTypeDefinition() == typeof(List<>)
        )
        {
            return Activator.CreateInstance(destinationType)!;
        }

        if (destinationType.IsInterface)
        {
            return Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType))!;
        }

        if (destinationType.IsGenericType)
        {
            return Activator.CreateInstance(destinationType)!;
        }

        throw new InvalidOperationException(
            $"Cannot create collection instance for type '{destinationType.FullName}'."
        );
    }

    /// <summary>
    /// Adds an item to a collection instance.
    /// </summary>
    /// <remarks>
    /// Uses <see cref="IList.Add"/> when available; otherwise invokes a public <c>Add</c> method via reflection.
    /// </remarks>
    /// <param name="collection">The collection to modify.</param>
    /// <param name="item">The item to add.</param>
    public static void AddToCollection(object collection, object? item)
    {
        switch (collection)
        {
            case IList list:
                list.Add(item);
                break;
            default:
                var addMethod = collection.GetType().GetMethod("Add");
                addMethod?.Invoke(collection, [item]);
                break;
        }
    }
}
