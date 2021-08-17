using System;
using System.Linq;
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
            if (!_stringIndexer.Resources.Any())
            {
                return;
            }

            var sharedStringTablePart = spreadsheetDocument.AddSharedStringTablePart();
            using var writer = sharedStringTablePart.CreateWriter();

            writer.WriteStartElement(
                new OpenXml.SharedStringTable
                {
                    Count = Convert.ToUInt32(_stringIndexer.Resources.Count),
                });
            foreach (var sharedString in _stringIndexer.Resources)
            {
                writer.WriteElement(
                    new OpenXml.SharedStringItem(new OpenXml.Text(sharedString)));
            }

            writer.WriteEndElement();
        }
    }
}
