using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Reading.Failures;

namespace LanceC.SpreadsheetIO.Reading.Internal
{
    [ExcludeFromCodeCoverage]
    internal class BodyRowReadingResult<TResource>
        where TResource : class
    {
        public BodyRowReadingResult(TResource? resource, ResourceReadingFailure? failure)
        {
            Resource = resource;
            Failure = failure;
        }

        public TResource? Resource { get; }

        public ResourceReadingFailure? Failure { get; }
    }
}
