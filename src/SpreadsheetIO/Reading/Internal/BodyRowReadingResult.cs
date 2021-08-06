using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Reading.Failures;

namespace LanceC.SpreadsheetIO.Reading.Internal
{
    [ExcludeFromCodeCoverage]
    internal record BodyRowReadingResult<TResource>(
        NumberedResource<TResource>? NumberedResource,
        ResourceReadingFailure? Failure)
        where TResource : class
    {
        public NumberedResource<TResource>? NumberedResource { get; init; } = NumberedResource;

        public ResourceReadingFailure? Failure { get; init; } = Failure;
    }
}
