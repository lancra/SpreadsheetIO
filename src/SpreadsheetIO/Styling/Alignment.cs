using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Styling
{
    /// <summary>
    /// Represents an alignment.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record Alignment(HorizontalAlignmentKind HorizontalKind, VerticalAlignmentKind VerticalKind)
    {
        /// <summary>
        /// Gets the default alignment.
        /// </summary>
        public static readonly Alignment Default = new(HorizontalAlignmentKind.General, VerticalAlignmentKind.Bottom);

        /// <summary>
        /// Gets the horizontal alignment kind.
        /// </summary>
        public HorizontalAlignmentKind HorizontalKind { get; init; } = HorizontalKind;

        /// <summary>
        /// Gets the vertical alignment kind.
        /// </summary>
        public VerticalAlignmentKind VerticalKind { get; init; } = VerticalKind;
    }
}
