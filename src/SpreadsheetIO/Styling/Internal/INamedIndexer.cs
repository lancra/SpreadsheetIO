using System;
using System.Collections.Generic;

namespace LanceC.SpreadsheetIO.Styling.Internal
{
    /// <summary>
    /// Defines a resource indexer that supports custom naming.
    /// </summary>
    /// <typeparam name="TResource">The type of resource to index.</typeparam>
    internal interface INamedIndexer<TResource>
        where TResource : IEquatable<TResource>
    {
        /// <summary>
        /// Gets the indexed keys.
        /// </summary>
        IReadOnlyCollection<IndexerKey> Keys { get; }

        /// <summary>
        /// Gets the index for a key.
        /// </summary>
        /// <param name="key">The indexer key.</param>
        /// <returns>The indexed resource.</returns>
        IndexedResource<TResource> this[IndexerKey key] { get; }

        /// <summary>
        /// Adds a resource tied to a key.
        /// </summary>
        /// <param name="key">The indexer key.</param>
        /// <param name="resource">The resource.</param>
        /// <returns>The index for the added resource.</returns>
        uint Add(IndexerKey key, TResource resource);

        /// <summary>
        /// Clears the indexed resources.
        /// </summary>
        void Clear();
    }
}
