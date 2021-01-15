using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Styling.Internal
{
    [ExcludeFromCodeCoverage]
    internal record IndexerKey(string Name, IndexerKeyKind Kind)
    {
        public string Name { get; init; } = Name;

        public IndexerKeyKind Kind { get; init; } = Kind;

        public string Display
            => $"'{Name}' from the '{Kind.Name}' source";
    }
}
