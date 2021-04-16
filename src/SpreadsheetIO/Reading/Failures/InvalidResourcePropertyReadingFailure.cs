using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Reading.Failures
{
    /// <summary>
    /// Represents a defined property that the spreadsheet does not match.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class InvalidResourcePropertyReadingFailure
    {
        internal InvalidResourcePropertyReadingFailure(uint columnNumber, string value)
        {
            ColumnNumber = columnNumber;
            Value = value;
        }

        /// <summary>
        /// Gets the column number for the invalid property.
        /// </summary>
        public uint ColumnNumber { get; }

        /// <summary>
        /// Gets the spreadsheet value present for the property.
        /// </summary>
        public string Value { get; }
    }
}
