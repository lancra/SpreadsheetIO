using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using LanceC.SpreadsheetIO.Shared.Internal.Generators;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;
using LanceC.SpreadsheetIO.Styling;
using LanceC.SpreadsheetIO.Styling.Internal.Indexers;

namespace LanceC.SpreadsheetIO.Writing.Internal
{
    internal class WritingSpreadsheet : IWritingSpreadsheet
    {
        private readonly ISpreadsheetDocumentWrapper _spreadsheetDocument;
        private readonly IWritingSpreadsheetPageCollectionModifiable _spreadsheetPages;
        private readonly IStyleIndexer _styleIndexer;
        private readonly IStringIndexer _stringIndexer;
        private readonly IEnumerable<ISpreadsheetGenerator> _spreadsheetGenerators;

        public WritingSpreadsheet(
            ISpreadsheetDocumentWrapper spreadsheetDocument,
            IWritingSpreadsheetPageCollectionModifiable spreadsheetPages,
            IStyleIndexer styleIndexer,
            IStringIndexer stringIndexer,
            IEnumerable<ISpreadsheetGenerator> spreadsheetGenerators)
        {
            _spreadsheetDocument = spreadsheetDocument;
            _spreadsheetPages = spreadsheetPages;
            _styleIndexer = styleIndexer;
            _stringIndexer = stringIndexer;
            _spreadsheetGenerators = spreadsheetGenerators;
        }

        public IWritingSpreadsheetPageCollection Pages
            => _spreadsheetPages;

        public IWritingSpreadsheetPage AddPage(string name)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));

            var worksheetPart = _spreadsheetDocument.AddWorksheetPart(name);
            var spreadsheetPage = new WritingSpreadsheetPage(worksheetPart, _styleIndexer, _stringIndexer);

            _spreadsheetPages.Add(spreadsheetPage);

            return spreadsheetPage;
        }

        public IWritingSpreadsheet AddStyle(string name, Style style)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.Null(style, nameof(style));

            var styleKey = new IndexerKey(name, IndexerKeyKind.Custom);
            _styleIndexer.Add(styleKey, style);
            return this;
        }

        public IWritingSpreadsheet AddStyle(BuiltInExcelStyle style)
        {
            Guard.Against.Null(style, nameof(style));

            _styleIndexer.Add(style.IndexerKey, style.Style);
            return this;
        }

        public IWritingSpreadsheet AddStyle(BuiltInPackageStyle style)
        {
            Guard.Against.Null(style, nameof(style));

            _styleIndexer.Add(style.IndexerKey, style.Style);
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
                ExecuteGenerators();
                _spreadsheetPages.Dispose();
                _spreadsheetDocument.Dispose();
            }
        }

        private void ExecuteGenerators()
        {
            foreach (var spreadsheetGenerator in _spreadsheetGenerators)
            {
                spreadsheetGenerator.Generate(_spreadsheetDocument);
            }
        }
    }
}
