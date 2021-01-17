using System.Collections.Generic;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;

namespace LanceC.SpreadsheetIO.Styling.Internal.Indexers
{
    internal class FontIndexer : IndexerBase<Font>, IFontIndexer
    {
        protected override IReadOnlyCollection<Font> DefaultResources
            => new[] { Font.Default, };
    }
}
