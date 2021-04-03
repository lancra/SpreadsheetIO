using System;
using System.Linq;
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

        public CellLocation(string cellReference)
        {
            Guard.Against.NullOrEmpty(cellReference, nameof(cellReference));
            Guard.Against.NonAlphanumeric(cellReference, nameof(cellReference));

            var firstDigit = cellReference.FirstOrDefault(cr => char.IsDigit(cr));
            if (firstDigit == default)
            {
                throw new ArgumentException($"The cell reference '{cellReference}' is not valid.");
            }

            var firstDigitIndex = cellReference.IndexOf(firstDigit, StringComparison.OrdinalIgnoreCase);
            if (firstDigitIndex == 0)
            {
                throw new ArgumentException($"The cell reference '{cellReference}' is not valid.");
            }

            var rowNumberText = cellReference[firstDigitIndex..];
            var validRowNumber = uint.TryParse(rowNumberText, out var rowNumber);
            if (!validRowNumber)
            {
                throw new ArgumentException($"The cell reference row number '{rowNumberText}' is not valid.");
            }

            RowNumber = rowNumber;
            Column = new ColumnLocation(cellReference.Substring(0, firstDigitIndex));
        }

        public uint RowNumber { get; }

        public ColumnLocation Column { get; }

        public string CellReference
            => Column.Letter + RowNumber;
    }
}
