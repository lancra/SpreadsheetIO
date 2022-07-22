using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Options.Extensions;
using LanceC.SpreadsheetIO.Shared;
using LanceC.SpreadsheetIO.Shared.Internal;

namespace LanceC.SpreadsheetIO.Reading.Internal.Parsing;

internal class DateTimeOffsetResourcePropertyParserStrategy : IDefaultResourcePropertyParserStrategy
{
    public Type PropertyType { get; } = typeof(DateTimeOffset);

    public ResourcePropertyParseResultKind TryParse(string cellValue, PropertyMap map, out object? value)
    {
        var noValue = string.IsNullOrEmpty(cellValue);
        if (noValue)
        {
            value = null;
            return Nullable.GetUnderlyingType(map.Property.PropertyType) is not null
                ? ResourcePropertyParseResultKind.Empty
                : ResourcePropertyParseResultKind.Missing;
        }

        var dateKind = map.Options.GetDateKind();
        if (dateKind == CellDateKind.Number)
        {
            return TryParseFromNumber(cellValue, out value);
        }
        else
        {
            return TryParseFromText(cellValue, out value);
        }
    }

    private static ResourcePropertyParseResultKind TryParseFromNumber(string cellValue, out object? value)
    {
        var hasOADate = double.TryParse(cellValue, out var oaDate);
        if (!hasOADate)
        {
            value = null;
            return ResourcePropertyParseResultKind.Invalid;
        }

        try
        {
            var dateTimeValue = oaDate.FromExcelOADate();
            value = new DateTimeOffset(dateTimeValue);
            return ResourcePropertyParseResultKind.Success;
        }
        catch (ArgumentException)
        {
            value = null;
            return ResourcePropertyParseResultKind.Invalid;
        }
    }

    private static ResourcePropertyParseResultKind TryParseFromText(string cellValue, out object? value)
    {
        var hasDateTimeOffsetValue = DateTimeOffset.TryParse(cellValue, out var dateTimeOffsetValue);
        if (!hasDateTimeOffsetValue)
        {
            value = null;
            return ResourcePropertyParseResultKind.Invalid;
        }

        value = dateTimeOffsetValue;
        return ResourcePropertyParseResultKind.Success;
    }
}
