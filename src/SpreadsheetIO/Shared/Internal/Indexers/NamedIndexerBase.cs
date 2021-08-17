using System;
using System.Collections.Generic;
using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Shared.Internal.Indexers
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
            => (IReadOnlyCollection<IndexerKey>)_nameIndexer.Keys;

        public IReadOnlyCollection<TResource> Resources
            => (IReadOnlyCollection<TResource>)_resourceIndexer.Keys;

        public IndexedResource<TResource> this[IndexerKey key]
        {
            get
            {
                if (!_nameIndexer.TryGetValue(key, out var resource))
                {
                    throw new KeyNotFoundException(Messages.UnindexedResourceForKey(key.Display));
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
                    throw new ArgumentException(Messages.AlreadyIndexedResourceForKey(key.Display));
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
