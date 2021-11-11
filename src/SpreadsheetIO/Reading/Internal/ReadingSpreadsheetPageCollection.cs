using System.Collections;
using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Reading.Internal
{
    internal class ReadingSpreadsheetPageCollection : IReadingSpreadsheetPageCollection
    {
        private readonly IList<IReadingSpreadsheetPage> _pages = new List<IReadingSpreadsheetPage>();
        private readonly IDictionary<string, int> _nameIndexes = new Dictionary<string, int>();

        public int Count
            => _pages.Count;

        public IReadingSpreadsheetPage this[int index]
            => _pages[index];

        public IReadingSpreadsheetPage this[string name]
        {
            get
            {
                if (!_nameIndexes.TryGetValue(name, out var index))
                {
                    throw new KeyNotFoundException(Messages.MissingSpreadsheetPageForName(name));
                }

                return this[index];
            }
        }

        public IEnumerator<IReadingSpreadsheetPage> GetEnumerator()
            => _pages.GetEnumerator();

        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        internal void Add(IReadingSpreadsheetPage spreadsheetPage, string name)
        {
            _pages.Add(spreadsheetPage);
            _nameIndexes.Add(name, _pages.Count - 1);
        }
    }
}
