using System;
using System.Collections.Generic;

namespace LanceC.SpreadsheetIO.Shared.Internal.Indexers
{
    internal class StringIndexer : ReverseIndexerBase<string>, IStringIndexer
    {
        protected override IReadOnlyCollection<string> DefaultResources
            => Array.Empty<string>();

        public override uint this[string resource]
        {
            get
            {
                ValidateString(resource);
                return base[resource];
            }
        }

        public override uint Add(string resource)
        {
            ValidateString(resource);
            return base.Add(resource);
        }

        private static void ValidateString(string resource)
        {
            if (string.IsNullOrEmpty(resource))
            {
                throw new ArgumentException("A null or empty string cannot be indexed.");
            }
        }
    }
}
