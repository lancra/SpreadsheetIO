using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Reading.Failures
{
    /// <summary>
    /// Represents a resource mismatch between the defined map and the spreadsheet.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record ResourceReadingFailure(
        uint RowNumber,
        IReadOnlyCollection<MissingResourcePropertyReadingFailure> MissingProperties,
        IReadOnlyCollection<InvalidResourcePropertyReadingFailure> InvalidProperties)
    {
        /// <summary>
        /// Gets the row number in the spreadsheet.
        /// </summary>
        public uint RowNumber { get; init; } = RowNumber;

        /// <summary>
        /// Gets the collection of properties that are not found.
        /// </summary>
        public IReadOnlyCollection<MissingResourcePropertyReadingFailure> MissingProperties { get; init; } = MissingProperties;

        /// <summary>
        /// Gets the collection of properties that do not match the map.
        /// </summary>
        public IReadOnlyCollection<InvalidResourcePropertyReadingFailure> InvalidProperties { get; init; } = InvalidProperties;
    }
}
