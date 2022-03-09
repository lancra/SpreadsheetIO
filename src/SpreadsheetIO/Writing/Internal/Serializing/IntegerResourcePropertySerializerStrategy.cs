using LanceC.SpreadsheetIO.Mapping2;

namespace LanceC.SpreadsheetIO.Writing.Internal.Serializing;

internal class IntegerResourcePropertySerializerStrategy : IResourcePropertySerializerStrategy
{
    public IReadOnlyCollection<Type> PropertyTypes { get; } =
        new[]
        {
                typeof(sbyte),
                typeof(byte),
                typeof(short),
                typeof(ushort),
                typeof(int),
        };

    public WritingCellValue Serialize(object? value, PropertyMap map)
    {
        if (value is null)
        {
            return new WritingCellValue();
        }

        var integerValue = Convert.ToInt32(value);
        var cellValue = new WritingCellValue(integerValue);
        return cellValue;
    }
}
