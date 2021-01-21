using System;

namespace LanceC.SpreadsheetIO.Shared.Internal.Wrappers
{
    /// <summary>
    /// Defines a wrapper for a spreadsheet.
    /// </summary>
    internal interface ISpreadsheetDocumentWrapper : IDisposable
    {
        /// <summary>
        /// Adds a shared string table part.
        /// </summary>
        /// <returns>The added shared string table part wrapper.</returns>
        ISharedStringTablePartWrapper AddSharedStringTablePart();

        /// <summary>
        /// Adds a worksheet part.
        /// </summary>
        /// <param name="name">The name of the sheet.</param>
        /// <returns>The added worksheet part wrapper.</returns>
        IWorksheetPartWrapper AddWorksheetPart(string name);

        /// <summary>
        /// Adds a workbook styles part.
        /// </summary>
        /// <returns>The added workbook styles part.</returns>
        IWorkbookStylesPartWrapper AddWorkbookStylesPart();
    }
}
