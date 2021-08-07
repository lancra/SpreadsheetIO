using Ardalis.SmartEnum;

namespace LanceC.SpreadsheetIO.Shared.Internal
{
    internal class ElementAttributeKind : SmartEnum<ElementAttributeKind>
    {
        public static readonly ElementAttributeKind RowNumber = new(1, "Row Number", "r");

        public static readonly ElementAttributeKind CellReference = new(2, "Cell Reference", "r");

        public static readonly ElementAttributeKind CellValueType = new(3, "Cell Value Type", "t");

        private ElementAttributeKind(int id, string name, string localName)
            : base(name, id)
        {
            LocalName = localName;
        }

        public string LocalName { get; }
    }
}
