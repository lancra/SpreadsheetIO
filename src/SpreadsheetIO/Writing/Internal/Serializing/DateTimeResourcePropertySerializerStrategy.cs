using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Options.Extensions;

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
        var dateKind = map.Options.GetDateKind();

        var cellValue = new WritingCellValue(dateTimeValue, dateKind);
        return cellValue;
    }
}
