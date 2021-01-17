using LanceC.SpreadsheetIO.Shared.Internal.Indexers;

namespace LanceC.SpreadsheetIO.Styling.Internal.Indexers
{
    internal class StyleIndexer : NamedIndexerBase<Style>, IStyleIndexer
    {
        private readonly IBorderIndexer _borderIndexer;
        private readonly IFillIndexer _fillIndexer;
        private readonly IFontIndexer _fontIndexer;

        public StyleIndexer(IBorderIndexer borderIndexer, IFillIndexer fillIndexer, IFontIndexer fontIndexer)
        {
            _borderIndexer = borderIndexer;
            _fillIndexer = fillIndexer;
            _fontIndexer = fontIndexer;

            AddDefaultStyle();
        }

        public override uint Add(IndexerKey key, Style style)
        {
            _borderIndexer.Add(style.Border);
            _fillIndexer.Add(style.Fill);
            _fontIndexer.Add(style.Font);

            var index = base.Add(key, style);
            return index;
        }

        public override void Clear()
        {
            _borderIndexer.Clear();
            _fillIndexer.Clear();
            _fontIndexer.Clear();
            base.Clear();

            AddDefaultStyle();
        }

        private void AddDefaultStyle()
            => Add(BuiltInExcelStyle.Normal.IndexerKey, BuiltInExcelStyle.Normal.Style);
    }
}
