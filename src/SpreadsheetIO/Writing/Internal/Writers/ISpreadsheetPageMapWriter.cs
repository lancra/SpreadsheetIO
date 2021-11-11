using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Writing.Internal.Writers;

/// <summary>
/// Defines the writer for a spreadsheet page which uses a defined resource map.
/// </summary>
internal interface ISpreadsheetPageMapWriter
{
    /// <summary>
    /// Writes the resources to a spreadsheet page.
    /// </summary>
    /// <typeparam name="TResource">The type of resource to write.</typeparam>
    /// <param name="spreadsheetPage">The spreadsheet page to write to.</param>
    /// <param name="resources">The resource to write.</param>
    /// <param name="map">The resource map.</param>
    void Write<TResource>(IWritingSpreadsheetPage spreadsheetPage, IEnumerable<TResource> resources, ResourceMap<TResource> map)
        where TResource : class;
}
