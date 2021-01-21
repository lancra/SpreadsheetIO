using DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Shared.Internal.Wrappers
{
    /// <summary>
    /// Defines a wrapper for a workbook styles part.
    /// </summary>
    internal interface IWorkbookStylesPartWrapper
    {
        /// <summary>
        /// Sets the internal stylesheet.
        /// </summary>
        /// <param name="stylesheet">The stylesheet.</param>
        void SetStylesheet(Stylesheet stylesheet);
    }
}
