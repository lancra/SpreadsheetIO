using System.Collections.Generic;

namespace LanceC.SpreadsheetIO.Styling.Internal
{
    internal class BorderIndexer : IndexerBase<Border>, IBorderIndexer
    {
        protected override IReadOnlyCollection<Border> DefaultResources
            => new[] { Border.Default, };
    }
}
