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

        private readonly IDictionary<IndexerKey, uint> _excelFormatIdLookup = new Dictionary<IndexerKey, uint>();
        private uint _excelFormatIdProvider = 0U;

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
            var excelStyleKeyValues = styleKeyValues.Where(styleKeyValue => styleKeyValue.Key.Kind == IndexerKeyKind.Excel)
                .ToArray();

            stylesheet.CellStyleFormats = GenerateCellStyleFormats(excelStyleKeyValues);
            stylesheet.CellStyles = GenerateCellStyles(excelStyleKeyValues);
            stylesheet.CellFormats = GenerateCellFormats(styleKeyValues);

            _excelFormatIdLookup.Clear();
            _excelFormatIdProvider = 0U;
        }

        private OpenXml.CellStyleFormats GenerateCellStyleFormats(IReadOnlyCollection<IndexedKeyValue> styleKeyValues)
        {
            var cellStyleFormats = new OpenXml.CellStyleFormats
            {
                Count = Convert.ToUInt32(styleKeyValues.Count),
            };

            foreach (var styleKeyValue in styleKeyValues)
            {
                var cellFormat = GenerateCellFormat(styleKeyValue);
                cellStyleFormats.Append(cellFormat);

                _excelFormatIdLookup.Add(styleKeyValue.Key, _excelFormatIdProvider);
                _excelFormatIdProvider++;
            }

            return cellStyleFormats;
        }

        private OpenXml.CellFormats GenerateCellFormats(IReadOnlyCollection<IndexedKeyValue> styleKeyValues)
        {
            var cellFormats = new OpenXml.CellFormats
            {
                Count = Convert.ToUInt32(styleKeyValues.Count),
            };

            foreach (var styleKeyValue in styleKeyValues)
            {
                var cellFormat = GenerateCellFormat(styleKeyValue);

                _excelFormatIdLookup.TryGetValue(styleKeyValue.Key, out var formatId);
                cellFormat.FormatId = formatId;

                cellFormats.Append(cellFormat);
            }

            return cellFormats;
        }

        private OpenXml.CellStyles GenerateCellStyles(IReadOnlyCollection<IndexedKeyValue> styleKeyValues)
        {
            var cellStyles = new OpenXml.CellStyles
            {
                Count = Convert.ToUInt32(styleKeyValues.Count),
            };

            foreach (var styleKeyValue in styleKeyValues)
            {
                if (!styleKeyValue.Value.Resource.BuiltInId.HasValue)
                {
                    throw new InvalidOperationException(
                        $"The {styleKeyValue.Key.Name} Excel style has not been set up with an identifier. " +
                        "Please contact the package maintainer.");
                }

                var formatId = _excelFormatIdLookup[styleKeyValue.Key];
                var cellStyle = new OpenXml.CellStyle
                {
                    Name = styleKeyValue.Key.Name,
                    FormatId = formatId,
                    BuiltinId = styleKeyValue.Value.Resource.BuiltInId.Value,
                };

                cellStyles.Append(cellStyle);
            }

            return cellStyles;
        }

        private OpenXml.CellFormat GenerateCellFormat(IndexedKeyValue styleKeyValue)
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
            return cellFormat;
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
