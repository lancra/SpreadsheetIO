using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Styling;

/// <summary>
/// Represents a numeric format for general monetary values.
/// </summary>
[ExcludeFromCodeCoverage]
public record CurrencyNumericFormat(int DecimalPlaces, string CurrencyCode, NegativeNumericFormatKind NegativeFormatKind)
{
    /// <summary>
    /// Gets the default currency numeric format.
    /// </summary>
    public static readonly CurrencyNumericFormat Default = new(2, "$", NegativeNumericFormatKind.Default);

    /// <summary>
    /// Gets the number of decimal places.
    /// </summary>
    public int DecimalPlaces { get; init; } = DecimalPlaces;

    /// <summary>
    /// Gets the currency code for the monetary value.
    /// </summary>
    public string CurrencyCode { get; init; } = CurrencyCode;

    /// <summary>
    /// Gets the kind of format to use for negative numbers.
    /// </summary>
    public NegativeNumericFormatKind NegativeFormatKind { get; init; } = NegativeFormatKind;
}
