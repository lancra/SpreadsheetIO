using System;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Reading.Failures;
using LanceC.SpreadsheetIO.Reading.Internal.Readers;

namespace LanceC.SpreadsheetIO.Reading.Internal
{
    internal class ReadingSpreadsheetPageOperation<TResource> : IReadingSpreadsheetPageOperation<TResource>
        where TResource : class
    {
        private readonly IWorksheetElementReader _worksheetReader;
        private readonly HeaderRowReadingResult<TResource> _headerRowResult;
        private readonly ResourceMap<TResource> _map;
        private readonly ISpreadsheetPageMapReader _spreadsheetPageMapReader;

        public ReadingSpreadsheetPageOperation(
            IWorksheetElementReader worksheetReader,
            HeaderRowReadingResult<TResource> headerRowResult,
            ResourceMap<TResource> map,
            ISpreadsheetPageMapReader spreadsheetPageMapReader)
        {
            _worksheetReader = worksheetReader;
            _headerRowResult = headerRowResult;
            _map = map;
            _spreadsheetPageMapReader = spreadsheetPageMapReader;
        }

        public HeaderReadingFailure? HeaderFailure => _headerRowResult.Failure;

        public ResourceReadingResult<TResource>? CurrentResult { get; private set; }

        public bool ReadNext()
        {
            if (_headerRowResult.Failure is not null)
            {
                return false;
            }

            var hasRow = _worksheetReader.ReadNextRow();
            CurrentResult = hasRow ? _spreadsheetPageMapReader.ReadBodyRow(_worksheetReader, _map, _headerRowResult.Headers) : default;
            return hasRow;
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
                _worksheetReader.Dispose();
            }
        }
    }
}
