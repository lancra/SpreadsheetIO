using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.SmartEnum;

namespace LanceC.SpreadsheetIO.Tests.Testing
{
    public class ExcelElementKind : SmartEnum<ExcelElementKind>
    {
        public static readonly ExcelElementKind Alignment =
            new(
                1,
                "Alignment",
                "alignment",
                false,
                ExcelAttributeKind.Horizontal,
                ExcelAttributeKind.JustifyLastLine,
                ExcelAttributeKind.Vertical);

        public static readonly ExcelElementKind Border =
            new(
                2,
                "Border",
                "border",
                false);

        public static readonly ExcelElementKind BorderBottom =
            new(
                3,
                "Border Bottom",
                "bottom",
                false,
                ExcelAttributeKind.Style);

        public static readonly ExcelElementKind BorderDiagonal =
            new(
                4,
                "Border Diagonal",
                "diagonal",
                false,
                ExcelAttributeKind.Style);

        public static readonly ExcelElementKind BorderLeft =
            new(
                5,
                "Border Left",
                "left",
                false,
                ExcelAttributeKind.Style);

        public static readonly ExcelElementKind BorderRight =
            new(
                6,
                "Border Right",
                "right",
                false,
                ExcelAttributeKind.Style);

        public static readonly ExcelElementKind Borders =
            new(
                7,
                "Borders",
                "borders",
                false,
                ExcelAttributeKind.Count);

        public static readonly ExcelElementKind BorderTop =
            new(
                8,
                "Border Top",
                "top",
                false,
                ExcelAttributeKind.Style);

        public static readonly ExcelElementKind Cell =
            new(
                9,
                "Cell",
                "c",
                false,
                ExcelAttributeKind.Row,
                ExcelAttributeKind.Type);

        public static readonly ExcelElementKind CellFormat =
            new(
                10,
                "Cell Format",
                "cellXf",
                false,
                ExcelAttributeKind.ApplyAlignment,
                ExcelAttributeKind.ApplyBorder,
                ExcelAttributeKind.ApplyFill,
                ExcelAttributeKind.ApplyFont,
                ExcelAttributeKind.ApplyNumberFormat,
                ExcelAttributeKind.ApplyProtection,
                ExcelAttributeKind.BorderId,
                ExcelAttributeKind.FillId,
                ExcelAttributeKind.FontId,
                ExcelAttributeKind.NumberFormatId,
                ExcelAttributeKind.CellFormatId);

        public static readonly ExcelElementKind CellFormats =
            new(
                11,
                "Cell Formats",
                "cellXfs",
                false,
                ExcelAttributeKind.Count);

        public static readonly ExcelElementKind CellStyle =
            new(
                12,
                "Cell Style",
                "cellStyle",
                false,
                ExcelAttributeKind.BuiltInId,
                ExcelAttributeKind.ElementName,
                ExcelAttributeKind.CellFormatId);

        public static readonly ExcelElementKind CellStyleFormats =
            new(
                13,
                "Cell Style Formats",
                "cellStyleXfs",
                false,
                ExcelAttributeKind.Count);

        public static readonly ExcelElementKind CellStyles =
            new(
                14,
                "Cell Styles",
                "cellStyles",
                false,
                ExcelAttributeKind.Count);

        public static readonly ExcelElementKind CellValue =
            new(
                15,
                "Cell Value",
                "v",
                false);

        public static readonly ExcelElementKind Color =
            new(
                16,
                "Color",
                "color",
                false,
                ExcelAttributeKind.Auto,
                ExcelAttributeKind.RedGreenBlue,
                ExcelAttributeKind.Theme);

        public static readonly ExcelElementKind Fill =
            new(
                17,
                "Fill",
                "fill",
                false);

        public static readonly ExcelElementKind FillForegroundColor =
            new(
                18,
                "Fill Foreground Color",
                "fgColor",
                false,
                ExcelAttributeKind.Indexed,
                ExcelAttributeKind.RedGreenBlue,
                ExcelAttributeKind.Theme,
                ExcelAttributeKind.Tint);

        public static readonly ExcelElementKind Fills =
            new(
                19,
                "Fills",
                "fills",
                false,
                ExcelAttributeKind.Count);

        public static readonly ExcelElementKind Font =
            new(
                20,
                "Font",
                "font",
                false);

        public static readonly ExcelElementKind FontName =
            new(
                21,
                "Font Name",
                "name",
                false,
                ExcelAttributeKind.ElementValue);

        public static readonly ExcelElementKind Fonts =
            new(
                22,
                "Fonts",
                "fonts",
                false,
                ExcelAttributeKind.Count);

        public static readonly ExcelElementKind FontSize =
            new(
                23,
                "Font Size",
                "sz",
                false,
                ExcelAttributeKind.ElementValue);

        public static readonly ExcelElementKind NumberFormat =
            new(
                24,
                "Number Format",
                "numFmt",
                false,
                ExcelAttributeKind.FormatCode,
                ExcelAttributeKind.NumberFormatId);

        public static readonly ExcelElementKind NumberFormats =
            new(
                25,
                "Number Formats",
                "numFmts",
                false,
                ExcelAttributeKind.Count);

        public static readonly ExcelElementKind PatternFill =
            new(
                26,
                "Pattern Fill",
                "patternFill",
                false,
                ExcelAttributeKind.PatternType);

        public static readonly ExcelElementKind Row =
            new(
                27,
                "Row",
                "r",
                false,
                ExcelAttributeKind.Row);

        public static readonly ExcelElementKind SharedStringItem =
            new(
                28,
                "Shared String Item",
                "si",
                false);

        public static readonly ExcelElementKind SharedStringTable =
            new(
                29,
                "Shared String Table",
                "sst",
                true);

        public static readonly ExcelElementKind Sheet =
            new(
                30,
                "Sheet",
                "sheet",
                false,
                ExcelAttributeKind.ElementName,
                ExcelAttributeKind.SheetId);

        public static readonly ExcelElementKind SheetData =
            new(
                31,
                "Sheet Data",
                "sheetData",
                false);

        public static readonly ExcelElementKind Sheets =
            new(
                32,
                "Sheets",
                "sheets",
                false);

        public static readonly ExcelElementKind Stylesheet =
            new(
                33,
                "Stylesheet",
                "styleSheet",
                true);

        public static readonly ExcelElementKind Text =
            new(
                34,
                "Text",
                "text",
                false);

        public static readonly ExcelElementKind Workbook =
            new(
                35,
                "Workbook",
                "workbook",
                true);

        public static readonly ExcelElementKind Worksheet =
            new(
                36,
                "Worksheet",
                "worksheet",
                true);

        private static readonly Lazy<Dictionary<string, ExcelElementKind>> _fromLocalName =
            new(() => List.ToDictionary(k => k.LocalName));

        private ExcelElementKind(int id, string name, string localName, bool isRoot, params ExcelAttributeKind[] attributeKinds)
            : base(name, id)
        {
            LocalName = localName;
            IsRoot = isRoot;
            AttributeKinds = attributeKinds;
        }

        public string LocalName { get; }

        public bool IsRoot { get; }

        public IReadOnlyCollection<ExcelAttributeKind> AttributeKinds { get; }

        public static bool TryFromLocalName(string localName, out ExcelElementKind? result)
        {
            if (string.IsNullOrEmpty(localName))
            {
                result = default;
                return false;
            }

            return _fromLocalName.Value.TryGetValue(localName, out result);
        }
    }
}
