using System;
using System.Collections.Generic;
using System.Drawing;
using LanceC.SpreadsheetIO.Shared.Internal;
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
            var openXmlFills = new OpenXml.Fills
            {
                Count = Convert.ToUInt32(fills.Count),
            };

            foreach (var fill in fills)
            {
                var openXmlFill = new OpenXml.Fill
                {
                    PatternFill = new OpenXml.PatternFill
                    {
                        PatternType = fill.Kind.OpenXmlValue,
                    },
                };

                if (fill.ForegroundColor != Color.White)
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
