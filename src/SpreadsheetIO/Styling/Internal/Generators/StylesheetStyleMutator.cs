using System;
using System.Collections.Generic;
using System.Linq;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Styling.Internal.Indexers;
using OpenXml = DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Styling.Internal.Generators
{
    internal class StylesheetStyleMutator : IStylesheetMutator
    {
        private readonly IBorderIndexer _borderIndexer;
        private readonly IFillIndexer _fillIndexer;
        private readonly IFontIndexer _fontIndexer;
        private readonly IStyleIndexer _styleIndexer;

        public StylesheetStyleMutator(
            IBorderIndexer borderIndexer,
            IFillIndexer fillIndexer,
            IFontIndexer fontIndexer,
            IStyleIndexer styleIndexer)
        {
            _borderIndexer = borderIndexer;
            _fillIndexer = fillIndexer;
            _fontIndexer = fontIndexer;
            _styleIndexer = styleIndexer;
        }

        public void Mutate(OpenXml.Stylesheet stylesheet)
        {
            var styleKeyValues = _styleIndexer.Keys
                .Select(key => new IndexedKeyValue(key, _styleIndexer[key]))
                .ToArray();
            stylesheet.CellFormats = GenerateCellFormats(
                GenerateCellFormatCollection(styleKeyValues));

            var excelStyleKeyValues = styleKeyValues.Where(styleKeyValue => styleKeyValue.Key.Kind == IndexerKeyKind.Excel)
                .ToArray();
            stylesheet.CellStyleFormats = GenerateCellStyleFormats(
                GenerateCellFormatCollection(excelStyleKeyValues));
            stylesheet.CellStyles = GenerateCellStyles(excelStyleKeyValues);
        }

        private static OpenXml.CellStyleFormats GenerateCellStyleFormats(IReadOnlyCollection<OpenXml.CellFormat> cellFormatCollection)
        {
            var cellStyleFormats = new OpenXml.CellStyleFormats();
            foreach (var cellFormat in cellFormatCollection)
            {
                cellStyleFormats.Append(cellFormat);
            }

            return cellStyleFormats;
        }

        private static OpenXml.CellFormats GenerateCellFormats(IReadOnlyCollection<OpenXml.CellFormat> cellFormatCollection)
        {
            var cellFormats = new OpenXml.CellFormats();
            foreach (var cellFormat in cellFormatCollection)
            {
                cellFormats.Append(cellFormat);
            }

            return cellFormats;
        }

        private static OpenXml.CellStyles GenerateCellStyles(IReadOnlyCollection<IndexedKeyValue> styleKeyValues)
        {
            var cellStyles = new OpenXml.CellStyles();

            foreach (var styleKeyValue in styleKeyValues)
            {
                if (!styleKeyValue.Value.Resource.BuiltInId.HasValue)
                {
                    throw new InvalidOperationException(
                        $"The {styleKeyValue.Key.Name} Excel style has not been set up with an identifier. " +
                        "Please contact the package maintainer.");
                }

                var cellStyle = new OpenXml.CellStyle
                {
                    Name = styleKeyValue.Key.Name,
                    FormatId = styleKeyValue.Value.Index,
                    BuiltinId = styleKeyValue.Value.Resource.BuiltInId.Value,
                };

                cellStyles.Append(cellStyle);
            }

            return cellStyles;
        }

        private IReadOnlyCollection<OpenXml.CellFormat> GenerateCellFormatCollection(
            IReadOnlyCollection<IndexedKeyValue> styleKeyValues)
        {
            var cellFormatCollection = new List<OpenXml.CellFormat>();

            foreach (var styleKeyValue in styleKeyValues)
            {
                var cellFormat = new OpenXml.CellFormat
                {
                    NumberFormatId = 0,
                    FontId = _fontIndexer[styleKeyValue.Value.Resource.Font],
                    FillId = _fillIndexer[styleKeyValue.Value.Resource.Fill],
                    BorderId = _borderIndexer[styleKeyValue.Value.Resource.Border],
                    ApplyNumberFormat = false,
                    ApplyFont = true,
                    ApplyFill = true,
                    ApplyBorder = true,
                    ApplyAlignment = false,
                    ApplyProtection = false,
                };

                cellFormatCollection.Add(cellFormat);
            }

            return cellFormatCollection;
        }

        private class IndexedKeyValue
        {
            public IndexedKeyValue(IndexerKey key, IndexedResource<Style> value)
            {
                Key = key;
                Value = value;
            }

            public IndexerKey Key { get; }

            public IndexedResource<Style> Value { get; }
        }
    }
}
