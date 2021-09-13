using System;
using System.Collections.Generic;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Extensions;

namespace LanceC.SpreadsheetIO.Writing.Internal.Serializing
{
    internal class DateTimeResourcePropertySerializerStrategy : IResourcePropertySerializerStrategy
    {
        public IReadOnlyCollection<Type> PropertyTypes { get; } =
            new[]
            {
                typeof(DateTime),
            };

        public WritingCellValue Serialize<TResource>(object? value, PropertyMap<TResource> map)
            where TResource : class
        {
            var dateTimeValue = (DateTime?)value;
            var dateKindExtension = map.Options.FindExtension<DateKindMapOptionsExtension>();

            if (dateKindExtension is not null)
            {
                var cellValue = new WritingCellValue(dateTimeValue, dateKindExtension.DateKind);
                return cellValue;
            }
            else
            {
                var cellValue = new WritingCellValue(dateTimeValue);
                return cellValue;
            }
        }
    }
}
