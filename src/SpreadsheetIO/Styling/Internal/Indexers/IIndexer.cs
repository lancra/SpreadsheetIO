using System;
using System.Collections.Generic;

namespace LanceC.SpreadsheetIO.Styling.Internal.Indexers
{
    /// <summary>
    /// Defines a resource indexer.
    /// </summary>
    /// <typeparam name="TResource">The type of resource to index.</typeparam>
    internal interface IIndexer<TResource>
        where TResource : IEquatable<TResource>
    {
        /// <summary>
        /// Gets the indexed resources.
        /// </summary>
        IReadOnlyCollection<TResource> Resources { get; }

        /// <summary>
        /// Gets the index for a resource.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns>The resource index.</returns>
        uint this[TResource resource] { get; }

        /// <summary>
        /// Adds a resource.
        /// </summary>
        /// <param name="resource">The resource .</param>
        /// <returns>The index for the added resource.</returns>
        uint Add(TResource resource);

        /// <summary>
        /// Clears the indexed resources.
        /// </summary>
        void Clear();
    }
}
