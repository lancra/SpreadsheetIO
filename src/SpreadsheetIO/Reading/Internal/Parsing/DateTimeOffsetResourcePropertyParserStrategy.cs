using System;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Extensions;
using LanceC.SpreadsheetIO.Shared;

namespace LanceC.SpreadsheetIO.Reading.Internal.Parsing
{
    internal class DateTimeOffsetResourcePropertyParserStrategy : IDefaultResourcePropertyParserStrategy
    {
        public Type PropertyType { get; } = typeof(DateTimeOffset);

        public ResourcePropertyParseResultKind TryParse<TResource>(string cellValue, PropertyMap<TResource> map, out object? value)
            where TResource : class
        {
            var noValue = string.IsNullOrEmpty(cellValue);
            if (noValue)
            {
                value = null;
                return Nullable.GetUnderlyingType(map.Property.PropertyType) is not null
                    ? ResourcePropertyParseResultKind.Empty
                    : ResourcePropertyParseResultKind.Missing;
            }

            var extension = map.Options.FindExtension<DateKindMapOptionsExtension>();
            if (extension is null || extension.DateKind == CellDateKind.Number)
            {
                return TryParseFromNumber(cellValue, out value);
            }
            else
            {
                return TryParseFromText(cellValue, out value);
            }
        }

        private static ResourcePropertyParseResultKind TryParseFromNumber(string cellValue, out object? value)
        {
            var hasDoubleValue = double.TryParse(cellValue, out var doubleValue);
            if (!hasDoubleValue)
            {
                value = null;
                return ResourcePropertyParseResultKind.Invalid;
            }

            try
            {
                var dateTimeValue = DateTime.FromOADate(doubleValue);
                value = new DateTimeOffset(dateTimeValue);
                return ResourcePropertyParseResultKind.Success;
            }
            catch (ArgumentException)
            {
                value = null;
                return ResourcePropertyParseResultKind.Invalid;
            }
        }

        private static ResourcePropertyParseResultKind TryParseFromText(string cellValue, out object? value)
        {
            var hasDateTimeOffsetValue = DateTimeOffset.TryParse(cellValue, out var dateTimeOffsetValue);
            if (!hasDateTimeOffsetValue)
            {
                value = null;
                return ResourcePropertyParseResultKind.Invalid;
            }

            value = dateTimeOffsetValue;
            return ResourcePropertyParseResultKind.Success;
        }
    }
}