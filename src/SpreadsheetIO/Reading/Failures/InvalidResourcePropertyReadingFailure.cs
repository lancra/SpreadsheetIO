using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Reading.Failures;

/// <summary>
/// Represents a defined property that the spreadsheet does not match.
/// </summary>
[ExcludeFromCodeCoverage]
public record InvalidResourcePropertyReadingFailure(uint ColumnNumber, string Value)
{
    /// <summary>
    /// Gets the column number for the invalid property.
    /// </summary>
    public uint ColumnNumber { get; init; } = ColumnNumber;

    /// <summary>
    /// Gets the spreadsheet value present for the property.
    /// </summary>
    public string Value { get; init; } = Value;
}
