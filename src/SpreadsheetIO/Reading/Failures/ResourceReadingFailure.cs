using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Reading.Failures
{
    /// <summary>
    /// Represents a resource mismatch between the defined map and the spreadsheet.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ResourceReadingFailure
    {
        internal ResourceReadingFailure(
            uint rowNumber,
            IReadOnlyCollection<MissingResourcePropertyReadingFailure> missingProperties,
            IReadOnlyCollection<InvalidResourcePropertyReadingFailure> invalidProperties)
        {
            RowNumber = rowNumber;
            MissingProperties = missingProperties;
            InvalidProperties = invalidProperties;
        }

        /// <summary>
        /// Gets the row number in the spreadsheet.
        /// </summary>
        public uint RowNumber { get; }

        /// <summary>
        /// Gets the collection of properties that are not found.
        /// </summary>
        public IReadOnlyCollection<MissingResourcePropertyReadingFailure> MissingProperties { get; }

        /// <summary>
        /// Gets the collection of properties that do not match the map.
        /// </summary>
        public IReadOnlyCollection<InvalidResourcePropertyReadingFailure> InvalidProperties { get; }
    }
}
