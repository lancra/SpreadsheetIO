using System.Diagnostics.CodeAnalysis;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;

namespace LanceC.SpreadsheetIO.Shared.Internal.Wrappers
{
    [ExcludeFromCodeCoverage]
    internal class SharedStringTablePartWrapper : ISharedStringTablePartWrapper
    {
        private readonly SharedStringTablePart _sharedStringTablePart;

        public SharedStringTablePartWrapper(SharedStringTablePart sharedStringTablePart)
        {
            _sharedStringTablePart = sharedStringTablePart;
        }

        public IOpenXmlWriterWrapper CreateWriter()
        {
            var openXmlWriter = OpenXmlWriter.Create(_sharedStringTablePart);
            var openXmlWriterWrapper = new OpenXmlWriterWrapper(openXmlWriter);
            return openXmlWriterWrapper;
        }
    }
}
