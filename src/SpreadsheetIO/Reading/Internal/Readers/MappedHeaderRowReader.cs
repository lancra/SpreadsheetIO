using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Options.Extensions;
using LanceC.SpreadsheetIO.Reading.Failures;
using LanceC.SpreadsheetIO.Reading.Internal.Properties;

namespace LanceC.SpreadsheetIO.Reading.Internal.Readers;

internal class MappedHeaderRowReader : IMappedHeaderRowReader
{
    private readonly IResourcePropertyCollectionFactory _resourcePropertyCollectionFactory;

    public MappedHeaderRowReader(IResourcePropertyCollectionFactory resourcePropertyCollectionFactory)
    {
        _resourcePropertyCollectionFactory = resourcePropertyCollectionFactory;
    }

    public HeaderRowReadingResult<TResource> Read<TResource>(IWorksheetElementReader reader, ResourceMap map)
        where TResource : class
    {
        var propertyHeaders = _resourcePropertyCollectionFactory.CreateHeaders();
        var missingHeaderFailures = new List<MissingHeaderReadingFailure>();
        var invalidHeaderFailures = new List<InvalidHeaderReadingFailure>();

        var headerRowNumber = map.Options.GetHeaderRowNumber();

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

            var numberPropertyMap = map.Properties.Find(location.Column.Number);
            var namePropertyMap = map.Properties.Find(value);
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
            var isRequired = propertyMap.Options.IsRequired(PropertyElementKind.Header);

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
}
