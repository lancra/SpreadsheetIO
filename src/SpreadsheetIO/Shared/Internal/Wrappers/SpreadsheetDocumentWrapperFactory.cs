using System.Diagnostics.CodeAnalysis;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;

namespace LanceC.SpreadsheetIO.Shared.Internal.Wrappers
{
    [ExcludeFromCodeCoverage]
    internal class SpreadsheetDocumentWrapperFactory : ISpreadsheetDocumentWrapperFactory
    {
        public ISpreadsheetDocumentWrapper Create(string path)
            => new SpreadsheetDocumentWrapper(
                SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook));

        public ISpreadsheetDocumentWrapper Create(Stream stream)
            => new SpreadsheetDocumentWrapper(
                SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook));
    }
}
