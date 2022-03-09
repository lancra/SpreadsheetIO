using LanceC.SpreadsheetIO.Mapping2;

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
        var cellValue = new WritingCellValue(stringValue);
        return cellValue;
    }
}
