using LanceC.SpreadsheetIO.Mapping2.Options.Registrations;

namespace LanceC.SpreadsheetIO.Mapping2.Options.Converters;

/// <summary>
/// Provides the result from converting a map option.
/// </summary>
public abstract class MapOptionConversionResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MapOptionConversionResult"/> class.
    /// </summary>
    /// <param name="isValid">The value that determines whether the conversion is valid.</param>
    /// <param name="registration">The map option registration.</param>
    /// <param name="option">The map option or <c>null</c> the conversion is not valid.</param>
    /// <param name="message">The error message from a failed conversion.</param>
    protected MapOptionConversionResult(bool isValid, IMapOptionRegistration registration, IMapOption? option, string message)
    {
        IsValid = isValid;
        Registration = registration;
        Option = option;
        Message = message;
    }

    /// <summary>
    /// Gets the value that determines whether the conversion is valid.
    /// </summary>
    public bool IsValid { get; }

    /// <summary>
    /// Gets the map option registration.
    /// </summary>
    public IMapOptionRegistration Registration { get; }

    /// <summary>
    /// Gets the map option or <c>null</c> if the conversion is not valid.
    /// </summary>
    public IMapOption? Option { get; }

    /// <summary>
    /// Gets the error message from a failed conversion.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Creates the result for a successful conversion.
    /// </summary>
    /// <typeparam name="TOption">The type of map option.</typeparam>
    /// <param name="registration">The map option registration.</param>
    /// <param name="option">The map option.</param>
    /// <returns>The successful map option conversion result.</returns>
    public static MapOptionConversionResult<TOption> Success<TOption>(IMapOptionRegistration registration, TOption option)
        where TOption : IMapOption
        => new(true, registration, option, string.Empty);

    /// <summary>
    /// Creates the result for a skipped conversion.
    /// </summary>
    /// <typeparam name="TOption">The type of map option.</typeparam>
    /// <param name="registration">The map option registration.</param>
    /// <returns>The skipped map option conversion result.</returns>
    public static MapOptionConversionResult<TOption> Skipped<TOption>(IMapOptionRegistration registration)
        where TOption : IMapOption
        => new(true, registration, default, string.Empty);

    /// <summary>
    /// Creates the result for a failed conversion.
    /// </summary>
    /// <typeparam name="TOption">The type of map option.</typeparam>
    /// <param name="registration">The map option registration.</param>
    /// <param name="message">The error message.</param>
    /// <returns>The failed map option conversion result.</returns>
    public static MapOptionConversionResult<TOption> Failure<TOption>(IMapOptionRegistration registration, string message)
        where TOption : IMapOption
        => new(false, registration, default, message);
}
