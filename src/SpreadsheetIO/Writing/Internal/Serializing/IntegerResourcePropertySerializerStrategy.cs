using System;
using System.Collections.Generic;
using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Writing.Internal.Serializing
{
    internal class IntegerResourcePropertySerializerStrategy : IResourcePropertySerializerStrategy
    {
        public IReadOnlyCollection<Type> PropertyTypes { get; } =
            new[]
            {
                typeof(sbyte),
                typeof(byte),
                typeof(short),
                typeof(ushort),
                typeof(int),
            };

        public WritingCellValue Serialize<TResource>(object? value, PropertyMap<TResource> map)
            where TResource : class
        {
            if (value is null)
            {
                return new WritingCellValue();
            }

            var integerValue = Convert.ToInt32(value);
            var cellValue = new WritingCellValue(integerValue);
            return cellValue;
        }
    }
}
