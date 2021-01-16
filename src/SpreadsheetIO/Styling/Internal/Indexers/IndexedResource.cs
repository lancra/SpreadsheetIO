using System;
using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Styling.Internal.Indexers
{
    [ExcludeFromCodeCoverage]
    internal class IndexedResource<TResource>
        where TResource : IEquatable<TResource>
    {
        public IndexedResource(TResource resource, uint index)
        {
            Resource = resource;
            Index = index;
        }

        public TResource Resource { get; }

        public uint Index { get; }
    }
}
