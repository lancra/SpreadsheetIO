using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Mapping.Internal.Extensions
{
    [ExcludeFromCodeCoverage]
    internal class OptionalPropertyMapOptionsExtension : IPropertyMapOptionsExtension
    {
        public OptionalPropertyMapOptionsExtension(PropertyElementKind kind)
        {
            Kind = kind;
        }

        public IReadOnlyCollection<Type> AllowedTypes { get; } = Array.Empty<Type>();

        public PropertyElementKind Kind { get; }
    }
}
