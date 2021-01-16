using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Shared;

namespace LanceC.SpreadsheetIO.Styling.Internal.Indexers
{
    [ExcludeFromCodeCoverage]
    internal class IndexerKeyKind : Enumeration
    {
        public static readonly IndexerKeyKind Excel = new IndexerKeyKind(1, "Excel");

        public static readonly IndexerKeyKind Package = new IndexerKeyKind(2, "Package");

        public static readonly IndexerKeyKind Custom = new IndexerKeyKind(3, "Custom");

        private IndexerKeyKind(int id, string name)
            : base(id, name)
        {
        }
    }
}
