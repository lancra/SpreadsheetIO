using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Reading.Failures;

namespace LanceC.SpreadsheetIO.Reading
{
    /// <summary>
    /// Represents the result from reading a spreadsheet page.
    /// </summary>
    /// <typeparam name="TResource">The type of resource that was read.</typeparam>
    [ExcludeFromCodeCoverage]
    public record ReadingResult<TResource>(
        IReadOnlyCollection<NumberedResource<TResource>> Resources,
        HeaderReadingFailure? HeaderFailure,
        IReadOnlyCollection<ResourceReadingFailure> ResourceFailures)
        where TResource : class
    {
        /// <summary>
        /// Gets the reading result kind.
        /// </summary>
        public ReadingResultKind Kind { get; } = GetKind(Resources, HeaderFailure, ResourceFailures);

        /// <summary>
        /// Gets the collection of resources read from the spreadsheet.
        /// </summary>
        public IReadOnlyCollection<NumberedResource<TResource>> Resources { get; init; } = Resources;

        /// <summary>
        /// Gets the failure encountered when reading the header row.
        /// </summary>
        public HeaderReadingFailure? HeaderFailure { get; init; } = HeaderFailure;

        /// <summary>
        /// Gets the collection of failures encountered when reading the resources.
        /// </summary>
        public IReadOnlyCollection<ResourceReadingFailure> ResourceFailures { get; init; } = ResourceFailures;

        private static ReadingResultKind GetKind(
            IReadOnlyCollection<NumberedResource<TResource>> resources,
            HeaderReadingFailure? headerFailure,
            IReadOnlyCollection<ResourceReadingFailure> resourceFailures)
        {
            if (headerFailure is not null)
            {
                return ReadingResultKind.Failure;
            }
            else if (resourceFailures.Any())
            {
                return !resources.Any() ? ReadingResultKind.Failure : ReadingResultKind.PartialFailure;
            }

            return ReadingResultKind.Success;
        }
    }
}
