using System.Collections.Generic;
using System.Linq;

namespace LanceC.SpreadsheetIO.Reading.Internal.Properties
{
    internal class ResourcePropertyHeaderUsageTracker : IResourcePropertyHeaderUsageTracker
    {
        private readonly IDictionary<uint, bool> _headerUsages;

        public ResourcePropertyHeaderUsageTracker(IEnumerable<uint> columnNumbers)
        {
            _headerUsages = columnNumbers.ToDictionary(columnNumber => columnNumber, value => false);
        }

        public IReadOnlyCollection<uint> GetUnusedColumnNumbers()
            => _headerUsages.Where(keyValuePair => !keyValuePair.Value)
            .Select(keyValuePair => keyValuePair.Key)
            .ToArray();

        public void MarkAsUsed(uint columnNumber)
        {
            _headerUsages[columnNumber] = true;
        }
    }
}
