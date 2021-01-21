using System;
using LanceC.SpreadsheetIO.Shared.Internal;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;
using LanceC.SpreadsheetIO.Styling.Internal.Indexers;
using OpenXml = DocumentFormat.OpenXml;

namespace LanceC.SpreadsheetIO.Writing.Internal
{
    internal class WritingSpreadsheetPage : IWritingSpreadsheetPage
    {
        private const uint DefaultNumber = 1;

        private readonly IOpenXmlWriterWrapper _writer;
        private readonly IStyleIndexer _styleIndexer;
        private readonly IStringIndexer _stringIndexer;

        private bool _isWorksheetEndWritten = false;
        private bool _isCurrentRowStartWritten = false;

        public WritingSpreadsheetPage(
            IWorksheetPartWrapper worksheetPart,
            IStyleIndexer styleIndexer,
            IStringIndexer stringIndexer)
        {
            _writer = worksheetPart.CreateWriter();
            _writer.WriteStartElement(new OpenXml.Spreadsheet.Worksheet());
            _writer.WriteStartElement(new OpenXml.Spreadsheet.SheetData());

            _styleIndexer = styleIndexer;
            _stringIndexer = stringIndexer;
            Name = worksheetPart.Name;
        }

        public string Name { get; }

        public bool CanWrite
            => !_isWorksheetEndWritten;

        public uint CurrentRowNumber { get; private set; } = DefaultNumber;

        public uint CurrentColumnNumber { get; private set; } = DefaultNumber;

        public IWritingSpreadsheetPage AddCell(WritingCell cell)
        {
            ThrowWhenUnwritable();

            var cellLocation = new CellLocation(CurrentRowNumber, CurrentColumnNumber);

            var openXmlCell = new OpenXml.Spreadsheet.Cell
            {
                CellReference = new OpenXml.StringValue(cellLocation.CellReference),
            };

            if (cell.Style is not null)
            {
                var indexedStyle = _styleIndexer[cell.Style.Key];
                openXmlCell.StyleIndex = new OpenXml.UInt32Value(indexedStyle.Index);
            }

            if (cell.Value.CellModifier is not null)
            {
                cell.Value.CellModifier(openXmlCell);

                if (openXmlCell.DataType is not null && openXmlCell.DataType == OpenXml.Spreadsheet.CellValues.SharedString)
                {
                    var stringIndex = _stringIndexer.Add(openXmlCell.CellValue.Text);
                    openXmlCell.CellValue.Text = stringIndex.ToString();
                }
            }

            WriteRowStartIfMissing();

            _writer.WriteElement(openXmlCell);
            CurrentColumnNumber++;

            return this;
        }

        public IWritingSpreadsheetPage AddCell(string value)
            => AddCell(
                new WritingCell(
                    new WritingCellValue(value)));

        public IWritingSpreadsheetPage AddCell(string value, string styleName)
            => AddCell(
                new WritingCell(
                    new WritingCellValue(value),
                    new WritingCellStyle(styleName)));

        public IWritingSpreadsheetPage AdvanceRow()
            => AdvanceRows(1);

        public IWritingSpreadsheetPage AdvanceRows(uint count)
        {
            if (count == 0)
            {
                throw new ArgumentException("Cannot advance zero rows.");
            }

            ThrowWhenUnwritable();

            WriteRowEndIfMissing();
            CurrentRowNumber += count;
            CurrentColumnNumber = DefaultNumber;

            return this;
        }

        public IWritingSpreadsheetPage AdvanceToRow(uint rowNumber)
        {
            if (rowNumber < CurrentRowNumber)
            {
                throw new ArgumentException($"Cannot to advance backwards from row {CurrentRowNumber} to {rowNumber}.");
            }

            var count = rowNumber - CurrentRowNumber;
            return AdvanceRows(count);
        }

        public IWritingSpreadsheetPage AdvanceColumn()
            => AdvanceColumns(1);

        public IWritingSpreadsheetPage AdvanceColumns(uint count)
        {
            if (count == 0)
            {
                throw new ArgumentException("Cannot advance zero columns.");
            }

            ThrowWhenUnwritable();

            CurrentColumnNumber += count;

            return this;
        }

        public IWritingSpreadsheetPage AdvanceToColumn(uint columnNumber)
        {
            if (columnNumber < CurrentColumnNumber)
            {
                throw new ArgumentException($"Unable to advance backwards from column {CurrentColumnNumber} to {columnNumber}");
            }

            var count = columnNumber - CurrentColumnNumber;
            return AdvanceColumns(count);
        }

        public IWritingSpreadsheetPage AdvanceToColumn(string columnLetter)
        {
            var column = new ColumnLocation(columnLetter);
            return AdvanceToColumn(column.Number);
        }

        public IWritingSpreadsheetPage Finish()
        {
            if (CanWrite)
            {
                WriteRowEndIfMissing();

                _writer.WriteEndElement()
                    .WriteEndElement();

                _isWorksheetEndWritten = true;
            }

            return this;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Finish();
                _writer.Dispose();
            }
        }

        private void WriteRowStartIfMissing()
        {
            if (!_isCurrentRowStartWritten)
            {
                var row = new OpenXml.Spreadsheet.Row
                {
                    RowIndex = CurrentRowNumber,
                };

                _writer.WriteStartElement(row);
                _isCurrentRowStartWritten = true;
            }
        }

        private void WriteRowEndIfMissing()
        {
            if (_isCurrentRowStartWritten)
            {
                _writer.WriteEndElement();
                _isCurrentRowStartWritten = false;
            }
        }

        private void ThrowWhenUnwritable()
        {
            if (!CanWrite)
            {
                throw new InvalidOperationException("This operation cannot be performed since writing has been closed.");
            }
        }
    }
}
