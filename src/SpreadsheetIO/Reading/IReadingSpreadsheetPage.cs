namespace LanceC.SpreadsheetIO.Reading;

/// <summary>
/// Defines a spreadsheet page to be read from.
/// </summary>
public interface IReadingSpreadsheetPage
{
    /// <summary>
    /// Reads all resources from the spreadsheet page.
    /// </summary>
    /// <typeparam name="TResource">The type of resource to read.</typeparam>
    /// <returns>The resulting resources and failures from reading from the spreadsheet page.</returns>
    ReadingResult<TResource> ReadAll<TResource>()
        where TResource : class;

    /// <summary>
    /// Starts a read operation on the spreadsheet page.
    /// </summary>
    /// <typeparam name="TResource">The type of resource to read.</typeparam>
    /// <returns>The read operation on the spreadsheet page.</returns>
    IReadingSpreadsheetPageOperation<TResource> StartRead<TResource>()
        where TResource : class;
}
