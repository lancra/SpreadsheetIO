using LanceC.SpreadsheetIO.Reading;
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
        IWritingSpreadsheet Create(Uri path);

        /// <summary>
        /// Creates a spreadsheet.
        /// </summary>
        /// <param name="stream">The stream of the spreadsheet to create.</param>
        /// <returns>The created spreadsheet.</returns>
        IWritingSpreadsheet Create(Stream stream);

        /// <summary>
        /// Opens a spreadsheet for reading.
        /// </summary>
        /// <param name="path">The file path of the spreadsheet to open.</param>
        /// <returns>The opened spreadsheet.</returns>
        IReadingSpreadsheet OpenRead(Uri path);

        /// <summary>
        /// Opens a spreadsheet for reading.
        /// </summary>
        /// <param name="stream">The stream of the spreadsheet to open.</param>
        /// <returns>The opened spreadsheet.</returns>
        IReadingSpreadsheet OpenRead(Stream stream);
    }
}
