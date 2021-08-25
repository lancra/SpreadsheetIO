using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace LanceC.SpreadsheetIO.Styling
{
    /// <summary>
    /// Represents a font.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record Font(string Name, double Size, Color Color, bool IsBold = false, bool IsItalic = false)
    {
        /// <summary>
        /// Gets the default font.
        /// </summary>
        public static readonly Font Default = new("Calibri", 11D, Color.Black);

        /// <summary>
        /// Gets the font name.
        /// </summary>
        public string Name { get; init; } = Name;

        /// <summary>
        /// Gets the font size.
        /// </summary>
        public double Size { get; init; } = Size;

        /// <summary>
        /// Gets the font color.
        /// </summary>
        public Color Color { get; init; } = Color;

        /// <summary>
        /// Gets the value that determines whether the font is bold.
        /// </summary>
        public bool IsBold { get; init; } = IsBold;

        /// <summary>
        /// Gets the value that determines whether the font is italicized.
        /// </summary>
        public bool IsItalic { get; init; } = IsItalic;
    }
}
