using System;

namespace LanceC.SpreadsheetIO.Shared.Internal.Indexers
{
    /// <summary>
    /// Defines a resource indexer that includes the reverse indexes.
    /// </summary>
    /// <typeparam name="TResource">The type of resource to index.</typeparam>
    internal interface IReverseIndexer<TResource> : IIndexer<TResource>
        where TResource : IEquatable<TResource>
    {
        /// <summary>
        /// Gets the resource for an index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The resource for the <paramref name="index"/>.</returns>
        TResource this[uint index] { get; }
    }
}
