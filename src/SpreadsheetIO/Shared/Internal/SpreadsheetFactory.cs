using System.IO;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;
using LanceC.SpreadsheetIO.Styling.Internal.Indexers;
using LanceC.SpreadsheetIO.Writing;
using LanceC.SpreadsheetIO.Writing.Internal;

namespace LanceC.SpreadsheetIO.Shared.Internal
{
    internal class SpreadsheetFactory : ISpreadsheetFactory
    {
        private readonly ISpreadsheetDocumentWrapperFactory _spreadsheetDocumentFactory;
        private readonly IStyleIndexer _styleIndexer;
        private readonly IStringIndexer _stringIndexer;

        public SpreadsheetFactory(
            ISpreadsheetDocumentWrapperFactory spreadsheetDocumentFactory,
            IStyleIndexer styleIndexer,
            IStringIndexer stringIndexer)
        {
            _spreadsheetDocumentFactory = spreadsheetDocumentFactory;
            _styleIndexer = styleIndexer;
            _stringIndexer = stringIndexer;
        }

        public IWritingSpreadsheet Create(string path)
        {
            var spreadsheetDocument = _spreadsheetDocumentFactory.Create(path);
            var spreadsheet = new WritingSpreadsheet(
                spreadsheetDocument,
                CreateSpreadsheetPageCollection(),
                _styleIndexer,
                _stringIndexer);

            return spreadsheet;
        }

        public IWritingSpreadsheet Create(Stream stream)
        {
            var spreadsheetDocument = _spreadsheetDocumentFactory.Create(stream);
            var spreadsheet = new WritingSpreadsheet(
                spreadsheetDocument,
                CreateSpreadsheetPageCollection(),
                _styleIndexer,
                _stringIndexer);

            return spreadsheet;
        }

        private static IWritingSpreadsheetPageCollectionModifiable CreateSpreadsheetPageCollection()
            => new WritingSpreadsheetPageCollection();
    }
}
