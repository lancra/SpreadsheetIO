using Ardalis.GuardClauses;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Extensions;
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
        ResourceMap<TResource> map)
        where TResource : class
    {
        Guard.Against.Null(spreadsheetPage, nameof(spreadsheetPage));
        Guard.Against.Null(map, nameof(map));

        var numberedPropertyMaps = GetNumberedPropertyMaps(map);

        var headerRowNumberExtension = map.Options.FindExtension<HeaderRowNumberResourceMapOptionsExtension>();
        if (headerRowNumberExtension is not null && spreadsheetPage.CurrentRowNumber != headerRowNumberExtension.Number)
        {
            spreadsheetPage.AdvanceToRow(headerRowNumberExtension.Number);
        }

        foreach (var (columnNumber, propertyMap) in numberedPropertyMaps)
        {
            AdvanceToColumnIfUnmatched(spreadsheetPage, columnNumber);

            var cellStyle = default(WritingCellStyle?);
            var headerStyleExtension = propertyMap.Options.FindExtension<HeaderStyleMapOptionsExtension>();
            if (headerStyleExtension is not null)
            {
                cellStyle = new WritingCellStyle(headerStyleExtension!.Key);
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
                var bodyStyleExtension = propertyMap.Options.FindExtension<BodyStyleMapOptionsExtension>();
                if (bodyStyleExtension is not null)
                {
                    cellStyle = new WritingCellStyle(bodyStyleExtension!.Key);
                }

                var cellValue = _resourcePropertySerializer.Serialize(resource, propertyMap);
                var cell = new WritingCell(cellValue, cellStyle);

                spreadsheetPage.AddCell(cell);
            }

            spreadsheetPage.AdvanceRow();
        }
    }

    private static IReadOnlyCollection<(uint Number, PropertyMap<TResource> Map)> GetNumberedPropertyMaps<TResource>(
        ResourceMap<TResource> map)
        where TResource : class
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
