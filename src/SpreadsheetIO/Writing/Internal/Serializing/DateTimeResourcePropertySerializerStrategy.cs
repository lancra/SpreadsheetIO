using LanceC.SpreadsheetIO.Mapping2;
using LanceC.SpreadsheetIO.Mapping2.Options;

namespace LanceC.SpreadsheetIO.Writing.Internal.Serializing;

internal class DateTimeResourcePropertySerializerStrategy : IResourcePropertySerializerStrategy
{
    public IReadOnlyCollection<Type> PropertyTypes { get; } =
        new[]
        {
                typeof(DateTime),
        };

    public WritingCellValue Serialize(object? value, PropertyMap map)
    {
        var dateTimeValue = (DateTime?)value;
        var dateKindOption = map.Options.Find<DateKindMapOption>();

        if (dateKindOption is not null)
        {
            var cellValue = new WritingCellValue(dateTimeValue, dateKindOption.DateKind);
            return cellValue;
        }
        else
        {
            var cellValue = new WritingCellValue(dateTimeValue);
            return cellValue;
        }
    }
}
