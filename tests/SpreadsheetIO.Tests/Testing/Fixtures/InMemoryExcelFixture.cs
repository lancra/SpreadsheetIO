using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Writing;

namespace LanceC.SpreadsheetIO.Tests.Testing.Fixtures;

public class InMemoryExcelFixture : ExcelFixtureBase, IDisposable
{
    public Stream Stream { get; } = new MemoryStream();

    public void ArrangeSpreadsheet(
        Action<IWritingSpreadsheet> spreadsheetAction,
        Action<ICartographerBuilder>? mapOptions = default)
    {
        using var spreadsheet = GetSpreadsheetFactory(mapOptions)
            .Create(Stream);
        spreadsheetAction(spreadsheet);
    }

    public IReadingSpreadsheet OpenReadSpreadsheet(Action<ICartographerBuilder>? mapOptions = default)
        => GetSpreadsheetFactory(mapOptions)
        .OpenRead(Stream);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Stream.Dispose();
        }
    }
}
