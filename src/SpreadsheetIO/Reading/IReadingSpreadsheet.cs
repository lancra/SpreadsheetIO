namespace LanceC.SpreadsheetIO.Reading;

/// <summary>
/// Defines a spreadsheet to be read from.
/// </summary>
public interface IReadingSpreadsheet
{
    /// <summary>
    /// Gets the spreadsheet pages.
    /// </summary>
    IReadingSpreadsheetPageCollection Pages { get; }
}
