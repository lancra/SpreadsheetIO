using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Reading.Failures
{
    /// <summary>
    /// Represents a defined header that is not found in the spreadsheet.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MissingHeaderReadingFailure
    {
        internal MissingHeaderReadingFailure(PropertyMapKey mapKey)
        {
            MapKey = mapKey;
        }

        /// <summary>
        /// Gets the property map key that is missing.
        /// </summary>
        public PropertyMapKey MapKey { get; }
    }
}
