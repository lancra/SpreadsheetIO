using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace LanceC.SpreadsheetIO.Styling
{
    /// <summary>
    /// Represents a border line.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record BorderLine(Color Color, BorderLineKind Kind)
    {
        /// <summary>
        /// Gets the default border line.
        /// </summary>
        public static readonly BorderLine Default = new BorderLine(Color.Black, BorderLineKind.None);

        /// <summary>
        /// Gets the line color.
        /// </summary>
        public Color Color { get; init; } = Color;

        /// <summary>
        /// Gets the line kind.
        /// </summary>
        public BorderLineKind Kind { get; init; } = Kind;
    }
}
