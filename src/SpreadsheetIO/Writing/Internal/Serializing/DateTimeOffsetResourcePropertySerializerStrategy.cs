using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Extensions;

namespace LanceC.SpreadsheetIO.Writing.Internal.Serializing
{
    internal class DateTimeOffsetResourcePropertySerializerStrategy : IResourcePropertySerializerStrategy
    {
        public IReadOnlyCollection<Type> PropertyTypes { get; } =
            new[]
            {
                typeof(DateTimeOffset),
            };

        public WritingCellValue Serialize<TResource>(object? value, PropertyMap<TResource> map)
            where TResource : class
        {
            var dateTimeOffsetValue = (DateTimeOffset?)value;
            var dateKindExtension = map.Options.FindExtension<DateKindMapOptionsExtension>();

            if (dateKindExtension is not null)
            {
                var cellValue = new WritingCellValue(dateTimeOffsetValue, dateKindExtension.DateKind);
                return cellValue;
            }
            else
            {
                var cellValue = new WritingCellValue(dateTimeOffsetValue);
                return cellValue;
            }
        }
    }
}
