namespace LanceC.SpreadsheetIO.Writing.Internal;

/// <summary>
/// Defines a collection of spreadsheet pages to be written that can have new elements added.
/// </summary>
internal interface IWritingSpreadsheetPageCollectionModifiable : IWritingSpreadsheetPageCollection
{
    /// <summary>
    /// Adds a spreadsheet page to the collection.
    /// </summary>
    /// <param name="spreadsheetPage">The spreadsheet page to add.</param>
    void Add(IWritingSpreadsheetPage spreadsheetPage);
}
