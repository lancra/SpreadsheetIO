using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Options;
using LanceC.SpreadsheetIO.Reading.Failures;
using LanceC.SpreadsheetIO.Reading.Internal.Readers;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;

namespace LanceC.SpreadsheetIO.Reading.Internal;

internal class ReadingSpreadsheetPage : IReadingSpreadsheetPage
{
    private readonly IWorksheetPartWrapper _worksheetPart;
    private readonly IElementReaderFactory _elementReaderFactory;
    private readonly ICartographer _cartographer;
    private readonly IMappedHeaderRowReader _mappedHeaderRowReader;
    private readonly IReadingSpreadsheetPageOperationFactory _operationFactory;

    public ReadingSpreadsheetPage(
        IWorksheetPartWrapper worksheetPart,
        IElementReaderFactory elementReaderFactory,
        ICartographer cartographer,
        IMappedHeaderRowReader mappedHeaderRowReader,
        IReadingSpreadsheetPageOperationFactory operationFactory)
    {
        _worksheetPart = worksheetPart;
        _elementReaderFactory = elementReaderFactory;
        _cartographer = cartographer;
        _mappedHeaderRowReader = mappedHeaderRowReader;
        _operationFactory = operationFactory;
    }

    public ReadingResult<TResource> ReadAll<TResource>()
        where TResource : class
    {
        var map = _cartographer.GetMap<TResource>();
        var result = ReadAllImpl<TResource>(map);
        return result;
    }

    public IReadingSpreadsheetPageOperation<TResource> StartRead<TResource>()
        where TResource : class
    {
        var map = _cartographer.GetMap<TResource>();
        var operation = StartReadImpl<TResource>(map);
        return operation;
    }

    private ReadingResult<TResource> ReadAllImpl<TResource>(ResourceMap map)
        where TResource : class
    {
        using var operation = StartReadImpl<TResource>(map);
        if (operation.HeaderFailure is not null)
        {
            return new ReadingResult<TResource>(
                Array.Empty<NumberedResource<TResource>>(),
                operation.HeaderFailure,
                Array.Empty<ResourceReadingFailure>());
        }

        var resources = new List<NumberedResource<TResource>>();
        var resourceFailures = new List<ResourceReadingFailure>();

        var shouldExitOnResourceReadingFailure = map.Options.Has<ExitOnResourceReadingFailureResourceMapOption>();
        while (operation.ReadNext())
        {
            if (operation.CurrentResult!.NumberedResource is not null)
            {
                resources.Add(operation.CurrentResult.NumberedResource);
            }

            if (operation.CurrentResult!.Failure is not null)
            {
                resourceFailures.Add(operation.CurrentResult.Failure);

                if (shouldExitOnResourceReadingFailure)
                {
                    break;
                }
            }
        }

        var result = new ReadingResult<TResource>(resources, default, resourceFailures);
        return result;
    }

    private IReadingSpreadsheetPageOperation<TResource> StartReadImpl<TResource>(ResourceMap map)
        where TResource : class
    {
        var worksheetReader = _elementReaderFactory.CreateWorksheetReader(_worksheetPart);
        var headerRowResult = _mappedHeaderRowReader.Read<TResource>(worksheetReader, map);
        var operation = _operationFactory.Create(worksheetReader, headerRowResult, map);
        return operation;
    }
}
