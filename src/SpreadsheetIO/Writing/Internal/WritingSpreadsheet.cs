using Ardalis.GuardClauses;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Extensions;
using LanceC.SpreadsheetIO.Mapping.Internal;
using LanceC.SpreadsheetIO.Shared.Internal.Generators;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;
using LanceC.SpreadsheetIO.Styling;
using LanceC.SpreadsheetIO.Styling.Internal.Indexers;
using LanceC.SpreadsheetIO.Writing.Internal.Writers;

namespace LanceC.SpreadsheetIO.Writing.Internal
{
    internal class WritingSpreadsheet : IWritingSpreadsheet
    {
        private readonly ISpreadsheetDocumentWrapper _spreadsheetDocument;
        private readonly IWritingSpreadsheetPageCollectionModifiable _spreadsheetPages;
        private readonly IStyleIndexer _styleIndexer;
        private readonly IStringIndexer _stringIndexer;
        private readonly IEnumerable<ISpreadsheetGenerator> _spreadsheetGenerators;
        private readonly ISpreadsheetPageMapWriter _spreadsheetPageMapWriter;
        private readonly IResourceMapManager _resourceMapManager;

        public WritingSpreadsheet(
            ISpreadsheetDocumentWrapper spreadsheetDocument,
            IWritingSpreadsheetPageCollectionModifiable spreadsheetPages,
            IStyleIndexer styleIndexer,
            IStringIndexer stringIndexer,
            IEnumerable<ISpreadsheetGenerator> spreadsheetGenerators,
            ISpreadsheetPageMapWriter spreadsheetPageMapWriter,
            IResourceMapManager resourceMapManager)
        {
            _spreadsheetDocument = spreadsheetDocument;
            _spreadsheetPages = spreadsheetPages;
            _styleIndexer = styleIndexer;
            _stringIndexer = stringIndexer;
            _spreadsheetGenerators = spreadsheetGenerators;
            _spreadsheetPageMapWriter = spreadsheetPageMapWriter;
            _resourceMapManager = resourceMapManager;
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

        public IWritingSpreadsheetPage WritePage<TResource>(string name, IEnumerable<TResource> resources)
            where TResource : class
        {
            var map = _resourceMapManager.Single<TResource>();
            return WritePageImpl(name, resources, map);
        }

        public IWritingSpreadsheetPage WritePage<TResource, TResourceMap>(string name, IEnumerable<TResource> resources)
            where TResource : class
            where TResourceMap : ResourceMap<TResource>
        {
            var map = _resourceMapManager.Single<TResource, TResourceMap>();
            return WritePageImpl(name, resources, map);
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

        private IWritingSpreadsheetPage WritePageImpl<TResource>(
            string name,
            IEnumerable<TResource> resources,
            ResourceMap<TResource> map)
            where TResource : class
        {
            var spreadsheetPage = AddPage(name);

            foreach (var propertyMap in map.Properties)
            {
                var headerStyleExtension = propertyMap.Options.FindExtension<HeaderStyleMapOptionsExtension>();
                if (headerStyleExtension is not null)
                {
                    _styleIndexer.Add(headerStyleExtension.Key, headerStyleExtension.Style);
                }

                var bodyStyleExtension = propertyMap.Options.FindExtension<BodyStyleMapOptionsExtension>();
                if (bodyStyleExtension is not null)
                {
                    _styleIndexer.Add(bodyStyleExtension.Key, bodyStyleExtension.Style);
                }
            }

            _spreadsheetPageMapWriter.Write(spreadsheetPage, resources, map);

            return spreadsheetPage;
        }
    }
}
