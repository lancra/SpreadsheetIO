using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Reading
{
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
        /// Reads all resources from the spreadsheet page.
        /// </summary>
        /// <typeparam name="TResource">The type of resource to read.</typeparam>
        /// <typeparam name="TResourceMap">The type of resource map to use for reading.</typeparam>
        /// <returns>The resulting resources and failures from reading from the spreadsheet page.</returns>
        ReadingResult<TResource> ReadAll<TResource, TResourceMap>()
            where TResource : class
            where TResourceMap : ResourceMap<TResource>;
    }
}
