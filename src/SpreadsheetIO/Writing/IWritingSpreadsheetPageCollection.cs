using System;
using System.Collections.Generic;

namespace LanceC.SpreadsheetIO.Writing
{
    /// <summary>
    /// Defines a collection of spreadsheet pages to be written to.
    /// </summary>
    public interface IWritingSpreadsheetPageCollection : IReadOnlyList<IWritingSpreadsheetPage>, IDisposable
    {
        /// <summary>
        /// Gets a spreadsheet page by name.
        /// </summary>
        /// <param name="name">The name of the spreadsheet page to retrieve.</param>
        /// <returns>The matching spreadsheet page.</returns>
        IWritingSpreadsheetPage this[string name] { get; }
    }
}
