using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Options.Extensions;

namespace LanceC.SpreadsheetIO.Writing.Internal.Serializing;

internal class StringResourcePropertySerializerStrategy : IResourcePropertySerializerStrategy
{
    public IReadOnlyCollection<Type> PropertyTypes { get; } =
        new[]
        {
            typeof(string),
        };

    public WritingCellValue Serialize(object? value, PropertyMap map)
    {
        var stringValue = value?.ToString();
        var stringKind = map.Options.GetStringKind();

        var cellValue = new WritingCellValue(stringValue, stringKind);
        return cellValue;
    }
}
