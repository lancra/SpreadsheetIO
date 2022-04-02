using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Options.Extensions;

namespace LanceC.SpreadsheetIO.Writing.Internal.Serializing;

internal class DateTimeOffsetResourcePropertySerializerStrategy : IResourcePropertySerializerStrategy
{
    public IReadOnlyCollection<Type> PropertyTypes { get; } =
        new[]
        {
                typeof(DateTimeOffset),
        };

    public WritingCellValue Serialize(object? value, PropertyMap map)
    {
        var dateTimeOffsetValue = (DateTimeOffset?)value;
        var dateKind = map.Options.GetDateKind();

        var cellValue = new WritingCellValue(dateTimeOffsetValue, dateKind);
        return cellValue;
    }
}
