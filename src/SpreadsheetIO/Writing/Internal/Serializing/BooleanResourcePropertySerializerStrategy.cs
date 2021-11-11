using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Writing.Internal.Serializing
{
    internal class BooleanResourcePropertySerializerStrategy : IResourcePropertySerializerStrategy
    {
        public IReadOnlyCollection<Type> PropertyTypes { get; } =
            new[]
            {
                typeof(bool),
            };

        public WritingCellValue Serialize<TResource>(object? value, PropertyMap<TResource> map)
            where TResource : class
        {
            var booleanValue = (bool?)value;
            var cellValue = new WritingCellValue(booleanValue);
            return cellValue;
        }
    }
}
