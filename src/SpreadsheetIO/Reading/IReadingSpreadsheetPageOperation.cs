using System;
using LanceC.SpreadsheetIO.Reading.Failures;

namespace LanceC.SpreadsheetIO.Reading
{
    /// <summary>
    /// Defines a reading operation initiated on a spreadsheet page.
    /// </summary>
    /// <typeparam name="TResource">The type of resource to read.</typeparam>
    public interface IReadingSpreadsheetPageOperation<TResource> : IDisposable
        where TResource : class
    {
        /// <summary>
        /// Gets the failure encountered when reading the header row.
        /// </summary>
        HeaderReadingFailure? HeaderFailure { get; }

        /// <summary>
        /// Gets the current resource reading result or <c>null</c> if the header was not read successfully or if all resources have
        /// been read.
        /// </summary>
        ResourceReadingResult<TResource>? CurrentResult { get; }

        /// <summary>
        /// Reads the next resource.
        /// </summary>
        /// <returns><c>true</c> when a resource is available to read; otherwise, <c>false</c>.</returns>
        bool ReadNext();
    }
}
