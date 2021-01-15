using System.Collections.Generic;
using Ardalis.GuardClauses;

namespace LanceC.SpreadsheetIO.Shared
{
    internal static class ColllectionExtensions
    {
        public static IReadOnlyCollection<T> AsReadOnly<T>(this ICollection<T> collection)
        {
            Guard.Against.Null(collection, nameof(collection));
            return collection as IReadOnlyCollection<T> ?? new ReadOnlyCollectionAdapter<T>(collection);
        }
    }
}
