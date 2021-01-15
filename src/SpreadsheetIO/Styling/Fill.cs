using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace LanceC.SpreadsheetIO.Styling
{
    /// <summary>
    /// Represents a fill.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record Fill(FillKind Kind, Color ForegroundColor)
    {
        /// <summary>
        /// Gets the default fill.
        /// </summary>
        public static readonly Fill Default = new Fill(FillKind.None, Color.White);

        /// <summary>
        /// Gets the fill kind.
        /// </summary>
        public FillKind Kind { get; init; } = Kind;

        /// <summary>
        /// Gets the fill foreground color.
        /// </summary>
        public Color ForegroundColor { get; init; } = ForegroundColor;
    }
}
