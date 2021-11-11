using LanceC.SpreadsheetIO.Shared.Internal;

namespace LanceC.SpreadsheetIO.Reading.Internal.Readers;

/// <summary>
/// Defines the reader for worksheet elements.
/// </summary>
internal interface IWorksheetElementReader : IDisposable
{
    /// <summary>
    /// Reads to a specified row number.
    /// </summary>
    /// <param name="number">The row number to read to.</param>
    /// <returns><c>true</c> if the row was read to; otherwise, <c>false</c>.</returns>
    bool ReadToRow(uint number);

    /// <summary>
    /// Reads to the next row.
    /// </summary>
    /// <returns><c>true</c> if the row was read to; otherwise, <c>false</c>.</returns>
    bool ReadNextRow();

    /// <summary>
    /// Gets the number of the current row.
    /// </summary>
    /// <returns>The current row number.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the current element is not a row start element.</exception>
    uint GetRowNumber();

    /// <summary>
    /// Reads to the next cell.
    /// </summary>
    /// <returns><c>true</c> if the cell was read to; otherwise, <c>false</c>.</returns>
    bool ReadNextCell();

    /// <summary>
    /// Gets the location of the current cell.
    /// </summary>
    /// <returns>The current cell location.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the current element is not a cell start element.</exception>
    CellLocation GetCellLocation();

    /// <summary>
    /// Gets the value of the current cell.
    /// </summary>
    /// <returns>The current cell value.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the current element is not a cell start element.</exception>
    string GetCellValue();
}
