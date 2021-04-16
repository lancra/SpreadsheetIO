using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Reading.Failures;

namespace LanceC.SpreadsheetIO.Reading
{
    /// <summary>
    /// Represents the result from reading a spreadsheet page.
    /// </summary>
    /// <typeparam name="TResource">The type of resource that was read.</typeparam>
    [ExcludeFromCodeCoverage]
    public class ReadingResult<TResource>
        where TResource : class
    {
        internal ReadingResult(
            IReadOnlyCollection<TResource> resources,
            HeaderReadingFailure? headerFailure,
            IReadOnlyCollection<ResourceReadingFailure> resourceFailures)
        {
            Resources = resources;
            HeaderFailure = headerFailure;
            ResourceFailures = resourceFailures;
        }

        /// <summary>
        /// Gets the collection of resources read from the spreadsheet.
        /// </summary>
        public IReadOnlyCollection<TResource> Resources { get; }

        /// <summary>
        /// Gets the failure encountered when reading the header row.
        /// </summary>
        public HeaderReadingFailure? HeaderFailure { get; }

        /// <summary>
        /// Gets the collection of failures encountered when reading the resources.
        /// </summary>
        public IReadOnlyCollection<ResourceReadingFailure> ResourceFailures { get; }
    }
}
