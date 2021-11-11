using LanceC.SpreadsheetIO.Shared.Internal.Indexers;

namespace LanceC.SpreadsheetIO.Styling.Internal.Indexers;

internal class FillIndexer : IndexerBase<Fill>, IFillIndexer
{
    protected override IReadOnlyCollection<Fill> DefaultResources
        => new[]
        {
                Fill.Default,
                Fill.Default with { Kind = FillKind.Gray125, },
        };
}
