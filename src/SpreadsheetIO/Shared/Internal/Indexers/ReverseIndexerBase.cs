using System;
using System.Collections.Generic;
using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Shared.Internal.Indexers
{
    internal abstract class ReverseIndexerBase<TResource> : IndexerBase<TResource>
        where TResource : IEquatable<TResource>
    {
        private readonly IDictionary<uint, TResource> _reverseResourceIndexer = new Dictionary<uint, TResource>();

        public virtual TResource this[uint index]
        {
            get
            {
                if (!_reverseResourceIndexer.TryGetValue(index, out var resource))
                {
                    throw new KeyNotFoundException(Messages.MissingResourceForIndex);
                }

                return resource;
            }
        }

        public override uint Add(TResource resource)
        {
            var index = base.Add(resource);

            if (!_reverseResourceIndexer.ContainsKey(index))
            {
                _reverseResourceIndexer.Add(index, resource);
            }

            return index;
        }

        public override void Clear()
        {
            _reverseResourceIndexer.Clear();

            base.Clear();
        }
    }
}
