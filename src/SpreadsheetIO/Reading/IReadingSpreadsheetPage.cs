using System;
using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Reading
{
    /// <summary>
    /// Defines a spreadsheet page to be read from.
    /// </summary>
    public interface IReadingSpreadsheetPage
    {
        /// <summary>
        /// Reads the spreadsheet page as a collection of resources.
        /// </summary>
        /// <typeparam name="TResource">The type of resource to read.</typeparam>
        /// <returns>The resulting resources and failures from reading from the spreadsheet page.</returns>
        ReadingResult<TResource> Read<TResource>()
            where TResource : class;

        /// <summary>
        /// Reads the spreadsheet page as a collection of resources.
        /// </summary>
        /// <typeparam name="TResource">The type of resource to read.</typeparam>
        /// <typeparam name="TResourceMap">The resource map.</typeparam>
        /// <returns>The resulting resources and failures from reading from the spreadsheet page.</returns>
        ReadingResult<TResource> Read<TResource, TResourceMap>()
            where TResource : class
            where TResourceMap : ResourceMap<TResource>;
    }
}
