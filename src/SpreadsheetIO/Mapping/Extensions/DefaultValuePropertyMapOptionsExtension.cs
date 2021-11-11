using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Reading;

namespace LanceC.SpreadsheetIO.Mapping.Extensions
{
    /// <summary>
    /// Provides a default value for a property.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DefaultValuePropertyMapOptionsExtension : IPropertyMapOptionsExtension
    {
        internal DefaultValuePropertyMapOptionsExtension(object value, params ResourcePropertyDefaultReadingResolution[] resolutions)
        {
            Value = value;
            Resolutions = resolutions;
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<Type> AllowedTypes { get; } = Array.Empty<Type>();

        /// <summary>
        /// Gets the default value.
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Gets the property default reading resolutions.
        /// </summary>
        public IReadOnlyCollection<ResourcePropertyDefaultReadingResolution> Resolutions { get; }
    }
}
