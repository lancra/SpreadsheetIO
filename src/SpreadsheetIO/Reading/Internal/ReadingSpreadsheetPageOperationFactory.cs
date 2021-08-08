using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Reading.Internal.Readers;

namespace LanceC.SpreadsheetIO.Reading.Internal
{
    [ExcludeFromCodeCoverage]
    internal class ReadingSpreadsheetPageOperationFactory : IReadingSpreadsheetPageOperationFactory
    {
        private readonly ISpreadsheetPageMapReader _spreadsheetPageMapReader;

        public ReadingSpreadsheetPageOperationFactory(
            ISpreadsheetPageMapReader spreadsheetPageMapReader)
        {
            _spreadsheetPageMapReader = spreadsheetPageMapReader;
        }

        public IReadingSpreadsheetPageOperation<TResource> Create<TResource>(
            IWorksheetElementReader worksheetReader,
            HeaderRowReadingResult<TResource> headerRowResult,
            ResourceMap<TResource> map)
            where TResource : class
            => new ReadingSpreadsheetPageOperation<TResource>(
                worksheetReader,
                headerRowResult,
                map,
                _spreadsheetPageMapReader);
    }
}
