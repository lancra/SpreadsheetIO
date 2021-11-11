using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Writing.Internal.Serializing;

internal class CharacterResourcePropertySerializerStrategy : IResourcePropertySerializerStrategy
{
    public IReadOnlyCollection<Type> PropertyTypes { get; } =
        new[]
        {
                typeof(char),
        };

    public WritingCellValue Serialize<TResource>(object? value, PropertyMap<TResource> map)
        where TResource : class
    {
        var characterValue = (char?)value;
        var cellValue = new WritingCellValue(characterValue);
        return cellValue;
    }
}
