using System;
using System.Collections.Generic;
using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Writing.Internal.Serializing
{
    internal class DoubleResourcePropertySerializerStrategy : IResourcePropertySerializerStrategy
    {
        public IReadOnlyCollection<Type> PropertyTypes { get; } =
            new[]
            {
                typeof(uint),
                typeof(long),
                typeof(ulong),
                typeof(float),
                typeof(double),
            };

        public WritingCellValue Serialize<TResource>(object? value, PropertyMap<TResource> map)
            where TResource : class
        {
            if (value is null)
            {
                return new WritingCellValue();
            }

            var doubleValue = Convert.ToDouble(value);
            var cellValue = new WritingCellValue(doubleValue);
            return cellValue;
        }
    }
}
