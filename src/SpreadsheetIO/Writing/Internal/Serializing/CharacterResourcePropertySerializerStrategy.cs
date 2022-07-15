using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Options.Extensions;

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
        var stringKind = map.Options.GetStringKind();

        var cellValue = new WritingCellValue(characterValue, stringKind);
        return cellValue;
    }
}
