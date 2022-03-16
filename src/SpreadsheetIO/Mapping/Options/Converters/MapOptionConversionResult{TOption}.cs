using LanceC.SpreadsheetIO.Mapping.Options.Registrations;

namespace LanceC.SpreadsheetIO.Mapping.Options.Converters;

/// <summary>
/// Provides the result from converting a map option.
/// </summary>
/// <typeparam name="TOption">The type of map option.</typeparam>
public class MapOptionConversionResult<TOption> : MapOptionConversionResult
    where TOption : IMapOption
{
    internal MapOptionConversionResult(bool isValid, IMapOptionRegistration registration, TOption? option, string message)
        : base(isValid, registration, option, message)
    {
        Option = option;
    }

    /// <summary>
    /// Gets the map option or <c>null</c> if the conversion is not valid.
    /// </summary>
    public new TOption? Option { get; }
}
