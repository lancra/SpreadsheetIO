using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.SmartEnum;

namespace LanceC.SpreadsheetIO.Tests.Testing
{
    public class ExcelFileKind : SmartEnum<ExcelFileKind>
    {
        public static readonly ExcelFileKind SharedStrings =
            new(
                1,
                "Shared String",
                new ExcelUri("/xl/sharedStrings.xml"),
                ExcelElementKind.SharedStringTable,
                ExcelElementKind.SharedStringItem,
                ExcelElementKind.Text);

        public static readonly ExcelFileKind Styles =
            new(
                2,
                "Styles",
                new ExcelUri("/xl/styles.xml"),
                ExcelElementKind.Stylesheet,
                ExcelElementKind.Color,
                ExcelElementKind.NumberFormats,
                ExcelElementKind.NumberFormat,
                ExcelElementKind.Fonts,
                ExcelElementKind.Font,
                ExcelElementKind.FontSize,
                ExcelElementKind.FontName,
                ExcelElementKind.Fills,
                ExcelElementKind.Fill,
                ExcelElementKind.PatternFill,
                ExcelElementKind.FillForegroundColor,
                ExcelElementKind.Borders,
                ExcelElementKind.Border,
                ExcelElementKind.BorderLeft,
                ExcelElementKind.BorderRight,
                ExcelElementKind.BorderTop,
                ExcelElementKind.BorderBottom,
                ExcelElementKind.BorderDiagonal,
                ExcelElementKind.CellStyleFormats,
                ExcelElementKind.CellFormats,
                ExcelElementKind.CellFormat,
                ExcelElementKind.Alignment,
                ExcelElementKind.CellStyles,
                ExcelElementKind.CellStyle);

        public static readonly ExcelFileKind Workbook =
            new(
                3,
                "Workbook",
                new ExcelUri("/xl/workbook.xml"),
                ExcelElementKind.Workbook,
                ExcelElementKind.Sheets,
                ExcelElementKind.Sheet);

        public static readonly ExcelFileKind Worksheet =
            new(
                4,
                "Worksheet",
                new ExcelUri("/xl/worksheets/"),
                ExcelElementKind.Worksheet,
                ExcelElementKind.SheetData,
                ExcelElementKind.Row,
                ExcelElementKind.Cell,
                ExcelElementKind.CellValue);

        private ExcelFileKind(int id, string name, ExcelUri relativePath, params ExcelElementKind[] elements)
            : base(name, id)
        {
            RelativePath = relativePath;
            Elements = elements;
        }

        public ExcelUri RelativePath { get; }

        public IReadOnlyCollection<ExcelElementKind> Elements { get; }

        public static bool TryFromUri(Uri uri, out ExcelFileKind? result)
        {
            if (uri is null)
            {
                result = default;
                return false;
            }

            result = List
                .Where(fileKind => uri.OriginalString.StartsWith(
                    fileKind.RelativePath.OriginalString, StringComparison.OrdinalIgnoreCase))
                .SingleOrDefault();
            return result is not null;
        }
    }
}
