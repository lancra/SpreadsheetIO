using System.Collections.Generic;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;

namespace LanceC.SpreadsheetIO.Styling.Internal.Indexers
{
    internal class BorderIndexer : IndexerBase<Border>, IBorderIndexer
    {
        protected override IReadOnlyCollection<Border> DefaultResources
            => new[] { Border.Default, };
    }
}
