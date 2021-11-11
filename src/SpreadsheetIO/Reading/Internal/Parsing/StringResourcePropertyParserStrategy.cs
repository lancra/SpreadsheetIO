using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Reading.Internal.Parsing
{
    internal class StringResourcePropertyParserStrategy : IDefaultResourcePropertyParserStrategy
    {
        public Type PropertyType { get; } = typeof(string);

        public ResourcePropertyParseResultKind TryParse<TResource>(string cellValue, PropertyMap<TResource> map, out object? value)
            where TResource : class
        {
            value = cellValue;
            return string.IsNullOrEmpty(cellValue) ? ResourcePropertyParseResultKind.Empty : ResourcePropertyParseResultKind.Success;
        }
    }
}
