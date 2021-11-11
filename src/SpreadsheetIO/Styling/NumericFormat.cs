using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Styling;

/// <summary>
/// Represents a numeric format.
/// </summary>
[ExcludeFromCodeCoverage]
public record NumericFormat(string Code)
{
    /// <summary>
    /// Gets the default numeric format.
    /// </summary>
    public static readonly NumericFormat Default = new("General");

    /// <summary>
    /// Gets the numeric format code.
    /// </summary>
    public string Code { get; init; } = Code;
}
