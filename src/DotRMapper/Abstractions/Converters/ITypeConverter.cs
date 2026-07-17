namespace DotRMapper.Abstractions.Converters;

/// <summary>
/// Defines a type converter used during member mapping.
/// </summary>
/// <remarks>
/// Register via <see cref="Configuration.IMemberConfigurationExpression{TSource, TDestination, TMember}.ConvertUsing{TConverter}"/>.
/// The converter receives the convention-matched source property value when one exists.
/// </remarks>
public interface ITypeConverter
{
    /// <summary>
    /// Converts the source value to the destination type.
    /// </summary>
    /// <remarks>
    /// Return null when conversion is not possible. The destination member type is provided via
    /// <paramref name="destinationType"/>.
    /// </remarks>
    /// <param name="source">The source value.</param>
    /// <param name="destinationType">The destination type.</param>
    /// <param name="context">The resolution context.</param>
    /// <returns>The converted value.</returns>
    object? Convert(object? source, Type destinationType, ResolutionContext context);
}
