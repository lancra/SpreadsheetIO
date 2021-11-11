using LanceC.SpreadsheetIO.Properties;
using LanceC.SpreadsheetIO.Shared.Internal;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;
using OpenXml = DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Reading.Internal.Readers
{
    internal class WorksheetElementReader : IWorksheetElementReader
    {
        private readonly IOpenXmlReaderWrapper _reader;
        private readonly IStringIndexer _stringIndexer;

        public WorksheetElementReader(IOpenXmlReaderWrapper reader, IStringIndexer stringIndexer)
        {
            _reader = reader;
            _stringIndexer = stringIndexer;
        }

        public bool ReadToRow(uint number)
        {
            while (ReadNextRow())
            {
                var rowNumber = GetRowNumber();
                if (rowNumber == number)
                {
                    return true;
                }
            }

            return false;
        }

        public bool ReadNextRow()
            => _reader.ReadNextElement(typeof(OpenXml.Row), typeof(OpenXml.SheetData));

        public uint GetRowNumber()
        {
            if (!_reader.IsStartElementOfType(typeof(OpenXml.Row)))
            {
                throw new InvalidOperationException(Messages.InvalidElementForValue("row number"));
            }

            var rowNumberAttribute = _reader.GetAttribute(ElementAttributeKind.RowNumber);
            var rowNumber = Convert.ToUInt32(rowNumberAttribute.Value);
            return rowNumber;
        }

        public bool ReadNextCell()
            => _reader.ReadNextElement(typeof(OpenXml.Cell), typeof(OpenXml.Row));

        public CellLocation GetCellLocation()
        {
            if (!_reader.IsStartElementOfType(typeof(OpenXml.Cell)))
            {
                throw new InvalidOperationException(Messages.InvalidElementForValue("cell location"));
            }

            var cellReferenceAttribute = _reader.GetAttribute(ElementAttributeKind.CellReference);
            var cellLocation = new CellLocation(cellReferenceAttribute.Value);
            return cellLocation;
        }

        public string GetCellValue()
        {
            if (!_reader.IsStartElementOfType(typeof(OpenXml.Cell)))
            {
                throw new InvalidOperationException(Messages.InvalidElementForValue("cell value"));
            }

            var valueTypeAttribute = _reader.GetAttribute(ElementAttributeKind.CellValueType);
            var hasTextChildElement = valueTypeAttribute != default &&
                valueTypeAttribute.Value.Equals(CellValueKind.InlineString.XmlName, StringComparison.OrdinalIgnoreCase);
            var valueElementType = hasTextChildElement
                ? typeof(OpenXml.Text)
                : typeof(OpenXml.CellValue);

            _reader.ReadNextElement(valueElementType, typeof(OpenXml.Cell));
            var cellValue = _reader.GetText();

            if (valueTypeAttribute != default &&
                valueTypeAttribute.Value.Equals(CellValueKind.SharedString.XmlName, StringComparison.OrdinalIgnoreCase))
            {
                var stringIndex = Convert.ToUInt32(cellValue);
                cellValue = _stringIndexer[stringIndex];
            }

            return cellValue;
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
                _reader.Dispose();
            }
        }
    }
}
