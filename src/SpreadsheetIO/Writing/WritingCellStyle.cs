using Ardalis.GuardClauses;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Styling;

namespace LanceC.SpreadsheetIO.Writing
{
    /// <summary>
    /// Represents the style segment of a <see cref="WritingCell"/>.
    /// </summary>
    public class WritingCellStyle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellStyle"/> class.
        /// </summary>
        /// <param name="name">The name of the user-defined style.</param>
        public WritingCellStyle(string name)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));

            Key = new IndexerKey(name, IndexerKeyKind.Custom);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellStyle"/> class.
        /// </summary>
        /// <param name="style">The style that is built-in to Excel.</param>
        public WritingCellStyle(BuiltInExcelStyle style)
        {
            Guard.Against.Null(style, nameof(style));

            Key = style.IndexerKey;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellStyle"/> class.
        /// </summary>
        /// <param name="style">The style that is built-in to this package.</param>
        public WritingCellStyle(BuiltInPackageStyle style)
        {
            Guard.Against.Null(style, nameof(style));

            Key = style.IndexerKey;
        }

        internal IndexerKey Key { get; }
    }
}
