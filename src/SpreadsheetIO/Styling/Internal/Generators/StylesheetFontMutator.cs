using System.Collections.Generic;
using LanceC.SpreadsheetIO.Shared.Internal;
using LanceC.SpreadsheetIO.Styling.Internal.Indexers;
using OpenXml = DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Styling.Internal.Generators
{
    internal class StylesheetFontMutator : IStylesheetMutator
    {
        private readonly IFontIndexer _fontIndexer;

        public StylesheetFontMutator(IFontIndexer fontIndexer)
        {
            _fontIndexer = fontIndexer;
        }

        public void Mutate(OpenXml.Stylesheet stylesheet)
            => stylesheet.Fonts = GenerateFonts(_fontIndexer.Resources);

        private static OpenXml.Fonts GenerateFonts(IReadOnlyCollection<Font> fonts)
        {
            var openXmlFonts = new OpenXml.Fonts();
            foreach (var font in fonts)
            {
                var openXmlFont = new OpenXml.Font
                {
                    FontSize = new OpenXml.FontSize
                    {
                        Val = font.Size,
                    },
                    Color = new OpenXml.Color
                    {
                        Rgb = font.Color.ToHex(),
                    },
                    FontName = new OpenXml.FontName
                    {
                        Val = font.Name,
                    },
                };

                if (font.IsBold)
                {
                    openXmlFont.Bold = new OpenXml.Bold();
                }

                if (font.IsItalic)
                {
                    openXmlFont.Italic = new OpenXml.Italic();
                }

                openXmlFonts.Append(openXmlFont);
            }

            return openXmlFonts;
        }
    }
}
