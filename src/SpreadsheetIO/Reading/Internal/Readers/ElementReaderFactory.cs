using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;

namespace LanceC.SpreadsheetIO.Reading.Internal.Readers;

[ExcludeFromCodeCoverage]
internal class ElementReaderFactory : IElementReaderFactory
{
    private readonly IStringIndexer _stringIndexer;

    public ElementReaderFactory(IStringIndexer stringIndexer)
    {
        _stringIndexer = stringIndexer;
    }

    public ISharedStringTableElementReader CreateSharedStringTableReader(ISharedStringTablePartWrapper sharedStringTablePart)
    {
        var reader = sharedStringTablePart.CreateReader();
        var sharedStringTableReader = new SharedStringTableElementReader(reader);
        return sharedStringTableReader;
    }

    public IWorksheetElementReader CreateWorksheetReader(IWorksheetPartWrapper worksheetPart)
    {
        var reader = worksheetPart.CreateReader();
        var worksheetReader = new WorksheetElementReader(reader, _stringIndexer);
        return worksheetReader;
    }
}
