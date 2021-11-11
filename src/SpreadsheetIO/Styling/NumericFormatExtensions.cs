using System.Text;
using Ardalis.GuardClauses;
using LanceC.SpreadsheetIO.Shared.Internal;

namespace LanceC.SpreadsheetIO.Styling;

/// <summary>
/// Provides extension methods for various numeric formats.
/// </summary>
public static class NumericFormatExtensions
{
    /// <summary>
    /// Converts a number numeric format into a coded numeric format.
    /// </summary>
    /// <param name="numericFormat">The number numeric format to convert.</param>
    /// <returns>The equivalent coded numeric format.</returns>
    public static NumericFormat ToNumericFormat(this NumberNumericFormat numericFormat)
    {
        Guard.Against.Null(numericFormat, nameof(numericFormat));

        var formatCodeBuilder = new StringBuilder();

        var wholeNumberFormat = GetWholeNumberFormat(numericFormat.UseThousandsSeparator);
        var decimalPlacesFormat = GetDecimalPlacesFormat(numericFormat.DecimalPlaces);

        formatCodeBuilder.Append(wholeNumberFormat)
            .Append(decimalPlacesFormat);

        if (numericFormat.NegativeFormatKind != NegativeNumericFormatKind.Default)
        {
            formatCodeBuilder.AppendIf("_)", numericFormat.NegativeFormatKind.HasParentheses)
                .Append(';')
                .AppendIf("[Red]", numericFormat.NegativeFormatKind.HasColor)
                .AppendIf(@"\(", numericFormat.NegativeFormatKind.HasParentheses)
                .Append(wholeNumberFormat)
                .Append(decimalPlacesFormat)
                .AppendIf(@"\)", numericFormat.NegativeFormatKind.HasParentheses);
        }

        var formatCode = formatCodeBuilder.ToString();
        return new NumericFormat(formatCode);
    }

    /// <summary>
    /// Converts a currency numeric format into a coded numeric format.
    /// </summary>
    /// <param name="numericFormat">The currency numeric format to convert.</param>
    /// <returns>The equivalent coded numeric format.</returns>
    public static NumericFormat ToNumericFormat(this CurrencyNumericFormat numericFormat)
    {
        Guard.Against.Null(numericFormat, nameof(numericFormat));

        if (string.IsNullOrEmpty(numericFormat.CurrencyCode))
        {
            var numberNumericFormat = new NumberNumericFormat(numericFormat.DecimalPlaces, true, numericFormat.NegativeFormatKind);
            return numberNumericFormat.ToNumericFormat();
        }

        var formatCodeBuilder = new StringBuilder();

        var currencyFormat = GetCurrencyFormat(numericFormat.CurrencyCode);
        var wholeNumberFormat = GetWholeNumberFormat(true);
        var decimalPlacesFormat = GetDecimalPlacesFormat(numericFormat.DecimalPlaces);

        formatCodeBuilder.Append(currencyFormat)
            .Append(wholeNumberFormat)
            .Append(decimalPlacesFormat);

        if (numericFormat.NegativeFormatKind != NegativeNumericFormatKind.Default)
        {
            formatCodeBuilder.AppendIf("_)", numericFormat.NegativeFormatKind.HasParentheses)
                .Append(';')
                .AppendIf("[Red]", numericFormat.NegativeFormatKind.HasColor)
                .AppendIf(@"\(", numericFormat.NegativeFormatKind.HasParentheses)
                .Append(currencyFormat)
                .Append(wholeNumberFormat)
                .Append(decimalPlacesFormat)
                .AppendIf(@"\)", numericFormat.NegativeFormatKind.HasParentheses);
        }

        var formatCode = formatCodeBuilder.ToString();
        return new NumericFormat(formatCode);
    }

    /// <summary>
    /// Converts an accounting numeric format into a coded numeric format.
    /// </summary>
    /// <param name="numericFormat">The accounting numeric format to convert.</param>
    /// <returns>The equivalent coded numeric format.</returns>
    public static NumericFormat ToNumericFormat(this AccountingNumericFormat numericFormat)
    {
        Guard.Against.Null(numericFormat, nameof(numericFormat));

        var formatCodeBuilder = new StringBuilder();

        var currencyFormat = GetCurrencyFormat(numericFormat.CurrencyCode);
        var wholeNumberFormat = GetWholeNumberFormat(true);
        var decimalPlacesFormat = GetDecimalPlacesFormat(numericFormat.DecimalPlaces);
        var decimalPlacesForZeroFormat = numericFormat.DecimalPlaces > 0
            ? new string('?', numericFormat.DecimalPlaces)
            : string.Empty;

        formatCodeBuilder.Append("_(")
            .Append(currencyFormat)
            .Append("* ")
            .Append(wholeNumberFormat)
            .Append(decimalPlacesFormat)
            .Append("_);_(")
            .Append(currencyFormat)
            .Append(@"* \(")
            .Append(wholeNumberFormat)
            .Append(decimalPlacesFormat)
            .Append(@"\);_(")
            .Append(currencyFormat)
            .Append("* \"-\"")
            .Append(decimalPlacesForZeroFormat)
            .Append("_);_(@_)");

        var formatCode = formatCodeBuilder.ToString();
        return new NumericFormat(formatCode);
    }

    /// <summary>
    /// Converts a text numeric format into a coded numeric format.
    /// </summary>
    /// <param name="numericFormat">The text numeric format to convert.</param>
    /// <returns>The equivalent coded numeric format.</returns>
    public static NumericFormat ToNumericFormat(this TextNumericFormat numericFormat)
    {
        Guard.Against.Null(numericFormat, nameof(numericFormat));

        return new NumericFormat("@");
    }

    private static string GetCurrencyFormat(string currencyCode)
    {
        if (string.IsNullOrEmpty(currencyCode))
        {
            return string.Empty;
        }

        if (currencyCode == "$")
        {
            return $"\"$\"";
        }

        return $@"[${currencyCode}]\ ";
    }

    private static string GetDecimalPlacesFormat(int decimalPlaces)
        => decimalPlaces > 0 ? "." + new string('0', decimalPlaces) : string.Empty;

    private static string GetWholeNumberFormat(bool useThousandsSeparator)
        => useThousandsSeparator ? "#,##0" : "0";
}
