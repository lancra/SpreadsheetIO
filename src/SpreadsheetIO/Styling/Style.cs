using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Styling
{
    /// <summary>
    /// Represents a style.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record Style(Border Border, Fill Fill, Font Font)
    {
        internal Style(Border border, Fill fill, Font font, uint builtInId)
            : this(border, fill, font)
        {
            BuiltInId = builtInId;
        }

        /// <summary>
        /// Gets the border.
        /// </summary>
        public Border Border { get; init; } = Border;

        /// <summary>
        /// Gets the fill.
        /// </summary>
        public Fill Fill { get; init; } = Fill;

        /// <summary>
        /// Gets the font.
        /// </summary>
        public Font Font { get; init; } = Font;

        /// <summary>
        /// Gets the built-in identifier for an Excel style.
        /// </summary>
        internal uint? BuiltInId { get; init; }
    }
}
