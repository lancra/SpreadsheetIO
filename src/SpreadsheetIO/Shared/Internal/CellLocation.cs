using Ardalis.GuardClauses;

namespace LanceC.SpreadsheetIO.Shared.Internal
{
    internal class CellLocation
    {
        public CellLocation(uint rowNumber, uint columnNumber)
        {
            Guard.Against.Zero(rowNumber, nameof(rowNumber));
            Guard.Against.Zero(columnNumber, nameof(columnNumber));

            RowNumber = rowNumber;
            Column = new ColumnLocation(columnNumber);
        }

        public CellLocation(uint rowNumber, string columnLetter)
        {
            Guard.Against.Zero(rowNumber, nameof(rowNumber));
            Guard.Against.NullOrEmpty(columnLetter, nameof(columnLetter));
            Guard.Against.NonAlphabetic(columnLetter, nameof(columnLetter));

            RowNumber = rowNumber;
            Column = new ColumnLocation(columnLetter);
        }

        public uint RowNumber { get; }

        public ColumnLocation Column { get; }

        public string CellReference
            => Column.Letter + RowNumber;
    }
}
