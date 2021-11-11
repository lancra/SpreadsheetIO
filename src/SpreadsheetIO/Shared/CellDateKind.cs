using System.Diagnostics.CodeAnalysis;
using Ardalis.SmartEnum;

namespace LanceC.SpreadsheetIO.Shared;

/// <summary>
/// Represents the kind of cell date.
/// </summary>
[ExcludeFromCodeCoverage]
public class CellDateKind : SmartEnum<CellDateKind>
{
    /// <summary>
    /// Specifies a date written as a number with a format required for readability.
    /// </summary>
    public static readonly CellDateKind Number = new(1, "Number");

    /// <summary>
    /// Specifies a date written as text where formatting cannot be modified.
    /// </summary>
    public static readonly CellDateKind Text = new(2, "Text");

    private CellDateKind(int id, string name)
        : base(name, id)
    {
    }
}
