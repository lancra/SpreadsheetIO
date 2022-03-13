using Ardalis.GuardClauses;
using LanceC.SpreadsheetIO.Mapping2;
using LanceC.SpreadsheetIO.Mapping2.Options;
using LanceC.SpreadsheetIO.Writing.Internal.Serializing;

namespace LanceC.SpreadsheetIO.Writing.Internal.Writers;

internal class SpreadsheetPageMapWriter : ISpreadsheetPageMapWriter
{
    private readonly IResourcePropertySerializer _resourcePropertySerializer;

    public SpreadsheetPageMapWriter(IResourcePropertySerializer resourcePropertySerializer)
    {
        _resourcePropertySerializer = resourcePropertySerializer;
    }

    public void Write<TResource>(
        IWritingSpreadsheetPage spreadsheetPage,
        IEnumerable<TResource> resources,
        ResourceMap map)
        where TResource : class
    {
        Guard.Against.Null(spreadsheetPage, nameof(spreadsheetPage));
        Guard.Against.Null(map, nameof(map));

        var numberedPropertyMaps = GetNumberedPropertyMaps(map);

        var headerRowNumberOption = map.Options.Find<HeaderRowNumberResourceMapOption>();
        if (headerRowNumberOption is not null && spreadsheetPage.CurrentRowNumber != headerRowNumberOption.Number)
        {
            spreadsheetPage.AdvanceToRow(headerRowNumberOption.Number);
        }

        foreach (var (columnNumber, propertyMap) in numberedPropertyMaps)
        {
            AdvanceToColumnIfUnmatched(spreadsheetPage, columnNumber);

            var cellStyle = default(WritingCellStyle?);
            var headerStyleOption = propertyMap.Options.Find<HeaderStyleMapOption>();
            if (headerStyleOption is not null)
            {
                cellStyle = new WritingCellStyle(headerStyleOption!.Key);
            }

            var cellValue = new WritingCellValue(propertyMap.Key.Name);
            var cell = new WritingCell(cellValue, cellStyle);

            spreadsheetPage.AddCell(cell);
        }

        spreadsheetPage.AdvanceRow();

        foreach (var resource in resources ?? Array.Empty<TResource>())
        {
            foreach (var (columnNumber, propertyMap) in numberedPropertyMaps)
            {
                AdvanceToColumnIfUnmatched(spreadsheetPage, columnNumber);

                var cellStyle = default(WritingCellStyle?);
                var bodyStyleOption = propertyMap.Options.Find<BodyStyleMapOption>();
                if (bodyStyleOption is not null)
                {
                    cellStyle = new WritingCellStyle(bodyStyleOption!.Key);
                }

                var cellValue = _resourcePropertySerializer.Serialize(resource, propertyMap);
                var cell = new WritingCell(cellValue, cellStyle);

                spreadsheetPage.AddCell(cell);
            }

            spreadsheetPage.AdvanceRow();
        }
    }

    private static IReadOnlyCollection<(uint Number, PropertyMap Map)> GetNumberedPropertyMaps(ResourceMap map)
    {
        var numberedPropertyMaps = map.Properties.Where(propertyMap => propertyMap.Key.Number.HasValue)
            .Select(propertyMap => (Number: propertyMap.Key.Number!.Value, Map: propertyMap))
            .ToList();

        var columnIndex = 1U;
        var nonNumberedPropertyMaps = map.Properties.Where(propertyMap => !propertyMap.Key.Number.HasValue);
        foreach (var propertyMap in nonNumberedPropertyMaps)
        {
            while (numberedPropertyMaps.Any(propertyMap => propertyMap.Number == columnIndex))
            {
                columnIndex++;
            }

            numberedPropertyMaps.Add(new(columnIndex, propertyMap));
            columnIndex++;
        }

        return numberedPropertyMaps.OrderBy(propertyMap => propertyMap.Number)
            .ToArray();
    }

    private static void AdvanceToColumnIfUnmatched(IWritingSpreadsheetPage spreadsheetPage, uint columnNumber)
    {
        if (spreadsheetPage.CurrentColumnNumber != columnNumber)
        {
            spreadsheetPage.AdvanceToColumn(columnNumber);
        }
    }
}
