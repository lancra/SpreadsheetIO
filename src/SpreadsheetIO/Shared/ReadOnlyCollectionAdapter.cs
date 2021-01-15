using System.Collections;
using System.Collections.Generic;

namespace LanceC.SpreadsheetIO.Shared
{
    internal class ReadOnlyCollectionAdapter<T> : IReadOnlyCollection<T>
    {
        private readonly ICollection<T> _collection;

        public ReadOnlyCollectionAdapter(ICollection<T> collection)
        {
            _collection = collection;
        }

        public int Count
            => _collection.Count;

        public IEnumerator<T> GetEnumerator()
            => _collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
