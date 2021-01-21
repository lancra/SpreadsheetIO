using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Writing.Internal
{
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
                    throw new ArgumentException($"No spreadsheet page was found for the name '{name}'.");
                }

                return this[index];
            }
        }

        public void Add(IWritingSpreadsheetPage spreadsheetPage)
        {
            if (_nameIndexes.TryGetValue(spreadsheetPage.Name, out var _))
            {
                throw new ArgumentException($"A spreadsheet page has already been added with the name '{spreadsheetPage.Name}'.");
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
}
