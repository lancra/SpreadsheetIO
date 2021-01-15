using System.Collections.Generic;

namespace LanceC.SpreadsheetIO.Styling.Internal
{
    internal class FillIndexer : IndexerBase<Fill>, IFillIndexer
    {
        protected override IReadOnlyCollection<Fill> DefaultResources
            => new[]
            {
                Fill.Default,
                Fill.Default with { Kind = FillKind.Gray125, },
            };
    }
}
