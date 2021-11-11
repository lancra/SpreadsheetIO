using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Reading.Failures;
using LanceC.SpreadsheetIO.Reading.Internal.Properties;

namespace LanceC.SpreadsheetIO.Reading.Internal;

[ExcludeFromCodeCoverage]
internal class HeaderRowReadingResult<TResource>
    where TResource : class
{
    public HeaderRowReadingResult(IResourcePropertyHeaders<TResource> headers, HeaderReadingFailure? failure)
    {
        Headers = headers;
        Failure = failure;
    }

    public IResourcePropertyHeaders<TResource> Headers { get; }

    public HeaderReadingFailure? Failure { get; }
}
