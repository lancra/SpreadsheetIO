using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Reading.Internal.Parsing;

internal class StringResourcePropertyParserStrategy : IDefaultResourcePropertyParserStrategy
{
    public Type PropertyType { get; } = typeof(string);

    public ResourcePropertyParseResultKind TryParse(string cellValue, PropertyMap map, out object? value)
    {
        value = cellValue;
        return string.IsNullOrEmpty(cellValue) ? ResourcePropertyParseResultKind.Empty : ResourcePropertyParseResultKind.Success;
    }
}
