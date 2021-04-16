using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Reading.Failures
{
    /// <summary>
    /// Represents a defined property that is not found in the spreadsheet.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MissingResourcePropertyReadingFailure
    {
        internal MissingResourcePropertyReadingFailure(uint columnNumber)
        {
            ColumnNumber = columnNumber;
        }

        /// <summary>
        /// Gets the column number for the expected property.
        /// </summary>
        public uint ColumnNumber { get; }
    }
}
