using System.IO;

namespace LanceC.SpreadsheetIO.Shared.Internal.Wrappers
{
    /// <summary>
    /// Defines a factory for generating spreadsheet wrappers.
    /// </summary>
    internal interface ISpreadsheetDocumentWrapperFactory
    {
        /// <summary>
        /// Creates a spreadsheet wrapper.
        /// </summary>
        /// <param name="path">The file path of the spreadsheet to create.</param>
        /// <returns>The created spreadsheet wrapper.</returns>
        ISpreadsheetDocumentWrapper Create(string path);

        /// <summary>
        /// Creates a spreadsheet wrapper.
        /// </summary>
        /// <param name="stream">The stream of spreadsheet to create.</param>
        /// <returns>The created spreadsheet wrapper.</returns>
        ISpreadsheetDocumentWrapper Create(Stream stream);
    }
}
