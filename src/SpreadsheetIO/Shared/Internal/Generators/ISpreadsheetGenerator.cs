using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;

namespace LanceC.SpreadsheetIO.Shared.Internal.Generators;

/// <summary>
/// Defines a generator for a part of a spreadsheet.
/// </summary>
internal interface ISpreadsheetGenerator
{
    /// <summary>
    /// Generates the part of the spreadsheet.
    /// </summary>
    /// <param name="spreadsheetDocument">The spreadsheet document to modify.</param>
    void Generate(ISpreadsheetDocumentWrapper spreadsheetDocument);
}
