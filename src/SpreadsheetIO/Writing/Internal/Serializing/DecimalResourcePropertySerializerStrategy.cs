using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Writing.Internal.Serializing;

internal class DecimalResourcePropertySerializerStrategy : IResourcePropertySerializerStrategy
{
    public IReadOnlyCollection<Type> PropertyTypes { get; } =
        new[]
        {
            typeof(decimal),
        };

    public WritingCellValue Serialize(object? value, PropertyMap map)
    {
        var decimalValue = (decimal?)value;
        var cellValue = new WritingCellValue(decimalValue);
        return cellValue;
    }
}
