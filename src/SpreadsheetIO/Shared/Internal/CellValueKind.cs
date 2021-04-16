namespace LanceC.SpreadsheetIO.Shared.Internal
{
    internal class CellValueKind : Enumeration
    {
        public static readonly CellValueKind Boolean = new(1, "Boolean", "b");

        public static readonly CellValueKind Number = new(2, "Number", "n");

        public static readonly CellValueKind SharedString = new(3, "Shared String", "s");

        public static readonly CellValueKind String = new(4, "String", "str");

        public static readonly CellValueKind InlineString = new(5, "Inline String", "inlineStr");

        private CellValueKind(int id, string name, string xmlName)
            : base(id, name)
        {
            XmlName = xmlName;
        }

        public string XmlName { get; }
    }
}
