using LanceC.SpreadsheetIO.Mapping2;

namespace LanceC.SpreadsheetIO.Reading.Internal.Readers;

/// <summary>
/// Defines the reader for a header row within a spreadsheet page using a defined resource map.
/// </summary>
internal interface IMappedHeaderRowReader
{
    /// <summary>
    /// Reads the header row from a spreadsheet page.
    /// </summary>
    /// <typeparam name="TResource">The type of resource to read.</typeparam>
    /// <param name="reader">The worksheet element reader.</param>
    /// <param name="map">The resource map.</param>
    /// <returns>The header row reading result.</returns>
    HeaderRowReadingResult<TResource> Read<TResource>(IWorksheetElementReader reader, ResourceMap map)
        where TResource : class;
}
