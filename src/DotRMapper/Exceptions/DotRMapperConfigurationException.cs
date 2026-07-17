namespace DotRMapper.Exceptions;

/// <summary>
/// Represents errors that occur during mapper configuration or validation.
/// </summary>
/// <remarks>
/// Thrown by <see cref="MapperConfiguration.AssertConfigurationIsValid"/> when mappings are incomplete.
/// </remarks>
public sealed class DotRMapperConfigurationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DotRMapperConfigurationException"/> class.
    /// </summary>
    /// <remarks>
    /// Use when validation fails without an underlying system exception.
    /// </remarks>
    /// <param name="message">The error message.</param>
    public DotRMapperConfigurationException(string message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DotRMapperConfigurationException"/> class.
    /// </summary>
    /// <remarks>
    /// Wraps an inner exception that caused the configuration failure.
    /// </remarks>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public DotRMapperConfigurationException(string message, Exception innerException)
        : base(message, innerException) { }
}
