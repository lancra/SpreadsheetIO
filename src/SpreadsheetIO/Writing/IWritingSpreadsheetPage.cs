namespace LanceC.SpreadsheetIO.Writing;

/// <summary>
/// Defines a spreadsheet page to be written to.
/// </summary>
public interface IWritingSpreadsheetPage : IDisposable
{
    /// <summary>
    /// Gets the page name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the value that determines whether the spreadsheet page can be written to.
    /// </summary>
    bool CanWrite { get; }

    /// <summary>
    /// Gets the current row number being written.
    /// </summary>
    uint CurrentRowNumber { get; }

    /// <summary>
    /// Gets the current column number being written.
    /// </summary>
    uint CurrentColumnNumber { get; }

    /// <summary>
    /// Adds a cell.
    /// </summary>
    /// <param name="cell">The cell to add.</param>
    /// <returns>The modified spreadsheet page.</returns>
    IWritingSpreadsheetPage AddCell(WritingCell cell);

    /// <summary>
    /// Adds a cell.
    /// </summary>
    /// <param name="value">The cell value.</param>
    /// <returns>The modified spreadsheet page.</returns>
    IWritingSpreadsheetPage AddCell(string value);

    /// <summary>
    /// Adds a cell with a style.
    /// </summary>
    /// <param name="value">The cell value.</param>
    /// <param name="styleName">The style name.</param>
    /// <returns>The modified spreadsheet page.</returns>
    IWritingSpreadsheetPage AddCell(string value, string styleName);

    /// <summary>
    /// Advances writing to the next row.
    /// </summary>
    /// <returns>The modified spreadsheet page.</returns>
    IWritingSpreadsheetPage AdvanceRow();

    /// <summary>
    /// Advances writing by a number of rows.
    /// </summary>
    /// <param name="count">The number of rows to advance past.</param>
    /// <returns>The modified spreadsheet page.</returns>
    IWritingSpreadsheetPage AdvanceRows(uint count);

    /// <summary>
    /// Advances writing to a specified row.
    /// </summary>
    /// <param name="rowNumber">The row number to advance to.</param>
    /// <returns>The modified spreadsheet page.</returns>
    IWritingSpreadsheetPage AdvanceToRow(uint rowNumber);

    /// <summary>
    /// Advances writing to the next column.
    /// </summary>
    /// <returns>The modified spreadsheet page.</returns>
    IWritingSpreadsheetPage AdvanceColumn();

    /// <summary>
    /// Advances writing by a number of columns.
    /// </summary>
    /// <param name="count">The number of columns to advance past.</param>
    /// <returns>The modified spreadsheet page.</returns>
    IWritingSpreadsheetPage AdvanceColumns(uint count);

    /// <summary>
    /// Advances writing to a specific column.
    /// </summary>
    /// <param name="columnNumber">The column number to advance to.</param>
    /// <returns>The modified spreadsheet page.</returns>
    IWritingSpreadsheetPage AdvanceToColumn(uint columnNumber);

    /// <summary>
    /// Advances writing to a specific column.
    /// </summary>
    /// <param name="columnLetter">The column letter to advance to.</param>
    /// <returns>The modified spreadsheet page.</returns>
    IWritingSpreadsheetPage AdvanceToColumn(string columnLetter);

    /// <summary>
    /// Finishes writing to the spreadsheet page.
    /// </summary>
    /// <returns>The modified spreadsheet page.</returns>
    IWritingSpreadsheetPage Finish();
}
