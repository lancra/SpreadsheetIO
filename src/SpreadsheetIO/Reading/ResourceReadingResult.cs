using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Reading.Failures;

namespace LanceC.SpreadsheetIO.Reading
{
    /// <summary>
    /// Represents the result from reading a resource from a spreadsheet page.
    /// </summary>
    /// <typeparam name="TResource">The type of resource that was read.</typeparam>
    [ExcludeFromCodeCoverage]
    public record ResourceReadingResult<TResource>(
        NumberedResource<TResource>? NumberedResource,
        ResourceReadingFailure? Failure)
        where TResource : class
    {
        /// <summary>
        /// Gets the resource reading result kind.
        /// </summary>
        public ResourceReadingResultKind Kind { get; } = Failure is null
            ? ResourceReadingResultKind.Success
            : ResourceReadingResultKind.Failure;

        /// <summary>
        /// Gets the resource read from the spreadsheet or <c>null</c> if a failure was encountered.
        /// </summary>
        public NumberedResource<TResource>? NumberedResource { get; init; } = NumberedResource;

        /// <summary>
        /// Gets the failure encountered when reading the resource or <c>null</c> when successful.
        /// </summary>
        public ResourceReadingFailure? Failure { get; init; } = Failure;
    }
}
