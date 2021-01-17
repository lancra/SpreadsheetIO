using System;

namespace LanceC.SpreadsheetIO.Shared.Internal.Wrappers
{
    /// <summary>
    /// Defines a wrapper for a spreadsheet.
    /// </summary>
    internal interface ISpreadsheetDocumentWrapper : IDisposable
    {
        /// <summary>
        /// Adds a worksheet part.
        /// </summary>
        /// <param name="name">The name of the sheet.</param>
        /// <returns>The added worksheet part wrapper.</returns>
        IWorksheetPartWrapper AddWorksheetPart(string name);
    }
}
