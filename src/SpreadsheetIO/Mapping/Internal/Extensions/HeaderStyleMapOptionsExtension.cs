using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Styling;

namespace LanceC.SpreadsheetIO.Mapping.Internal.Extensions
{
    [ExcludeFromCodeCoverage]
    internal class HeaderStyleMapOptionsExtension : IResourceMapOptionsExtension, IPropertyMapOptionsExtension
    {
        public HeaderStyleMapOptionsExtension(IndexerKey key, Style style)
        {
            Key = key;
            Style = style;
        }

        public IReadOnlyCollection<Type> AllowedTypes { get; } = Array.Empty<Type>();

        public IndexerKey Key { get; }

        public Style Style { get; }
    }
}
