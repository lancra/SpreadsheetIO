using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;
using OpenXml = DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Shared.Internal.Generators
{
    internal class SharedStringTableGenerator : ISpreadsheetGenerator
    {
        private readonly IStringIndexer _stringIndexer;

        public SharedStringTableGenerator(IStringIndexer stringIndexer)
        {
            _stringIndexer = stringIndexer;
        }

        public void Generate(ISpreadsheetDocumentWrapper spreadsheetDocument)
        {
            var sharedStringTablePart = spreadsheetDocument.AddSharedStringTablePart();
            using var writer = sharedStringTablePart.CreateWriter();

            writer.WriteStartElement(new OpenXml.SharedStringTable());
            foreach (var sharedString in _stringIndexer.Resources)
            {
                writer.WriteElement(
                    new OpenXml.SharedStringItem(new OpenXml.Text(sharedString)));
            }

            writer.WriteEndElement();
        }
    }
}
