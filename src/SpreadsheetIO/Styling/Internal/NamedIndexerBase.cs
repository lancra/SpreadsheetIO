using System;
using System.Collections.Generic;
using LanceC.SpreadsheetIO.Shared;

namespace LanceC.SpreadsheetIO.Styling.Internal
{
    internal abstract class NamedIndexerBase<TResource>
        where TResource : IEquatable<TResource>
    {
        private readonly IDictionary<IndexerKey, TResource> _nameIndexer = new Dictionary<IndexerKey, TResource>();
        private readonly IDictionary<TResource, uint> _resourceIndexer = new Dictionary<TResource, uint>();
        private readonly uint _initialIndex;
        private uint _indexProvider;

        public NamedIndexerBase(uint initialIndex = 0)
        {
            _initialIndex = initialIndex;
            _indexProvider = initialIndex;
        }

        public IReadOnlyCollection<IndexerKey> Keys
            => _nameIndexer.Keys.AsReadOnly();

        public IndexedResource<TResource> this[IndexerKey key]
        {
            get
            {
                if (!_nameIndexer.TryGetValue(key, out var resource))
                {
                    throw new KeyNotFoundException($"No resource is indexed for {key.Display}.");
                }

                var index = _resourceIndexer[resource];
                var indexedResource = new IndexedResource<TResource>(resource, index);
                return indexedResource;
            }
        }

        public virtual uint Add(IndexerKey key, TResource resource)
        {
            if (_nameIndexer.TryGetValue(key, out var storedResource))
            {
                if (!resource.Equals(storedResource))
                {
                    throw new ArgumentException($"Another resource is already indexed for {key.Display}.");
                }

                var index = _resourceIndexer[resource];
                return index;
            }
            else
            {
                _nameIndexer.Add(key, resource);
                var index = Add(resource);
                return index;
            }
        }

        public virtual void Clear()
        {
            _nameIndexer.Clear();
            _resourceIndexer.Clear();
            _indexProvider = _initialIndex;
        }

        private uint Add(TResource resource)
        {
            if (_resourceIndexer.TryGetValue(resource, out var index))
            {
                return index;
            }

            index = _indexProvider;
            _resourceIndexer.Add(resource, index);
            _indexProvider++;

            return index;
        }
    }
}
