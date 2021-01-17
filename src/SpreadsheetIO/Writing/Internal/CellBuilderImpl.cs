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

        public OpenXml.Cell Cell { get; private set; }

        public Style? Style { get; private set; }

        public CellStringKind StringKind { get; private set; } = CellStringKind.SharedString;

        public ICellValueBuilder WithStyle(Style style)
        {
            Style = style;
            return this;
        }

        public ICellStringValueBuilder WrittenAs(CellStringKind kind)
        {
            StringKind = kind;
            return this;
        }
    }
}
