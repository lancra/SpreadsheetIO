using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Mapping.Extensions
{
    /// <summary>
    /// Provides an optional setting for a property header or body.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class OptionalPropertyMapOptionsExtension : IPropertyMapOptionsExtension
    {
        internal OptionalPropertyMapOptionsExtension(PropertyElementKind kind)
        {
            Kind = kind;
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<Type> AllowedTypes { get; } = Array.Empty<Type>();

        /// <summary>
        /// Gets the optional property element.
        /// </summary>
        public PropertyElementKind Kind { get; }
    }
}
