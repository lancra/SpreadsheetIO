using System.Collections.Generic;
using System.Linq;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Extensions;
using LanceC.SpreadsheetIO.Reading.Failures;
using LanceC.SpreadsheetIO.Reading.Internal.Properties;

namespace LanceC.SpreadsheetIO.Reading.Internal.Readers
{
    internal class SpreadsheetPageMapReader : ISpreadsheetPageMapReader
    {
        private readonly IResourcePropertyCollectionFactory _resourcePropertyCollectionFactory;
        private readonly IResourcePropertyValueResolver _resourcePropertyValueResolver;
        private readonly IResourcePropertyDefaultValueResolver _resourcePropertyDefaultValueResolver;
        private readonly IResourceCreator _resourceCreator;

        public SpreadsheetPageMapReader(
            IResourcePropertyCollectionFactory resourcePropertyCollectionFactory,
            IResourcePropertyValueResolver resourcePropertyValueResolver,
            IResourcePropertyDefaultValueResolver resourcePropertyDefaultValueResolver,
            IResourceCreator resourceCreator)
        {
            _resourcePropertyCollectionFactory = resourcePropertyCollectionFactory;
            _resourcePropertyValueResolver = resourcePropertyValueResolver;
            _resourcePropertyDefaultValueResolver = resourcePropertyDefaultValueResolver;
            _resourceCreator = resourceCreator;
        }

        public HeaderRowReadingResult<TResource> ReadHeaderRow<TResource>(
            IWorksheetElementReader reader,
            ResourceMap<TResource> map)
            where TResource : class
        {
            var propertyHeaders = _resourcePropertyCollectionFactory.CreateHeaders<TResource>();
            var missingHeaderFailures = new List<MissingHeaderReadingFailure>();
            var invalidHeaderFailures = new List<InvalidHeaderReadingFailure>();

            var headerRowNumberExtension = map.Options.FindExtension<HeaderRowNumberResourceMapOptionsExtension>();
            var headerRowNumber = headerRowNumberExtension?.Number ?? 1U;

            var hasHeaderRow = reader.ReadToRow(headerRowNumber);
            if (!hasHeaderRow)
            {
                return new HeaderRowReadingResult<TResource>(
                    propertyHeaders,
                    new HeaderReadingFailure(true, missingHeaderFailures, invalidHeaderFailures));
            }

            while (reader.ReadNextCell())
            {
                var location = reader.GetCellLocation();
                var value = reader.GetCellValue();

                var numberPropertyMap = map.Properties.SingleOrDefault(map => map.Key.Number == location.Column.Number);
                var namePropertyMap = map.Properties.SingleOrDefault(map => map.Key.Name == value && !map.Key.IsNameIgnored);
                var propertyMap = numberPropertyMap ?? namePropertyMap;

                if (propertyMap is null)
                {
                    continue;
                }
                else if (numberPropertyMap is not null &&
                    namePropertyMap is not null &&
                    numberPropertyMap != namePropertyMap)
                {
                    var invalidHeader = new InvalidHeaderReadingFailure(numberPropertyMap.Key, namePropertyMap.Key);
                    invalidHeaderFailures.Add(invalidHeader);
                }
                else
                {
                    propertyHeaders.Add(propertyMap, location.Column.Number);
                }
            }

            foreach (var propertyMap in map.Properties)
            {
                var isIndexed = propertyHeaders.ContainsMap(propertyMap);

                var optionalExtension = propertyMap.Options.FindExtension<OptionalPropertyMapOptionsExtension>();
                var isRequired = optionalExtension is null ||
                    (optionalExtension.Kind != PropertyElementKind.All && optionalExtension.Kind != PropertyElementKind.Header);

                if (!isIndexed && isRequired)
                {
                    var missingHeader = new MissingHeaderReadingFailure(propertyMap.Key);
                    missingHeaderFailures.Add(missingHeader);
                }
            }

            var headerFailure = default(HeaderReadingFailure?);
            if (missingHeaderFailures.Any() || invalidHeaderFailures.Any())
            {
                headerFailure = new HeaderReadingFailure(false, missingHeaderFailures, invalidHeaderFailures);
            }

            var headerRowResult = new HeaderRowReadingResult<TResource>(propertyHeaders, headerFailure);
            return headerRowResult;
        }

        public BodyRowReadingResult<TResource> ReadBodyRow<TResource>(
            IWorksheetElementReader reader,
            ResourceMap<TResource> map,
            IResourcePropertyHeaders<TResource> propertyHeaders)
            where TResource : class
        {
            var propertyValues = _resourcePropertyCollectionFactory.CreateValues<TResource>();
            var missingPropertyFailures = new List<MissingResourcePropertyReadingFailure>();
            var invalidPropertyFailures = new List<InvalidResourcePropertyReadingFailure>();

            var rowNumber = reader.GetRowNumber();
            var columnNumbers = propertyHeaders.ColumnNumbers;
            var headerUsageTracker = columnNumbers.ToDictionary(columnNumber => columnNumber, value => false);

            while (reader.ReadNextCell())
            {
                var location = reader.GetCellLocation();
                var hasPropertyMap = propertyHeaders.TryGetMap(location.Column.Number, out var propertyMap);
                if (!hasPropertyMap)
                {
                    continue;
                }

                headerUsageTracker[location.Column.Number] = true;

                var cellValue = reader.GetCellValue();

                var isValueResolved = _resourcePropertyValueResolver.TryResolve(cellValue, propertyMap!, out var value);
                if (isValueResolved)
                {
                    propertyValues.Add(propertyMap!, value);
                }
                else
                {
                    var invalidProperty = new InvalidResourcePropertyReadingFailure(location.Column.Number, cellValue);
                    invalidPropertyFailures.Add(invalidProperty);
                }
            }

            foreach (var columnNumber in columnNumbers)
            {
                if (headerUsageTracker[columnNumber])
                {
                    continue;
                }

                var propertyMap = propertyHeaders.GetMap(columnNumber);

                var optionalExtension = propertyMap.Options.FindExtension<OptionalPropertyMapOptionsExtension>();
                var isRequired = optionalExtension is null ||
                    (optionalExtension.Kind != PropertyElementKind.All && optionalExtension.Kind != PropertyElementKind.Body);

                var hasDefaultValue = _resourcePropertyDefaultValueResolver
                    .TryResolve(
                        propertyMap,
                        isRequired ? ResourcePropertyParseResultKind.Missing : ResourcePropertyParseResultKind.Empty,
                        out var defaultValue);

                if (hasDefaultValue)
                {
                    propertyValues.Add(propertyMap, defaultValue);
                }
                else if (isRequired)
                {
                    var missingProperty = new MissingResourcePropertyReadingFailure(columnNumber);
                    missingPropertyFailures.Add(missingProperty);
                }
            }

            var resource = default(TResource);
            var resourceFailure = default(ResourceReadingFailure?);
            if (!missingPropertyFailures.Any() && !invalidPropertyFailures.Any())
            {
                resource = _resourceCreator.Create(map, propertyValues);
            }
            else
            {
                resourceFailure = new ResourceReadingFailure(rowNumber, missingPropertyFailures, invalidPropertyFailures);
            }

            var bodyRowResult = new BodyRowReadingResult<TResource>(resource, resourceFailure);
            return bodyRowResult;
        }
    }
}
