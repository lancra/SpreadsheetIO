using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Styling
{
    /// <summary>
    /// Represents a border.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record Border(BorderLine LeftLine, BorderLine RightLine, BorderLine TopLine, BorderLine BottomLine)
    {
        /// <summary>
        /// Gets the default border.
        /// </summary>
        public static readonly Border Default =
            new Border(BorderLine.Default);

        /// <summary>
        /// Initializes a new instance of the <see cref="Border"/> class.
        /// </summary>
        /// <param name="line">The line for all four sides.</param>
        public Border(BorderLine line)
            : this(line, line, line, line)
        {
        }

        /// <summary>
        /// Gets the left border line.
        /// </summary>
        public BorderLine LeftLine { get; init; } = LeftLine;

        /// <summary>
        /// Gets the right border line.
        /// </summary>
        public BorderLine RightLine { get; init; } = RightLine;

        /// <summary>
        /// Gets the top border line.
        /// </summary>
        public BorderLine TopLine { get; init; } = TopLine;

        /// <summary>
        /// Gets the bottom border line.
        /// </summary>
        public BorderLine BottomLine { get; init; } = BottomLine;
    }
}
