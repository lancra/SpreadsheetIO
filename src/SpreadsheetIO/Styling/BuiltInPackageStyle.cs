using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Shared;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;

namespace LanceC.SpreadsheetIO.Styling
{
    /// <summary>
    /// Represents a style that is built-in to this package.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class BuiltInPackageStyle : Enumeration
    {
        /// <summary>
        /// Specifies the bold style.
        /// </summary>
        public static readonly BuiltInPackageStyle Bold =
            new BuiltInPackageStyle(
                1,
                "Bold",
                new Style(
                    Border.Default,
                    Fill.Default,
                    Font.Default with { IsBold = true, }));

        private BuiltInPackageStyle(int id, string name, Style style)
            : base(id, name)
        {
            Style = style;
        }

        /// <summary>
        /// Gets the style.
        /// </summary>
        public Style Style { get; }

        internal IndexerKey IndexerKey
            => new IndexerKey(Name, IndexerKeyKind.Package);
    }
}
