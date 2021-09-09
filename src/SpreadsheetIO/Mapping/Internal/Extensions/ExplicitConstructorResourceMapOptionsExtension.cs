using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Mapping.Internal.Extensions
{
    [ExcludeFromCodeCoverage]
    internal class ExplicitConstructorResourceMapOptionsExtension : IResourceMapOptionsExtension
    {
        public ExplicitConstructorResourceMapOptionsExtension(params string[] propertyNames)
        {
            PropertyNames = propertyNames;
        }

        public IReadOnlyCollection<string> PropertyNames { get; }
    }
}
