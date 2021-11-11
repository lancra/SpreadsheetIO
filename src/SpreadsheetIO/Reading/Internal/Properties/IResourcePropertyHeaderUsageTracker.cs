namespace LanceC.SpreadsheetIO.Reading.Internal.Properties;

/// <summary>
/// Defines the tracker for determining the usage of headers within a body row.
/// </summary>
internal interface IResourcePropertyHeaderUsageTracker
{
    /// <summary>
    /// Gets all column numbers that have not been marked as used.
    /// </summary>
    /// <returns>The collection of column numbers that have not been marked as used.</returns>
    IReadOnlyCollection<uint> GetUnusedColumnNumbers();

    /// <summary>
    /// Marks a column as used.
    /// </summary>
    /// <param name="columnNumber">The column number.</param>
    void MarkAsUsed(uint columnNumber);
}
