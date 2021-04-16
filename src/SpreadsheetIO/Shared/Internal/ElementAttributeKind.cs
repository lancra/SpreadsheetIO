namespace LanceC.SpreadsheetIO.Shared.Internal
{
    internal class ElementAttributeKind : Enumeration
    {
        public static readonly ElementAttributeKind RowNumber = new(1, "Row Number", "r");

        public static readonly ElementAttributeKind CellReference = new(2, "Cell Reference", "r");

        public static readonly ElementAttributeKind CellValueType = new(3, "Cell Value Type", "t");

        private ElementAttributeKind(int id, string name, string localName)
            : base(id, name)
        {
            LocalName = localName;
        }

        public string LocalName { get; }
    }
}
