using System.Diagnostics.CodeAnalysis;
using Ardalis.SmartEnum;
using DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Styling;

/// <summary>
/// Represents the kind of fill.
/// </summary>
[ExcludeFromCodeCoverage]
public class FillKind : SmartEnum<FillKind>
{
    /// <summary>
    /// Specifies a blank fill.
    /// </summary>
    public static readonly FillKind None = new(0, "None", PatternValues.None);

    /// <summary>
    /// Specifies a solid fill.
    /// </summary>
    public static readonly FillKind Solid = new(1, "Solid", PatternValues.Solid);

    internal static readonly FillKind Gray125 = new(-1, "Gray125", PatternValues.Gray125);

    private FillKind(int id, string name, PatternValues openXmlValue)
        : base(name, id)
    {
        OpenXmlValue = openXmlValue;
    }

    internal PatternValues OpenXmlValue { get; }
}
