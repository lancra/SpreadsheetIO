using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Reading.Internal.Properties;

namespace LanceC.SpreadsheetIO.Reading.Internal.Readers;

/// <summary>
/// Defines the reader for a body row within a spreadsheet page using a defined resource map.
/// </summary>
internal interface IMappedBodyRowReader
{
    /// <summary>
    /// Reads a body row from a spreadsheet page.
    /// </summary>
    /// <typeparam name="TResource">The type of resource to read.</typeparam>
    /// <param name="reader">The worksheet element reader.</param>
    /// <param name="map">The resource map.</param>
    /// <param name="propertyHeaders">The headers for the resource properties.</param>
    /// <returns>The resource reading result.</returns>
    ResourceReadingResult<TResource> Read<TResource>(
        IWorksheetElementReader reader,
        ResourceMap<TResource> map,
        IResourcePropertyHeaders<TResource> propertyHeaders)
        where TResource : class;
}
