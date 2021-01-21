using System.IO;
using LanceC.SpreadsheetIO.Writing;

namespace LanceC.SpreadsheetIO
{
    /// <summary>
    /// Defines a factory for creating and opening spreadsheets.
    /// </summary>
    public interface ISpreadsheetFactory
    {
        /// <summary>
        /// Creates a spreadsheet.
        /// </summary>
        /// <param name="path">The file path of the spreadsheet to create.</param>
        /// <returns>The created spreadsheet.</returns>
        IWritingSpreadsheet Create(string path);

        /// <summary>
        /// Creates a spreadsheet.
        /// </summary>
        /// <param name="stream">The stream of the spreadsheet to create.</param>
        /// <returns>The created spreadsheet.</returns>
        IWritingSpreadsheet Create(Stream stream);
    }
}
