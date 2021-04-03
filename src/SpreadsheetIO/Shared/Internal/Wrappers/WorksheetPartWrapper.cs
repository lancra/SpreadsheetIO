using System.Diagnostics.CodeAnalysis;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;

namespace LanceC.SpreadsheetIO.Shared.Internal.Wrappers
{
    [ExcludeFromCodeCoverage]
    internal class WorksheetPartWrapper : IWorksheetPartWrapper
    {
        private readonly WorksheetPart _worksheetPart;

        public WorksheetPartWrapper(WorksheetPart worksheetPart, string name)
        {
            _worksheetPart = worksheetPart;
            Name = name;
        }

        public string Name { get; }

        public IOpenXmlReaderWrapper CreateReader()
        {
            var openXmlReader = OpenXmlReader.Create(_worksheetPart);
            var openXmlReaderWrapper = new OpenXmlReaderWrapper(openXmlReader);
            return openXmlReaderWrapper;
        }

        public IOpenXmlWriterWrapper CreateWriter()
        {
            var openXmlWriter = OpenXmlWriter.Create(_worksheetPart);
            var openXmlWriterWrapper = new OpenXmlWriterWrapper(openXmlWriter);
            return openXmlWriterWrapper;
        }
    }
}
