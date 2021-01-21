using Ardalis.GuardClauses;

namespace LanceC.SpreadsheetIO.Writing
{
    /// <summary>
    /// Represents a cell to be written.
    /// </summary>
    public class WritingCell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCell"/> class.
        /// </summary>
        /// <param name="value">The cell value.</param>
        public WritingCell(WritingCellValue value)
        {
            Guard.Against.Null(value, nameof(value));

            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCell"/> class.
        /// </summary>
        /// <param name="value">The cell value.</param>
        /// <param name="style">The cell style.</param>
        public WritingCell(WritingCellValue value, WritingCellStyle style)
            : this(value)
        {
            Guard.Against.Null(style, nameof(style));

            Style = style;
        }

        /// <summary>
        /// Gets the cell value.
        /// </summary>
        public WritingCellValue Value { get; }

        /// <summary>
        /// Gets the cell style.
        /// </summary>
        public WritingCellStyle? Style { get; }
    }
}
