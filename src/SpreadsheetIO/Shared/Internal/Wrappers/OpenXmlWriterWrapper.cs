using System.Diagnostics.CodeAnalysis;
using DocumentFormat.OpenXml;

namespace LanceC.SpreadsheetIO.Shared.Internal.Wrappers
{
    [ExcludeFromCodeCoverage]
    internal class OpenXmlWriterWrapper : IOpenXmlWriterWrapper
    {
        private readonly OpenXmlWriter _openXmlWriter;

        public OpenXmlWriterWrapper(OpenXmlWriter openXmlWriter)
        {
            _openXmlWriter = openXmlWriter;
        }

        public IOpenXmlWriterWrapper WriteElement(OpenXmlElement element)
        {
            _openXmlWriter.WriteElement(element);
            return this;
        }

        public IOpenXmlWriterWrapper WriteStartElement(OpenXmlElement element)
        {
            _openXmlWriter.WriteStartElement(element);
            return this;
        }

        public IOpenXmlWriterWrapper WriteEndElement()
        {
            _openXmlWriter.WriteEndElement();
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
                _openXmlWriter.Dispose();
            }
        }
    }
}
