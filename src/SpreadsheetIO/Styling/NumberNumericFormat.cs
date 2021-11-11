using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Styling;

/// <summary>
/// Represents a numeric format for general display of numbers.
/// </summary>
[ExcludeFromCodeCoverage]
public record NumberNumericFormat(int DecimalPlaces, bool UseThousandsSeparator, NegativeNumericFormatKind NegativeFormatKind)
{
    /// <summary>
    /// Gets the default number numeric format.
    /// </summary>
    public static readonly NumberNumericFormat Default = new(2, false, NegativeNumericFormatKind.Default);

    /// <summary>
    /// Gets the number of decimal places.
    /// </summary>
    public int DecimalPlaces { get; init; } = DecimalPlaces;

    /// <summary>
    /// Gets the value that determines whether the thousands separator is shown.
    /// </summary>
    public bool UseThousandsSeparator { get; init; } = UseThousandsSeparator;

    /// <summary>
    /// Gets the kind of format to use for negative numbers.
    /// </summary>
    public NegativeNumericFormatKind NegativeFormatKind { get; init; } = NegativeFormatKind;
}
