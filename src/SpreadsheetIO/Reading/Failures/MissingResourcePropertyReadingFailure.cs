using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Reading.Failures;

/// <summary>
/// Represents a defined property that is not found in the spreadsheet.
/// </summary>
[ExcludeFromCodeCoverage]
public record MissingResourcePropertyReadingFailure(uint ColumnNumber)
{
    /// <summary>
    /// Gets the column number for the expected property.
    /// </summary>
    public uint ColumnNumber { get; init; } = ColumnNumber;
}
