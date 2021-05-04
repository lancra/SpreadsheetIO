using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Styling
{
    /// <summary>
    /// Represents a numeric format for monetary values with vertical alignments of the currency code and the decimal points.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record AccountingNumericFormat(int DecimalPlaces, string CurrencyCode)
    {
        /// <summary>
        /// Gets the default accounting numeric format.
        /// </summary>
        public static readonly AccountingNumericFormat Default = new(2, "$");

        /// <summary>
        /// Gets the number of decimal places.
        /// </summary>
        public int DecimalPlaces { get; init; } = DecimalPlaces;

        /// <summary>
        /// Gets the currency code for the monetary value.
        /// </summary>
        public string CurrencyCode { get; init; } = CurrencyCode;
    }
}
