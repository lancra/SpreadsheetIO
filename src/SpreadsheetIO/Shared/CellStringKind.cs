using System.Diagnostics.CodeAnalysis;
using Ardalis.SmartEnum;
using DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Shared;

/// <summary>
/// Represents the kind of cell string.
/// </summary>
[ExcludeFromCodeCoverage]
public class CellStringKind : SmartEnum<CellStringKind>
{
    /// <summary>
    /// Specifies a string shared between cells in a spreadsheet.
    /// </summary>
    public static readonly CellStringKind SharedString = new(1, "Shared String", CellValues.SharedString);

    /// <summary>
    /// Specifies a string written directly to a cell.
    /// </summary>
    public static readonly CellStringKind InlineString = new(2, "Inline String", CellValues.InlineString);

    private CellStringKind(int id, string name, CellValues openXmlValue)
        : base(name, id)
    {
        OpenXmlValue = openXmlValue;
    }

    internal CellValues OpenXmlValue { get; }
}
