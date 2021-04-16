using System;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;
using OpenXml = DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Reading.Internal.Readers
{
    internal class SharedStringTableElementReader : ISharedStringTableElementReader
    {
        private readonly IOpenXmlReaderWrapper _reader;

        public SharedStringTableElementReader(IOpenXmlReaderWrapper reader)
        {
            _reader = reader;
        }

        public bool ReadNextItem()
            => _reader.ReadNextElement(typeof(OpenXml.SharedStringItem), typeof(OpenXml.SharedStringTable));

        public string GetItemValue()
        {
            if (!_reader.IsStartElementOfType(typeof(OpenXml.SharedStringItem)))
            {
                throw new InvalidOperationException("Cannot retrieve item value from the unrecognized element type.");
            }

            _reader.ReadNextElement(typeof(OpenXml.Text), typeof(OpenXml.SharedStringItem));
            var itemValue = _reader.GetText();
            return itemValue;
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
