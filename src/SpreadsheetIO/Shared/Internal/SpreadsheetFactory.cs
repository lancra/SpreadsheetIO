using Ardalis.GuardClauses;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Builders;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Reading.Internal;
using LanceC.SpreadsheetIO.Reading.Internal.Readers;
using LanceC.SpreadsheetIO.Shared.Internal.Generators;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;
using LanceC.SpreadsheetIO.Styling.Internal.Indexers;
using LanceC.SpreadsheetIO.Writing;
using LanceC.SpreadsheetIO.Writing.Internal;
using LanceC.SpreadsheetIO.Writing.Internal.Writers;
using Microsoft.Extensions.DependencyInjection;

namespace LanceC.SpreadsheetIO.Shared.Internal;

internal class SpreadsheetFactory : ISpreadsheetFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ISpreadsheetDocumentWrapperFactory _spreadsheetDocumentFactory;

    public SpreadsheetFactory(IServiceProvider serviceProvider, ISpreadsheetDocumentWrapperFactory spreadsheetDocumentFactory)
    {
        _serviceProvider = serviceProvider;
        _spreadsheetDocumentFactory = spreadsheetDocumentFactory;
    }

    public IWritingSpreadsheet Create(Uri path)
    {
        Guard.Against.Null(path, nameof(path));

        using var scope = _serviceProvider.CreateScope();
        var spreadsheetDocument = _spreadsheetDocumentFactory.Create(path.LocalPath);

        return CreateImpl(scope, spreadsheetDocument);
    }

    public IWritingSpreadsheet Create(Stream stream)
    {
        Guard.Against.Null(stream, nameof(stream));

        using var scope = _serviceProvider.CreateScope();
        var spreadsheetDocument = _spreadsheetDocumentFactory.Create(stream);

        return CreateImpl(scope, spreadsheetDocument);
    }

    public IReadingSpreadsheet OpenRead(Uri path)
    {
        Guard.Against.Null(path, nameof(path));

        using var scope = _serviceProvider.CreateScope();
        var spreadsheetDocument = _spreadsheetDocumentFactory.Open(path.LocalPath, false);

        return OpenReadImpl(scope, spreadsheetDocument);
    }

    public IReadingSpreadsheet OpenRead(Stream stream)
    {
        Guard.Against.Null(stream, nameof(stream));

        using var scope = _serviceProvider.CreateScope();
        var spreadsheetDocument = _spreadsheetDocumentFactory.Open(stream, false);

        return OpenReadImpl(scope, spreadsheetDocument);
    }

    private static IWritingSpreadsheet CreateImpl(
        IServiceScope scope,
        ISpreadsheetDocumentWrapper spreadsheetDocument)
    {
        var styleIndexer = scope.ServiceProvider.GetRequiredService<IStyleIndexer>();
        var stringIndexer = scope.ServiceProvider.GetRequiredService<IStringIndexer>();
        var spreadsheetGenerators = scope.ServiceProvider.GetRequiredService<IEnumerable<ISpreadsheetGenerator>>();
        var spreadsheetPageMapWriter = scope.ServiceProvider.GetRequiredService<ISpreadsheetPageMapWriter>();
        var cartographer = CreateCartographer(scope);

        var spreadsheet = new WritingSpreadsheet(
            spreadsheetDocument,
            new WritingSpreadsheetPageCollection(),
            styleIndexer,
            stringIndexer,
            spreadsheetGenerators,
            spreadsheetPageMapWriter,
            cartographer);
        return spreadsheet;
    }

    private static IReadingSpreadsheet OpenReadImpl(
        IServiceScope scope,
        ISpreadsheetDocumentWrapper spreadsheetDocument)
    {
        var elementReaderFactory = scope.ServiceProvider.GetRequiredService<IElementReaderFactory>();
        var cartographer = CreateCartographer(scope);
        var mappedHeaderRowReader = scope.ServiceProvider.GetRequiredService<IMappedHeaderRowReader>();
        var operationFactory = scope.ServiceProvider.GetRequiredService<IReadingSpreadsheetPageOperationFactory>();

        var stringIndexer = scope.ServiceProvider.GetRequiredService<IStringIndexer>();
        PopulateStringIndexer(stringIndexer, spreadsheetDocument, elementReaderFactory);

        var worksheetParts = spreadsheetDocument.WorksheetParts;
        var spreadsheetPages = new ReadingSpreadsheetPageCollection();
        foreach (var worksheetPart in worksheetParts)
        {
            var spreadsheetPage = new ReadingSpreadsheetPage(
                worksheetPart,
                elementReaderFactory,
                cartographer,
                mappedHeaderRowReader,
                operationFactory);
            spreadsheetPages.Add(spreadsheetPage, worksheetPart.Name);
        }

        var spreadsheet = new ReadingSpreadsheet(spreadsheetPages);
        return spreadsheet;
    }

    private static ICartographer CreateCartographer(IServiceScope scope)
    {
        var builder = scope.ServiceProvider.GetRequiredService<IInternalCartographerBuilder>();

        var options = scope.ServiceProvider.GetService<CartographyOptions>();
        if (options is not null)
        {
            options.ConfigureAction(builder);
        }

        return builder.Build();
    }

    private static void PopulateStringIndexer(
        IStringIndexer stringIndexer,
        ISpreadsheetDocumentWrapper spreadsheetDocument,
        IElementReaderFactory elementReaderFactory)
    {
        var sharedStringTablePart = spreadsheetDocument.SharedStringTablePart;
        if (sharedStringTablePart is null)
        {
            return;
        }

        var sharedStringTableReader = elementReaderFactory
            .CreateSharedStringTableReader(sharedStringTablePart);
        while (sharedStringTableReader.ReadNextItem())
        {
            var sharedString = sharedStringTableReader.GetItemValue();
            stringIndexer.Add(sharedString);
        }
    }
}
