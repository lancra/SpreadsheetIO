using LanceC.SpreadsheetIO.Mapping2;

namespace LanceC.SpreadsheetIO.Writing.Internal.Serializing;

internal class BooleanResourcePropertySerializerStrategy : IResourcePropertySerializerStrategy
{
    public IReadOnlyCollection<Type> PropertyTypes { get; } =
        new[]
        {
                typeof(bool),
        };

    public WritingCellValue Serialize(object? value, PropertyMap map)
    {
        var booleanValue = (bool?)value;
        var cellValue = new WritingCellValue(booleanValue);
        return cellValue;
    }
}
