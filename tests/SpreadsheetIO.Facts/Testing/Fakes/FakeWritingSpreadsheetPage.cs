using System;
using System.Collections.Generic;
using LanceC.SpreadsheetIO.Shared.Internal;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Writing;
using OpenXml = DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Facts.Testing.Fakes
{
    internal class FakeWritingSpreadsheetPage : IWritingSpreadsheetPage
    {
        private readonly Dictionary<(uint RowNumber, uint ColumnNumber), WritingCell> _addedCells = new();

        public string Name
            => string.Empty;

        public bool CanWrite
            => true;

        public uint CurrentRowNumber { get; private set; } = 1;

        public uint CurrentColumnNumber { get; private set; } = 1;

        public WritingCell GetAddedCell(uint rowNumber, uint columnNumber)
            => _addedCells[(rowNumber, columnNumber)];

        public void VerifyAddCell(uint rowNumber, uint columnNumber, WritingCellValue cellValue, IndexerKey? cellStyleKey)
        {
            var addedCell = _addedCells.TryGetValue((rowNumber, columnNumber), out var cell);
            if (!addedCell)
            {
                throw new InvalidOperationException($"No cell was added at row {rowNumber} and column {columnNumber}.");
            }

            if (cell!.Value != cellValue)
            {
                throw new InvalidOperationException(
                    $"The cell value at row {rowNumber} and column {columnNumber} does not match the expected value.");
            }

            if (cell.Style?.Key?.Kind != cellStyleKey?.Kind || cell.Style?.Key?.Name != cellStyleKey?.Name)
            {
                throw new InvalidOperationException(
                    $"The cell style at row {rowNumber} and column {columnNumber} does not match the expected style.");
            }
        }

        public void VerifyAddCell(uint rowNumber, uint columnNumber, string value, IndexerKey? cellStyleKey)
        {
            var addedCell = _addedCells.TryGetValue((rowNumber, columnNumber), out var cell);
            if (!addedCell)
            {
                throw new InvalidOperationException($"No cell was added at row {rowNumber} and column {columnNumber}.");
            }

            var openXmlCell = new OpenXml.Cell();
            cell?.Value.CellModifier?.Invoke(openXmlCell);

            if (openXmlCell.CellValue.Text != value)
            {
                throw new InvalidOperationException(
                    $"The cell value at row {rowNumber} and column {columnNumber} does not match the expected value.");
            }

            if (cell!.Style?.Key?.Kind != cellStyleKey?.Kind || cell.Style?.Key?.Name != cellStyleKey?.Name)
            {
                throw new InvalidOperationException(
                    $"The cell style at row {rowNumber} and column {columnNumber} does not match the expected style.");
            }
        }

        public IWritingSpreadsheetPage AddCell(WritingCell cell)
        {
            _addedCells.Add((CurrentRowNumber, CurrentColumnNumber), cell);
            return this;
        }

        public IWritingSpreadsheetPage AddCell(string value)
            => this;

        public IWritingSpreadsheetPage AddCell(string value, string styleName)
            => this;

        public IWritingSpreadsheetPage AdvanceRow()
        {
            CurrentRowNumber++;
            CurrentColumnNumber = 1;
            return this;
        }

        public IWritingSpreadsheetPage AdvanceRows(uint count)
        {
            CurrentRowNumber += count;
            CurrentColumnNumber = 1;
            return this;
        }

        public IWritingSpreadsheetPage AdvanceToRow(uint rowNumber)
        {
            CurrentRowNumber = rowNumber;
            CurrentColumnNumber = 1;
            return this;
        }

        public IWritingSpreadsheetPage AdvanceColumn()
        {
            CurrentColumnNumber++;
            return this;
        }

        public IWritingSpreadsheetPage AdvanceColumns(uint count)
        {
            CurrentColumnNumber += count;
            return this;
        }

        public IWritingSpreadsheetPage AdvanceToColumn(uint columnNumber)
        {
            CurrentColumnNumber = columnNumber;
            return this;
        }

        public IWritingSpreadsheetPage AdvanceToColumn(string columnLetter)
        {
            var location = new CellLocation(columnLetter);
            CurrentColumnNumber = location.Column.Number;
            return this;
        }

        public IWritingSpreadsheetPage Finish()
            => this;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
