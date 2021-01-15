using System.Collections.Generic;

namespace LanceC.SpreadsheetIO.Styling.Internal
{
    internal class FontIndexer : IndexerBase<Font>, IFontIndexer
    {
        protected override IReadOnlyCollection<Font> DefaultResources
            => new[] { Font.Default, };
    }
}
