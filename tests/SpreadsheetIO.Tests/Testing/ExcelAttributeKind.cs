using Ardalis.SmartEnum;

namespace LanceC.SpreadsheetIO.Tests.Testing;

public class ExcelAttributeKind : SmartEnum<ExcelAttributeKind>
{
    public static readonly ExcelAttributeKind ApplyAlignment = new(1, "Apply Alignment", "applyAlignment");
    public static readonly ExcelAttributeKind ApplyBorder = new(2, "Apply Border", "applyBorder");
    public static readonly ExcelAttributeKind ApplyFill = new(3, "Apply Fill", "applyFill");
    public static readonly ExcelAttributeKind ApplyFont = new(4, "Apply Font", "applyFont");
    public static readonly ExcelAttributeKind ApplyNumberFormat = new(5, "Apply Number Format", "applyNumberFormat");
    public static readonly ExcelAttributeKind ApplyProtection = new(6, "Apply Protection", "applyProtection");
    public static readonly ExcelAttributeKind Auto = new(7, "Auto", "auto");
    public static readonly ExcelAttributeKind BorderId = new(8, "Border ID", "borderId");
    public static readonly ExcelAttributeKind BuiltInId = new(9, "Built-In ID", "builtInId");
    public static readonly ExcelAttributeKind CellFormatId = new(10, "Cell Format ID", "xfId");
    public static readonly ExcelAttributeKind Count = new(11, "Count", "count");
    public static readonly ExcelAttributeKind ElementName = new(12, "Element Name", "name");
    public static readonly ExcelAttributeKind ElementValue = new(13, "Element Value", "val");
    public static readonly ExcelAttributeKind FillId = new(14, "Fill ID", "fillId");
    public static readonly ExcelAttributeKind FontId = new(15, "Font ID", "fontId");
    public static readonly ExcelAttributeKind FormatCode = new(16, "Format Code", "formatCode");
    public static readonly ExcelAttributeKind Horizontal = new(17, "Horizontal", "horizontal");
    public static readonly ExcelAttributeKind Indexed = new(18, "Indexed", "indexed");
    public static readonly ExcelAttributeKind JustifyLastLine = new(19, "Justify Last Line", "justifyLastLine");
    public static readonly ExcelAttributeKind NumberFormatId = new(20, "Number Format ID", "numFmtId");
    public static readonly ExcelAttributeKind PatternType = new(21, "Pattern Type", "patternType");
    public static readonly ExcelAttributeKind RedGreenBlue = new(22, "Red Green Blue", "rgb");
    public static readonly ExcelAttributeKind Row = new(23, "Row", "r");
    public static readonly ExcelAttributeKind SheetId = new(24, "Sheet ID", "sheetId");
    public static readonly ExcelAttributeKind Style = new(25, "Style", "style");
    public static readonly ExcelAttributeKind Theme = new(26, "Theme", "theme");
    public static readonly ExcelAttributeKind Tint = new(27, "Tint", "tint");
    public static readonly ExcelAttributeKind Type = new(28, "Type", "t");
    public static readonly ExcelAttributeKind Vertical = new(29, "Vertical", "vertical");

    private static readonly Lazy<Dictionary<string, ExcelAttributeKind>> _fromLocalName =
        new(() => List.ToDictionary(k => k.LocalName));

    private ExcelAttributeKind(int id, string name, string localName)
        : base(name, id)
    {
        LocalName = localName;
    }

    public string LocalName { get; }

    public static bool TryFromLocalName(string localName, out ExcelAttributeKind? result)
    {
        if (string.IsNullOrEmpty(localName))
        {
            result = default;
            return false;
        }

        return _fromLocalName.Value.TryGetValue(localName, out result);
    }
}
