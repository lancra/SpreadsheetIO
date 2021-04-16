using System;
using System.Collections.Generic;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Extensions;
using LanceC.SpreadsheetIO.Mapping.Internal;
using LanceC.SpreadsheetIO.Reading.Failures;
using LanceC.SpreadsheetIO.Reading.Internal.Readers;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;

namespace LanceC.SpreadsheetIO.Reading.Internal
{
    internal class ReadingSpreadsheetPage : IReadingSpreadsheetPage
    {
        private readonly IWorksheetPartWrapper _worksheetPart;
        private readonly IElementReaderFactory _elementReaderFactory;
        private readonly IResourceMapManager _resourceMapManager;
        private readonly ISpreadsheetPageReader _spreadsheetPageReader;

        public ReadingSpreadsheetPage(
            IWorksheetPartWrapper worksheetPart,
            IElementReaderFactory elementReaderFactory,
            IResourceMapManager resourceMapManager,
            ISpreadsheetPageReader spreadsheetPageReader)
        {
            _worksheetPart = worksheetPart;
            _elementReaderFactory = elementReaderFactory;
            _resourceMapManager = resourceMapManager;
            _spreadsheetPageReader = spreadsheetPageReader;
        }

        public ReadingResult<TResource> Read<TResource>()
            where TResource : class
        {
            var map = _resourceMapManager.Single<TResource>();
            var result = ReadImpl(map);
            return result;
        }

        public ReadingResult<TResource> Read<TResource, TResourceMap>()
            where TResource : class
            where TResourceMap : ResourceMap<TResource>
        {
            var map = _resourceMapManager.Single<TResource, TResourceMap>();
            var result = ReadImpl(map);
            return result;
        }

        private ReadingResult<TResource> ReadImpl<TResource>(ResourceMap<TResource> map)
            where TResource : class
        {
            using var worksheetReader = _elementReaderFactory.CreateWorksheetReader(_worksheetPart);

            var headerRowResult = _spreadsheetPageReader.ReadHeaderRow(worksheetReader, map);
            if (headerRowResult.Failure is not null)
            {
                return new ReadingResult<TResource>(
                    Array.Empty<TResource>(),
                    headerRowResult.Failure,
                    Array.Empty<ResourceReadingFailure>());
            }

            var resources = new List<TResource>();
            var resourceFailures = new List<ResourceReadingFailure>();

            var shouldExitOnResourceReadingFailure = map.Options
                .HasExtension<ExitOnResourceReadingFailureResourceMapOptionsExtension>();

            while (worksheetReader.ReadNextRow())
            {
                var bodyRowResult = _spreadsheetPageReader.ReadBodyRow(worksheetReader, map, headerRowResult.Headers);

                if (bodyRowResult.Resource is not null)
                {
                    resources.Add(bodyRowResult.Resource);
                }

                if (bodyRowResult.Failure is not null)
                {
                    resourceFailures.Add(bodyRowResult.Failure);

                    if (shouldExitOnResourceReadingFailure)
                    {
                        break;
                    }
                }
            }

            var result = new ReadingResult<TResource>(resources, default, resourceFailures);
            return result;
        }
    }
}
