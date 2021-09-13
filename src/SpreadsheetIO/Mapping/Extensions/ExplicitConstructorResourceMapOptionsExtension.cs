using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Mapping.Extensions
{
    /// <summary>
    /// Provides an explicitly-defined constructor for creating a resource.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ExplicitConstructorResourceMapOptionsExtension : IResourceMapOptionsExtension
    {
        internal ExplicitConstructorResourceMapOptionsExtension(params string[] propertyNames)
        {
            PropertyNames = propertyNames;
        }

        /// <summary>
        /// Gets the constructor property names.
        /// </summary>
        public IReadOnlyCollection<string> PropertyNames { get; }
    }
}
