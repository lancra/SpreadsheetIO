using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Reading.Internal.Parsing;

internal class BooleanResourcePropertyParserStrategy : IDefaultResourcePropertyParserStrategy
{
    public Type PropertyType { get; } = typeof(bool);

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

        var hasIntegerValue = int.TryParse(cellValue, out var integerValue);
        if (!hasIntegerValue)
        {
            value = null;
            return ResourcePropertyParseResultKind.Invalid;
        }

        if (integerValue != 0 && integerValue != 1)
        {
            value = null;
            return ResourcePropertyParseResultKind.Invalid;
        }

        value = integerValue == 1;
        return ResourcePropertyParseResultKind.Success;
    }
}
