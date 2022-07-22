using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Shared.Internal;

internal static class ExcelOADateExtensions
{
    /// <summary>
    /// Represents the invalid date 1900-01-00 that is allowed within Excel.
    /// </summary>
    private const double MaximumExcelOADateForDoubleOffset = 0D;

    /// <summary>
    /// Represents the invalid date 1900-02-29 that is allowed within Excel.
    /// </summary>
    private const double MaximumExcelOADateForSingleOffset = 60D;

    private const double MaximumOADateForDoubleOffset = MaximumExcelOADateForDoubleOffset + 2D;
    private const double MaximumOADateForSingleOffset = MaximumExcelOADateForSingleOffset + 1D;

    public static DateTime FromExcelOADate(this double value)
    {
        ThrowIfInvalidOADate(value);

        var offset = GetOffset(value, MaximumExcelOADateForDoubleOffset, MaximumExcelOADateForSingleOffset);
        return DateTime.FromOADate(value + offset);
    }

    public static double ToExcelOADate(this DateTime value)
    {
        var oaDate = value.ToOADate();
        var offset = GetOffset(oaDate, MaximumOADateForDoubleOffset, MaximumOADateForSingleOffset);
        return oaDate - offset;
    }

    private static double GetOffset(double value, double maximumValueForDoubleOffset, double maximumValueForSingleOffset)
    {
        if (value < maximumValueForDoubleOffset)
        {
            return 2D;
        }
        else if (value < maximumValueForSingleOffset)
        {
            return 1D;
        }
        else
        {
            return 0D;
        }
    }

    private static void ThrowIfInvalidOADate(double value)
    {
        var truncatedValue = Math.Truncate(value);
        if (truncatedValue == MaximumExcelOADateForDoubleOffset || truncatedValue == MaximumExcelOADateForSingleOffset)
        {
            throw new ArgumentException(Messages.InvalidExcelDate, nameof(value));
        }
    }
}
