using System.Collections.Generic;
using LanceC.SpreadsheetIO.Styling.Internal.Indexers;
using OpenXml = DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Styling.Internal.Generators
{
    internal class StylesheetNumericFormatMutator : IStylesheetMutator
    {
        private readonly INumericFormatIndexer _numericFormatIndexer;
        private readonly BuiltInNumericFormats _builtInNumericFormats;

        public StylesheetNumericFormatMutator(INumericFormatIndexer numericFormatIndexer, BuiltInNumericFormats builtInNumericFormats)
        {
            _numericFormatIndexer = numericFormatIndexer;
            _builtInNumericFormats = builtInNumericFormats;
        }

        public void Mutate(OpenXml.Stylesheet stylesheet)
            => stylesheet.NumberingFormats = GenerateNumberingFormats(_numericFormatIndexer.Resources);

        private OpenXml.NumberingFormats? GenerateNumberingFormats(IReadOnlyCollection<NumericFormat> numericFormats)
        {
            if (_numericFormatIndexer.NonBuiltInCount == 0)
            {
                return default;
            }

            var openXmlNumberingFormats = new OpenXml.NumberingFormats
            {
                Count = _numericFormatIndexer.NonBuiltInCount,
            };

            foreach (var numericFormat in numericFormats)
            {
                var isBuiltIn = _builtInNumericFormats.TryGetValue(numericFormat.Code, out var _);
                if (isBuiltIn)
                {
                    continue;
                }

                var id = _numericFormatIndexer[numericFormat];
                var openXmlNumberingFormat = new OpenXml.NumberingFormat
                {
                    NumberFormatId = id,
                    FormatCode = numericFormat.Code,
                };

                openXmlNumberingFormats.Append(openXmlNumberingFormat);
            }

            return openXmlNumberingFormats;
        }
    }
}
