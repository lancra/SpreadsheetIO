using DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Styling.Internal.Generators
{
    /// <summary>
    /// Defines the generator used to create a stylesheet from specified styles.
    /// </summary>
    internal interface IStylesheetGenerator
    {
        /// <summary>
        /// Generates a stylesheet.
        /// </summary>
        /// <returns>The generated stylesheet.</returns>
        Stylesheet Generate();
    }
}
