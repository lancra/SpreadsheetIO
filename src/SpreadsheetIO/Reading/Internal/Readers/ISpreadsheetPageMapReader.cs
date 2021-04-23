using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Reading.Internal.Properties;

namespace LanceC.SpreadsheetIO.Reading.Internal.Readers
{
    /// <summary>
    /// Defines the reader for a spreadsheet page which uses a defined resource map.
    /// </summary>
    internal interface ISpreadsheetPageMapReader
    {
        /// <summary>
        /// Reads the header row from a spreadsheet page.
        /// </summary>
        /// <typeparam name="TResource">The type of resource to read.</typeparam>
        /// <param name="reader">The worksheet element reader.</param>
        /// <param name="map">The resource map.</param>
        /// <returns>The header row reading result.</returns>
        HeaderRowReadingResult<TResource> ReadHeaderRow<TResource>(IWorksheetElementReader reader, ResourceMap<TResource> map)
            where TResource : class;

        /// <summary>
        /// Reads a body row from a spreadsheet page.
        /// </summary>
        /// <typeparam name="TResource">The type of resource to read.</typeparam>
        /// <param name="reader">The worksheet element reader.</param>
        /// <param name="map">The resource map.</param>
        /// <param name="propertyHeaders">The headers for the resource properties.</param>
        /// <returns>The body row reading result.</returns>
        BodyRowReadingResult<TResource> ReadBodyRow<TResource>(
            IWorksheetElementReader reader,
            ResourceMap<TResource> map,
            IResourcePropertyHeaders<TResource> propertyHeaders)
            where TResource : class;
    }
}
