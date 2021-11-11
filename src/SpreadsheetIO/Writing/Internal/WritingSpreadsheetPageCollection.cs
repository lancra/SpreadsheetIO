using System.Collections;
using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Writing.Internal;

internal class WritingSpreadsheetPageCollection : IWritingSpreadsheetPageCollectionModifiable
{
    private readonly IList<IWritingSpreadsheetPage> _pages = new List<IWritingSpreadsheetPage>();
    private readonly IDictionary<string, int> _nameIndexes = new Dictionary<string, int>();

    public int Count
        => _pages.Count;

    public IWritingSpreadsheetPage this[int index]
        => _pages[index];

    public IWritingSpreadsheetPage this[string name]
    {
        get
        {
            if (!_nameIndexes.TryGetValue(name, out var index))
            {
                throw new ArgumentException(Messages.MissingSpreadsheetPageForName(name), nameof(name));
            }

            return this[index];
        }
    }

    public void Add(IWritingSpreadsheetPage spreadsheetPage)
    {
        if (_nameIndexes.TryGetValue(spreadsheetPage.Name, out var _))
        {
            throw new ArgumentException(Messages.DuplicateSpreadsheetPageForName(spreadsheetPage.Name), nameof(spreadsheetPage));
        }

        var index = Count;
        _pages.Add(spreadsheetPage);
        _nameIndexes.Add(spreadsheetPage.Name, index);
    }

    public IEnumerator<IWritingSpreadsheetPage> GetEnumerator()
        => _pages.GetEnumerator();

    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            foreach (var page in _pages)
            {
                page.Dispose();
            }
        }
    }
}
