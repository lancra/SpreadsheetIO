using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Options;

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
        var dateKindOption = map.Options.Find<DateKindMapOption>();

        if (dateKindOption is not null)
        {
            var cellValue = new WritingCellValue(dateTimeOffsetValue, dateKindOption.DateKind);
            return cellValue;
        }
        else
        {
            var cellValue = new WritingCellValue(dateTimeOffsetValue);
            return cellValue;
        }
    }
}
