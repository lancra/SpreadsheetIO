using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Options.Extensions;
using LanceC.SpreadsheetIO.Reading.Failures;
using LanceC.SpreadsheetIO.Reading.Internal.Creation;
using LanceC.SpreadsheetIO.Reading.Internal.Properties;

namespace LanceC.SpreadsheetIO.Reading.Internal.Readers;

internal class MappedBodyRowReader : IMappedBodyRowReader
{
    private readonly IResourcePropertyCollectionFactory _resourcePropertyCollectionFactory;
    private readonly IResourcePropertyValueResolver _resourcePropertyValueResolver;
    private readonly IResourcePropertyDefaultValueResolver _resourcePropertyDefaultValueResolver;
    private readonly IResourceCreator _resourceCreator;

    public MappedBodyRowReader(
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

    public ResourceReadingResult<TResource> Read<TResource>(
        IWorksheetElementReader reader,
        ResourceMap map,
        IResourcePropertyHeaders propertyHeaders)
        where TResource : class
    {
        var propertyValues = _resourcePropertyCollectionFactory.CreateValues();
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
            var isRequired = propertyMap.Options.IsRequired(PropertyElementKind.Body);
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

        var resource = default(NumberedResource<TResource>?);
        var resourceFailure = default(ResourceReadingFailure?);
        if (!missingPropertyFailures.Any() && !invalidPropertyFailures.Any())
        {
            resource = new NumberedResource<TResource>(rowNumber, _resourceCreator.Create<TResource>(map, propertyValues));
        }
        else
        {
            resourceFailure = new ResourceReadingFailure(rowNumber, missingPropertyFailures, invalidPropertyFailures);
        }

        var resourceResult = new ResourceReadingResult<TResource>(resource, resourceFailure);
        return resourceResult;
    }
}
