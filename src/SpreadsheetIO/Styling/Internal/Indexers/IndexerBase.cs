using System;
using System.Collections.Generic;

namespace LanceC.SpreadsheetIO.Styling.Internal.Indexers
{
    internal abstract class IndexerBase<TResource>
        where TResource : IEquatable<TResource>
    {
        private readonly IDictionary<TResource, uint> _resourceIndexer = new Dictionary<TResource, uint>();
        private readonly uint _initialIndex;
        private uint _indexProvider;

        public IndexerBase(uint initialIndex = 0)
        {
            _initialIndex = initialIndex;
            _indexProvider = initialIndex;

            AddDefaultResources();
        }

        public IReadOnlyCollection<TResource> Resources
            => (IReadOnlyCollection<TResource>)_resourceIndexer.Keys;

        protected abstract IReadOnlyCollection<TResource> DefaultResources { get; }

        public uint this[TResource resource]
        {
            get
            {
                if (!_resourceIndexer.TryGetValue(resource, out var index))
                {
                    throw new KeyNotFoundException("The resource has not been indexed.");
                }

                return index;
            }
        }

        public virtual uint Add(TResource resource)
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

        public virtual void Clear()
        {
            _resourceIndexer.Clear();
            _indexProvider = _initialIndex;

            AddDefaultResources();
        }

        private void AddDefaultResources()
        {
            foreach (var resource in DefaultResources)
            {
                Add(resource);
            }
        }
    }
}
