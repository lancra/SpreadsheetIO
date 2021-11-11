using System.Diagnostics.CodeAnalysis;
using Ardalis.SmartEnum;
using DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Styling;

/// <summary>
/// Represents the kind of border line.
/// </summary>
[ExcludeFromCodeCoverage]
public class BorderLineKind : SmartEnum<BorderLineKind>
{
    /// <summary>
    /// Specifies a blank border line.
    /// </summary>
    public static readonly BorderLineKind None = new(0, "None", BorderStyleValues.None);

    /// <summary>
    /// Specifies a thin border line.
    /// </summary>
    public static readonly BorderLineKind Thin = new(1, "Thin", BorderStyleValues.Thin);

    /// <summary>
    /// Specifies a thick border line.
    /// </summary>
    public static readonly BorderLineKind Thick = new(2, "Thick", BorderStyleValues.Thick);

    /// <summary>
    /// Specifies a dashed border line.
    /// </summary>
    public static readonly BorderLineKind Dashed = new(3, "Dashed", BorderStyleValues.Dashed);

    /// <summary>
    /// Specifies a dotted border line.
    /// </summary>
    public static readonly BorderLineKind Dotted = new(4, "Dotted", BorderStyleValues.Dotted);

    /// <summary>
    /// Specifies a double border line.
    /// </summary>
    public static readonly BorderLineKind Double = new(5, "Double", BorderStyleValues.Double);

    private BorderLineKind(int id, string name, BorderStyleValues openXmlValue)
        : base(name, id)
    {
        OpenXmlValue = openXmlValue;
    }

    internal BorderStyleValues OpenXmlValue { get; }
}
