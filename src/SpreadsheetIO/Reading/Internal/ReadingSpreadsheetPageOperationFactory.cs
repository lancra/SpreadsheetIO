using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Mapping2;
using LanceC.SpreadsheetIO.Reading.Internal.Readers;

namespace LanceC.SpreadsheetIO.Reading.Internal;

[ExcludeFromCodeCoverage]
internal class ReadingSpreadsheetPageOperationFactory : IReadingSpreadsheetPageOperationFactory
{
    private readonly IMappedBodyRowReader _mappedBodyRowReader;

    public ReadingSpreadsheetPageOperationFactory(IMappedBodyRowReader mappedBodyRowReader)
    {
        _mappedBodyRowReader = mappedBodyRowReader;
    }

    public IReadingSpreadsheetPageOperation<TResource> Create<TResource>(
        IWorksheetElementReader worksheetReader,
        HeaderRowReadingResult<TResource> headerRowResult,
        ResourceMap map)
        where TResource : class
        => new ReadingSpreadsheetPageOperation<TResource>(
            worksheetReader,
            headerRowResult,
            map,
            _mappedBodyRowReader);
}
