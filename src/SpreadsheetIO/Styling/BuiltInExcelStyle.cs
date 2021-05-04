using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using LanceC.SpreadsheetIO.Shared;
using LanceC.SpreadsheetIO.Shared.Internal;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;

namespace LanceC.SpreadsheetIO.Styling
{
    /// <summary>
    /// Represents a style that is built-in to Excel.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class BuiltInExcelStyle : Enumeration
    {
        /// <summary>
        /// Specifies the normal style.
        /// </summary>
        public static readonly BuiltInExcelStyle Normal =
            new BuiltInExcelStyle(
                1,
                "Normal",
                new Style(Border.Default, Fill.Default, Font.Default, NumericFormat.Default, 0));

        /// <summary>
        /// Specifies the style which denotes bad data.
        /// </summary>
        public static readonly BuiltInExcelStyle Bad =
            new BuiltInExcelStyle(
                2,
                "Bad",
                new Style(
                    Border.Default,
                    new Fill(FillKind.Solid, 0xFFFFC7CE.ToColor()),
                    Font.Default with { Color = 0xFF9C0006.ToColor(), },
                    NumericFormat.Default,
                    27));

        /// <summary>
        /// Specifies the style which denotes good data.
        /// </summary>
        public static readonly BuiltInExcelStyle Good =
            new BuiltInExcelStyle(
                3,
                "Good",
                new Style(
                    Border.Default,
                    new Fill(FillKind.Solid, 0xFFC6EFCE.ToColor()),
                    Font.Default with { Color = 0xFF006100.ToColor(), },
                    NumericFormat.Default,
                    26));

        /// <summary>
        /// Specifies the style which denotes neutral data.
        /// </summary>
        public static readonly BuiltInExcelStyle Neutral =
            new BuiltInExcelStyle(
                4,
                "Neutral",
                new Style(
                    Border.Default,
                    new Fill(FillKind.Solid, 0xFFFFEB9C.ToColor()),
                    Font.Default with { Color = 0xFF9C5700.ToColor(), },
                    NumericFormat.Default,
                    28));

        /// <summary>
        /// Specifies the style which denotes calculated data.
        /// </summary>
        public static readonly BuiltInExcelStyle Calculation =
            new BuiltInExcelStyle(
                5,
                "Calculation",
                new Style(
                    new Border(new BorderLine(0xFF7F7F7F.ToColor(), BorderLineKind.Thin)),
                    new Fill(FillKind.Solid, 0xFFF2F2F2.ToColor()),
                    Font.Default with { Color = 0xFFFA7D00.ToColor(), IsBold = true, },
                    NumericFormat.Default,
                    22));

        /// <summary>
        /// Specifies the style which denotes data the should be checked.
        /// </summary>
        public static readonly BuiltInExcelStyle CheckCell =
            new BuiltInExcelStyle(
                6,
                "Check Cell",
                new Style(
                    new Border(new BorderLine(0xFF3F3F3F.ToColor(), BorderLineKind.Double)),
                    new Fill(FillKind.Solid, 0xFFA5A5A5.ToColor()),
                    Font.Default with { Color = Color.White, IsBold = true, },
                    NumericFormat.Default,
                    23));

        /// <summary>
        /// Specifies the style which denotes explanatory text.
        /// </summary>i
        public static readonly BuiltInExcelStyle ExplanatoryText =
            new BuiltInExcelStyle(
                7,
                "Explanatory Text",
                new Style(
                    Border.Default,
                    Fill.Default,
                    Font.Default with { Color = 0xFF7F7F7F.ToColor(), IsItalic = true, },
                    NumericFormat.Default,
                    53));

        /// <summary>
        /// Specifies the style which denotes an input field.
        /// </summary>
        public static readonly BuiltInExcelStyle Input =
            new BuiltInExcelStyle(
                8,
                "Input",
                new Style(
                    new Border(new BorderLine(0xFF7F7F7F.ToColor(), BorderLineKind.Thin)),
                    new Fill(FillKind.Solid, 0xFFFFCC99.ToColor()),
                    Font.Default with { Color = 0xFF3F3F76.ToColor(), },
                    NumericFormat.Default,
                    20));

        /// <summary>
        /// Specifies the style which denotes a linked cell.
        /// </summary>
        public static readonly BuiltInExcelStyle LinkedCell =
            new BuiltInExcelStyle(
                9,
                "Linked Cell",
                new Style(
                    new Border(
                        BorderLine.Default,
                        BorderLine.Default,
                        BorderLine.Default,
                        new BorderLine(0xFFFF8001.ToColor(), BorderLineKind.Double)),
                    Fill.Default,
                    Font.Default with { Color = 0xFFFA7D00.ToColor(), },
                    NumericFormat.Default,
                    24));

        /// <summary>
        /// Specifies the style which denotes note text.
        /// </summary>
        public static readonly BuiltInExcelStyle Note =
            new BuiltInExcelStyle(
                10,
                "Note",
                new Style(
                    new Border(new BorderLine(0xFFB2B2B2.ToColor(), BorderLineKind.Thin)),
                    new Fill(FillKind.Solid, 0xFFFFFFCC.ToColor()),
                    Font.Default,
                    NumericFormat.Default,
                    10));

        /// <summary>
        /// Specifies the style which denotes an output field.
        /// </summary>
        public static readonly BuiltInExcelStyle Output =
            new BuiltInExcelStyle(
                11,
                "Output",
                new Style(
                    new Border(new BorderLine(0xFF3F3F3F.ToColor(), BorderLineKind.Thin)),
                    new Fill(FillKind.Solid, 0xFFF2F2F2.ToColor()),
                    Font.Default with { Color = 0xFF3F3F3F.ToColor(), IsBold = true, },
                    NumericFormat.Default,
                    21));

        /// <summary>
        /// Specifies the style which denotes warning text.
        /// </summary>
        public static readonly BuiltInExcelStyle WarningText =
            new BuiltInExcelStyle(
                12,
                "Warning Text",
                new Style(
                    Border.Default,
                    Fill.Default,
                    Font.Default with { Color = 0xFFFF0000.ToColor(), },
                    NumericFormat.Default,
                    11));

        private BuiltInExcelStyle(int id, string name, Style style)
            : base(id, name)
        {
            Style = style;
        }

        /// <summary>
        /// Gets the style.
        /// </summary>
        public Style Style { get; }

        internal IndexerKey IndexerKey
            => new IndexerKey(Name, IndexerKeyKind.Excel);
    }
}
