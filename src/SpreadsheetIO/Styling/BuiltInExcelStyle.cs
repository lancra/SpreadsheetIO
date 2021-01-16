using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using LanceC.SpreadsheetIO.Shared;
using LanceC.SpreadsheetIO.Styling.Internal.Indexers;

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
                new Style(Border.Default, Fill.Default, Font.Default, 0));

        /// <summary>
        /// Specifies the style which denotes bad data.
        /// </summary>
        public static readonly BuiltInExcelStyle Bad =
            new BuiltInExcelStyle(
                2,
                "Bad",
                new Style(
                    Border.Default,
                    new Fill(FillKind.Solid, CreateColor(0xFFFFC7CE)),
                    Font.Default with { Color = CreateColor(0xFF9C0006), },
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
                    new Fill(FillKind.Solid, CreateColor(0xFFC6EFCE)),
                    Font.Default with { Color = CreateColor(0xFF006100), },
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
                    new Fill(FillKind.Solid, CreateColor(0xFFFFEB9C)),
                    Font.Default with { Color = CreateColor(0xFF9C5700), },
                    28));

        /// <summary>
        /// Specifies the style which denotes calculated data.
        /// </summary>
        public static readonly BuiltInExcelStyle Calculation =
            new BuiltInExcelStyle(
                5,
                "Calculation",
                new Style(
                    new Border(new BorderLine(CreateColor(0xFF7F7F7F), BorderLineKind.Thin)),
                    new Fill(FillKind.Solid, CreateColor(0xFFF2F2F2)),
                    Font.Default with { Color = CreateColor(0xFFFA7D00), IsBold = true, },
                    22));

        /// <summary>
        /// Specifies the style which denotes data the should be checked.
        /// </summary>
        public static readonly BuiltInExcelStyle CheckCell =
            new BuiltInExcelStyle(
                6,
                "Check Cell",
                new Style(
                    new Border(new BorderLine(CreateColor(0xFF3F3F3F), BorderLineKind.Double)),
                    new Fill(FillKind.Solid, CreateColor(0xFFA5A5A5)),
                    Font.Default with { Color = Color.White, IsBold = true, },
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
                    Font.Default with { Color = CreateColor(0xFF7F7F7F), IsItalic = true, },
                    53));

        /// <summary>
        /// Specifies the style which denotes an input field.
        /// </summary>
        public static readonly BuiltInExcelStyle Input =
            new BuiltInExcelStyle(
                8,
                "Input",
                new Style(
                    new Border(new BorderLine(CreateColor(0xFF7F7F7F), BorderLineKind.Thin)),
                    new Fill(FillKind.Solid, CreateColor(0xFFFFCC99)),
                    Font.Default with { Color = CreateColor(0xFF3F3F76), },
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
                        new BorderLine(CreateColor(0xFFFF8001), BorderLineKind.Double)),
                    Fill.Default,
                    Font.Default with { Color = CreateColor(0xFFFA7D00), },
                    24));

        /// <summary>
        /// Specifies the style which denotes note text.
        /// </summary>
        public static readonly BuiltInExcelStyle Note =
            new BuiltInExcelStyle(
                10,
                "Note",
                new Style(
                    new Border(new BorderLine(CreateColor(0xFFB2B2B2), BorderLineKind.Thin)),
                    new Fill(FillKind.Solid, CreateColor(0xFFFFFFCC)),
                    Font.Default,
                    10));

        /// <summary>
        /// Specifies the style which denotes an output field.
        /// </summary>
        public static readonly BuiltInExcelStyle Output =
            new BuiltInExcelStyle(
                11,
                "Output",
                new Style(
                    new Border(new BorderLine(CreateColor(0xFF3F3F3F), BorderLineKind.Thin)),
                    new Fill(FillKind.Solid, CreateColor(0xFFF2F2F2)),
                    Font.Default with { Color = CreateColor(0xFF3F3F3F), IsBold = true, },
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
                    Font.Default with { Color = CreateColor(0xFFFF0000), },
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

        private static Color CreateColor(uint argb)
            => Color.FromArgb((int)argb);
    }
}
