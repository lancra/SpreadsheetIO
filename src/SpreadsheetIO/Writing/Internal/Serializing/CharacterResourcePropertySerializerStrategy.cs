using LanceC.SpreadsheetIO.Mapping2;

namespace LanceC.SpreadsheetIO.Writing.Internal.Serializing;

internal class CharacterResourcePropertySerializerStrategy : IResourcePropertySerializerStrategy
{
    public IReadOnlyCollection<Type> PropertyTypes { get; } =
        new[]
        {
                typeof(char),
        };

    public WritingCellValue Serialize(object? value, PropertyMap map)
    {
        var characterValue = (char?)value;
        var cellValue = new WritingCellValue(characterValue);
        return cellValue;
    }
}
