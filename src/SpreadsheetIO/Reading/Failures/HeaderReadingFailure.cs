using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Reading.Failures
{
    /// <summary>
    /// Represents a header mismatch between the defined map and the spreadsheet.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class HeaderReadingFailure
    {
        internal HeaderReadingFailure(
            bool missingHeaderRow,
            IReadOnlyCollection<MissingHeaderReadingFailure> missingHeaders,
            IReadOnlyCollection<InvalidHeaderReadingFailure> invalidHeaders)
        {
            MissingHeaderRow = missingHeaderRow;
            MissingHeaders = missingHeaders;
            InvalidHeaders = invalidHeaders;
        }

        /// <summary>
        /// Gets the value that determines whether the specified header row is not found.
        /// </summary>
        public bool MissingHeaderRow { get; }

        /// <summary>
        /// Gets the collection of headers that are not found.
        /// </summary>
        public IReadOnlyCollection<MissingHeaderReadingFailure> MissingHeaders { get; }

        /// <summary>
        /// Gets the collection of headers that do not match the map.
        /// </summary>
        public IReadOnlyCollection<InvalidHeaderReadingFailure> InvalidHeaders { get; }
    }
}
