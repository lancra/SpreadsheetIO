using System.Diagnostics.CodeAnalysis;
using Ardalis.SmartEnum;

namespace LanceC.SpreadsheetIO.Shared.Internal.Indexers;

[ExcludeFromCodeCoverage]
internal class IndexerKeyKind : SmartEnum<IndexerKeyKind>
{
    public static readonly IndexerKeyKind Excel = new IndexerKeyKind(1, "Excel");

    public static readonly IndexerKeyKind Package = new IndexerKeyKind(2, "Package");

    public static readonly IndexerKeyKind Custom = new IndexerKeyKind(3, "Custom");

    private IndexerKeyKind(int id, string name)
        : base(name, id)
    {
    }
}
