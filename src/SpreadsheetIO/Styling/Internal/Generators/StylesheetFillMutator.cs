using System.Collections.Generic;
using LanceC.SpreadsheetIO.Shared;
using LanceC.SpreadsheetIO.Styling.Internal.Indexers;
using OpenXml = DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Styling.Internal.Generators
{
    internal class StylesheetFillMutator : IStylesheetMutator
    {
        private readonly IFillIndexer _fillIndexer;

        public StylesheetFillMutator(IFillIndexer fillIndexer)
        {
            _fillIndexer = fillIndexer;
        }

        public void Mutate(OpenXml.Stylesheet stylesheet)
            => stylesheet.Fills = GenerateFills(_fillIndexer.Resources);

        private static OpenXml.Fills GenerateFills(IReadOnlyCollection<Fill> fills)
        {
            var openXmlFills = new OpenXml.Fills();
            foreach (var fill in fills)
            {
                var openXmlFill = new OpenXml.Fill
                {
                    PatternFill = new OpenXml.PatternFill
                    {
                        PatternType = fill.Kind.OpenXmlValue,
                    },
                };

                if (fill.Kind != FillKind.None)
                {
                    openXmlFill.PatternFill.ForegroundColor = new OpenXml.ForegroundColor
                    {
                        Rgb = fill.ForegroundColor.ToHex(),
                    };
                }

                openXmlFills.Append(openXmlFill);
            }

            return openXmlFills;
        }
    }
}
