using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Reading.Failures;

/// <summary>
/// Represents a header mismatch between the defined map and the spreadsheet.
/// </summary>
[ExcludeFromCodeCoverage]
public record HeaderReadingFailure(
    bool MissingHeaderRow,
    IReadOnlyCollection<MissingHeaderReadingFailure> MissingHeaders,
    IReadOnlyCollection<InvalidHeaderReadingFailure> InvalidHeaders)
{
    /// <summary>
    /// Gets a value indicating whether the specified header row is not found.
    /// </summary>
    public bool MissingHeaderRow { get; init; } = MissingHeaderRow;

    /// <summary>
    /// Gets the collection of headers that are not found.
    /// </summary>
    public IReadOnlyCollection<MissingHeaderReadingFailure> MissingHeaders { get; init; } = MissingHeaders;

    /// <summary>
    /// Gets the collection of headers that do not match the map.
    /// </summary>
    public IReadOnlyCollection<InvalidHeaderReadingFailure> InvalidHeaders { get; init; } = InvalidHeaders;
}
