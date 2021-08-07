using System.Diagnostics.CodeAnalysis;
using Ardalis.SmartEnum;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;

namespace LanceC.SpreadsheetIO.Styling
{
    /// <summary>
    /// Represents a style that is built-in to this package.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class BuiltInPackageStyle : SmartEnum<BuiltInPackageStyle>
    {
        /// <summary>
        /// Specifies the bold style.
        /// </summary>
        public static readonly BuiltInPackageStyle Bold =
            new(
                1,
                "Bold",
                new Style(
                    Border.Default,
                    Fill.Default,
                    Font.Default with { IsBold = true, },
                    NumericFormat.Default,
                    Alignment.Default));

        private BuiltInPackageStyle(int id, string name, Style style)
            : base(name, id)
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
