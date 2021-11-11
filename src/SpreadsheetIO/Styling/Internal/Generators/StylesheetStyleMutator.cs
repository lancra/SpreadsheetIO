using LanceC.SpreadsheetIO.Properties;
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
        private readonly INumericFormatIndexer _numericFormatIndexer;
        private readonly IStyleIndexer _styleIndexer;

        private readonly IDictionary<Style, uint> _excelFormatIdLookup = new Dictionary<Style, uint>();
        private uint _excelFormatIdProvider = 0U;

        public StylesheetStyleMutator(
            IBorderIndexer borderIndexer,
            IFillIndexer fillIndexer,
            IFontIndexer fontIndexer,
            INumericFormatIndexer numericFormatIndexer,
            IStyleIndexer styleIndexer)
        {
            _borderIndexer = borderIndexer;
            _fillIndexer = fillIndexer;
            _fontIndexer = fontIndexer;
            _numericFormatIndexer = numericFormatIndexer;
            _styleIndexer = styleIndexer;
        }

        public void Mutate(OpenXml.Stylesheet stylesheet)
        {
            var excelStyleKeyValues = _styleIndexer.Keys
                .Where(key => key.Kind == IndexerKeyKind.Excel)
                .Select(key => new IndexedKeyValue(key, _styleIndexer[key]))
                .ToArray();

            stylesheet.CellStyleFormats = GenerateCellStyleFormats(excelStyleKeyValues);
            stylesheet.CellStyles = GenerateCellStyles(excelStyleKeyValues);
            stylesheet.CellFormats = GenerateCellFormats(_styleIndexer.Resources);

            _excelFormatIdLookup.Clear();
            _excelFormatIdProvider = 0U;
        }

        private static void SetCellFormatAlignment(OpenXml.CellFormat cellFormat, Alignment alignment)
        {
            if (!alignment.HorizontalKind.IsDefault || !alignment.VerticalKind.IsDefault)
            {
                cellFormat.ApplyAlignment = true;
                cellFormat.Alignment = new();

                if (!alignment.HorizontalKind.IsDefault)
                {
                    cellFormat.Alignment.Horizontal = alignment.HorizontalKind.OpenXmlValue;
                    if (alignment.HorizontalKind == HorizontalAlignmentKind.JustifyDistributed)
                    {
                        cellFormat.Alignment.JustifyLastLine = true;
                    }
                }

                if (!alignment.VerticalKind.IsDefault)
                {
                    cellFormat.Alignment.Vertical = alignment.VerticalKind.OpenXmlValue;
                }
            }
        }

        private OpenXml.CellStyleFormats GenerateCellStyleFormats(IReadOnlyCollection<IndexedKeyValue> styleKeyValues)
        {
            var cellStyleFormats = new OpenXml.CellStyleFormats
            {
                Count = Convert.ToUInt32(styleKeyValues.Count),
            };

            foreach (var styleKeyValue in styleKeyValues)
            {
                var style = styleKeyValue.Value.Resource;
                var cellFormat = GenerateCellFormat(style);
                cellStyleFormats.Append(cellFormat);

                _excelFormatIdLookup.Add(styleKeyValue.Value.Resource, _excelFormatIdProvider);
                _excelFormatIdProvider++;
            }

            return cellStyleFormats;
        }

        private OpenXml.CellFormats GenerateCellFormats(IReadOnlyCollection<Style> styles)
        {
            var cellFormats = new OpenXml.CellFormats
            {
                Count = Convert.ToUInt32(styles.Count),
            };

            foreach (var style in styles)
            {
                var cellFormat = GenerateCellFormat(style);

                _excelFormatIdLookup.TryGetValue(style, out var formatId);
                cellFormat.FormatId = formatId;

                SetCellFormatAlignment(cellFormat, style.Alignment);

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
                    throw new InvalidOperationException(Messages.InvalidExcelStyleSetup(styleKeyValue.Key.Name));
                }

                var formatId = _excelFormatIdLookup[styleKeyValue.Value.Resource];
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

        private OpenXml.CellFormat GenerateCellFormat(Style style)
        {
            var numericFormatId = _numericFormatIndexer[style.NumericFormat];
            var fontId = _fontIndexer[style.Font];
            var fillId = _fillIndexer[style.Fill];
            var borderId = _borderIndexer[style.Border];

            var cellFormat = new OpenXml.CellFormat
            {
                NumberFormatId = _numericFormatIndexer[style.NumericFormat],
                FontId = _fontIndexer[style.Font],
                FillId = _fillIndexer[style.Fill],
                BorderId = _borderIndexer[style.Border],
                ApplyNumberFormat = numericFormatId != 0U ? true : null,
                ApplyFont = fontId != 0U ? true : null,
                ApplyFill = fillId != 0U ? true : null,
                ApplyBorder = borderId != 0U ? true : null,
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
