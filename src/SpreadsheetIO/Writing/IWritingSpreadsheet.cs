using System;
using LanceC.SpreadsheetIO.Styling;

namespace LanceC.SpreadsheetIO.Writing
{
    /// <summary>
    /// Defines a spreadsheet to be written to.
    /// </summary>
    public interface IWritingSpreadsheet : IDisposable
    {
        /// <summary>
        /// Adds a new spreadsheet page.
        /// </summary>
        /// <param name="name">The name of the page.</param>
        /// <returns>The modified spreadsheet.</returns>
        IWritingSpreadsheetPage AddPage(string name);

        /// <summary>
        /// Adds a style to the spreadsheet.
        /// </summary>
        /// <param name="name">The unique name of the style.</param>
        /// <param name="style">The style.</param>
        /// <returns>The modified spreadsheet.</returns>
        IWritingSpreadsheet AddStyle(string name, Style style);

        /// <summary>
        /// Adds a style to the spreadsheet that is defined by Excel.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <returns>The modified spreadsheet.</returns>
        IWritingSpreadsheet AddStyle(BuiltInExcelStyle style);

        /// <summary>
        /// Adds a style to the spreadsheet that is defined by this package.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <returns>The modified spreadsheet.</returns>
        IWritingSpreadsheet AddStyle(BuiltInPackageStyle style);
    }
}
