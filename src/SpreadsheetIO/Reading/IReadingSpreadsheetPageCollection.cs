namespace LanceC.SpreadsheetIO.Reading
{
    /// <summary>
    /// Defines a collection of spreadsheet pages to be read from.
    /// </summary>
    public interface IReadingSpreadsheetPageCollection : IReadOnlyList<IReadingSpreadsheetPage>
    {
        /// <summary>
        /// Gets a spreadsheet page by name.
        /// </summary>
        /// <param name="name">The name of the spreadsheet page to retrieve.</param>
        /// <returns>The matching spreadsheet page.</returns>
        IReadingSpreadsheetPage this[string name] { get; }
    }
}
