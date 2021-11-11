using LanceC.SpreadsheetIO.Shared.Internal;
using LanceC.SpreadsheetIO.Styling.Internal.Indexers;
using OpenXml = DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Styling.Internal.Generators
{
    internal class StylesheetBorderMutator : IStylesheetMutator
    {
        private readonly IBorderIndexer _borderIndexer;

        public StylesheetBorderMutator(IBorderIndexer borderIndexer)
        {
            _borderIndexer = borderIndexer;
        }

        public void Mutate(OpenXml.Stylesheet stylesheet)
            => stylesheet.Borders = GenerateBorders(_borderIndexer.Resources);

        private static OpenXml.Borders GenerateBorders(IReadOnlyCollection<Border> borders)
        {
            var openXmlBorders = new OpenXml.Borders
            {
                Count = Convert.ToUInt32(borders.Count),
            };

            foreach (var border in borders)
            {
                var openXmlBorder = new OpenXml.Border
                {
                    LeftBorder = GenerateBorderLine(border.LeftLine, new OpenXml.LeftBorder()),
                    RightBorder = GenerateBorderLine(border.RightLine, new OpenXml.RightBorder()),
                    TopBorder = GenerateBorderLine(border.TopLine, new OpenXml.TopBorder()),
                    BottomBorder = GenerateBorderLine(border.BottomLine, new OpenXml.BottomBorder()),
                    DiagonalBorder = new OpenXml.DiagonalBorder(),
                };

                openXmlBorders.Append(openXmlBorder);
            }

            return openXmlBorders;
        }

        private static TBorderLine GenerateBorderLine<TBorderLine>(BorderLine borderLine, TBorderLine openXmlBorderLine)
            where TBorderLine : OpenXml.BorderPropertiesType
        {
            if (borderLine.Kind != BorderLineKind.None)
            {
                openXmlBorderLine.Color = new OpenXml.Color();
                if (borderLine.Color == System.Drawing.Color.Black)
                {
                    openXmlBorderLine.Color.Auto = true;
                }
                else
                {
                    openXmlBorderLine.Color.Rgb = borderLine.Color.ToHex();
                }

                openXmlBorderLine.Style = borderLine.Kind.OpenXmlValue;
            }

            return openXmlBorderLine;
        }
    }
}
