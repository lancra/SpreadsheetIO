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
        private readonly IReadingSpreadsheetPageOperationFactory _operationFactory;

        public ReadingSpreadsheetPage(
            IWorksheetPartWrapper worksheetPart,
            IElementReaderFactory elementReaderFactory,
            IResourceMapManager resourceMapManager,
            ISpreadsheetPageMapReader spreadsheetPageMapReader,
            IReadingSpreadsheetPageOperationFactory operationFactory)
        {
            _worksheetPart = worksheetPart;
            _elementReaderFactory = elementReaderFactory;
            _resourceMapManager = resourceMapManager;
            _spreadsheetPageMapReader = spreadsheetPageMapReader;
            _operationFactory = operationFactory;
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

        public IReadingSpreadsheetPageOperation<TResource> StartRead<TResource>()
            where TResource : class
        {
            var map = _resourceMapManager.Single<TResource>();
            var operation = StartReadImpl(map);
            return operation;
        }

        public IReadingSpreadsheetPageOperation<TResource> StartRead<TResource, TResourceMap>()
            where TResource : class
            where TResourceMap : ResourceMap<TResource>
        {
            var map = _resourceMapManager.Single<TResource, TResourceMap>();
            var operation = StartReadImpl(map);
            return operation;
        }

        private ReadingResult<TResource> ReadAllImpl<TResource>(ResourceMap<TResource> map)
            where TResource : class
        {
            using var operation = StartReadImpl(map);
            if (operation.HeaderFailure is not null)
            {
                return new ReadingResult<TResource>(
                    Array.Empty<NumberedResource<TResource>>(),
                    operation.HeaderFailure,
                    Array.Empty<ResourceReadingFailure>());
            }

            var resources = new List<NumberedResource<TResource>>();
            var resourceFailures = new List<ResourceReadingFailure>();

            var shouldExitOnResourceReadingFailure = map.Options
                .HasExtension<ExitOnResourceReadingFailureResourceMapOptionsExtension>();

            while (operation.ReadNext())
            {
                if (operation.CurrentResult!.NumberedResource is not null)
                {
                    resources.Add(operation.CurrentResult.NumberedResource);
                }

                if (operation.CurrentResult!.Failure is not null)
                {
                    resourceFailures.Add(operation.CurrentResult.Failure);

                    if (shouldExitOnResourceReadingFailure)
                    {
                        break;
                    }
                }
            }

            var result = new ReadingResult<TResource>(resources, default, resourceFailures);
            return result;
        }

        private IReadingSpreadsheetPageOperation<TResource> StartReadImpl<TResource>(ResourceMap<TResource> map)
            where TResource : class
        {
            var worksheetReader = _elementReaderFactory.CreateWorksheetReader(_worksheetPart);
            var headerRowResult = _spreadsheetPageMapReader.ReadHeaderRow(worksheetReader, map);
            var operation = _operationFactory.Create(worksheetReader, headerRowResult, map);
            return operation;
        }
    }
}
