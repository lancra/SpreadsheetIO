using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Styling
{
    /// <summary>
    /// Represents a style.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record Style(Border Border, Fill Fill, Font Font, NumericFormat NumericFormat)
    {
        internal Style(Border border, Fill fill, Font font, NumericFormat numericFormat, uint builtInId)
            : this(border, fill, font, numericFormat)
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
        /// Gets the numeric format.
        /// </summary>
        public NumericFormat NumericFormat { get; init; } = NumericFormat;

        /// <summary>
        /// Gets the built-in identifier for an Excel style.
        /// </summary>
        internal uint? BuiltInId { get; init; }
    }
}
