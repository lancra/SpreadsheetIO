using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Styling;
using OpenXml = DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Writing.Internal
{
    internal class CellBuilderImpl : ICellStringValueBuilder
    {
        public CellBuilderImpl(OpenXml.Cell cell)
        {
            Cell = cell;
        }

        internal OpenXml.Cell Cell { get; private set; }

        internal IndexerKey? StyleKey { get; private set; }

        internal CellStringKind StringKind { get; private set; } = CellStringKind.SharedString;

        public ICellValueBuilder WithStyle(string name)
        {
            StyleKey = new IndexerKey(name, IndexerKeyKind.Custom);
            return this;
        }

        public ICellValueBuilder WithStyle(BuiltInExcelStyle style)
        {
            StyleKey = style.IndexerKey;
            return this;
        }

        public ICellValueBuilder WithStyle(BuiltInPackageStyle style)
        {
            StyleKey = style.IndexerKey;
            return this;
        }

        public ICellStringValueBuilder WrittenAs(CellStringKind kind)
        {
            StringKind = kind;
            return this;
        }
    }
}
