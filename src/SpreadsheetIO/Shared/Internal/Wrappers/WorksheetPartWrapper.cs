using System.Diagnostics.CodeAnalysis;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;

namespace LanceC.SpreadsheetIO.Shared.Internal.Wrappers
{
    [ExcludeFromCodeCoverage]
    internal class WorksheetPartWrapper : IWorksheetPartWrapper
    {
        private readonly WorksheetPart _worksheetPart;

        public WorksheetPartWrapper(WorksheetPart worksheetPart)
        {
            _worksheetPart = worksheetPart;
        }

        public IOpenXmlWriterWrapper CreateWriter()
        {
            var openXmlWriter = OpenXmlWriter.Create(_worksheetPart);
            var openXmlWriterWrapper = new OpenXmlWriterWrapper(openXmlWriter);
            return openXmlWriterWrapper;
        }
    }
}
