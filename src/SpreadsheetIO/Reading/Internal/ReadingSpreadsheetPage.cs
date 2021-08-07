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
        private readonly ISpreadsheetPageMapReader _spreadsheetPageMapReader;

        public ReadingSpreadsheetPage(
            IWorksheetPartWrapper worksheetPart,
            IElementReaderFactory elementReaderFactory,
            IResourceMapManager resourceMapManager,
            ISpreadsheetPageMapReader spreadsheetPageMapReader)
        {
            _worksheetPart = worksheetPart;
            _elementReaderFactory = elementReaderFactory;
            _resourceMapManager = resourceMapManager;
            _spreadsheetPageMapReader = spreadsheetPageMapReader;
        }

        public ReadingResult<TResource> ReadAll<TResource>()
            where TResource : class
        {
            var map = _resourceMapManager.Single<TResource>();
            var result = ReadAllImpl(map);
            return result;
        }

        public ReadingResult<TResource> ReadAll<TResource, TResourceMap>()
            where TResource : class
            where TResourceMap : ResourceMap<TResource>
        {
            var map = _resourceMapManager.Single<TResource, TResourceMap>();
            var result = ReadAllImpl(map);
            return result;
        }

        private ReadingResult<TResource> ReadAllImpl<TResource>(ResourceMap<TResource> map)
            where TResource : class
        {
            using var worksheetReader = _elementReaderFactory.CreateWorksheetReader(_worksheetPart);

            var headerRowResult = _spreadsheetPageMapReader.ReadHeaderRow(worksheetReader, map);
            if (headerRowResult.Failure is not null)
            {
                return new ReadingResult<TResource>(
                    Array.Empty<NumberedResource<TResource>>(),
                    headerRowResult.Failure,
                    Array.Empty<ResourceReadingFailure>());
            }

            var resources = new List<NumberedResource<TResource>>();
            var resourceFailures = new List<ResourceReadingFailure>();

            var shouldExitOnResourceReadingFailure = map.Options
                .HasExtension<ExitOnResourceReadingFailureResourceMapOptionsExtension>();

            while (worksheetReader.ReadNextRow())
            {
                var bodyRowResult = _spreadsheetPageMapReader.ReadBodyRow(worksheetReader, map, headerRowResult.Headers);

                if (bodyRowResult.NumberedResource is not null)
                {
                    resources.Add(bodyRowResult.NumberedResource);
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
