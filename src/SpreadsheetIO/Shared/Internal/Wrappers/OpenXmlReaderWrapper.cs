using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DocumentFormat.OpenXml;

namespace LanceC.SpreadsheetIO.Shared.Internal.Wrappers
{
    [ExcludeFromCodeCoverage]
    internal class OpenXmlReaderWrapper : IOpenXmlReaderWrapper
    {
        private readonly OpenXmlReader _openXmlReader;

        public OpenXmlReaderWrapper(OpenXmlReader openXmlReader)
        {
            _openXmlReader = openXmlReader;
        }

        public bool IsStartElement
            => _openXmlReader.IsStartElement;

        public bool IsEndElement
            => _openXmlReader.IsEndElement;

        public bool HasAttributes
            => _openXmlReader.HasAttributes;

        public Type ElementType
            => _openXmlReader.ElementType;

        public IReadOnlyCollection<OpenXmlAttribute> Attributes
            => _openXmlReader.Attributes;

        public bool Read()
            => _openXmlReader.Read();

        public string GetText()
            => _openXmlReader.GetText();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _openXmlReader.Dispose();
            }
        }
    }
}
