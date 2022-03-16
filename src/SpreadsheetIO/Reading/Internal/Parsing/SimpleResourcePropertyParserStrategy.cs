using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Reading.Internal.Parsing;

internal abstract class SimpleResourcePropertyParserStrategy<TProperty> : IDefaultResourcePropertyParserStrategy
{
    public Type PropertyType { get; } = typeof(TProperty);

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

        var hasTypeValue = TryParseValue(cellValue, out var typeValue);
        if (!hasTypeValue)
        {
            value = null;
            return ResourcePropertyParseResultKind.Invalid;
        }

        value = typeValue;
        return ResourcePropertyParseResultKind.Success;
    }

    protected abstract bool TryParseValue(string cellValue, out TProperty value);
}
