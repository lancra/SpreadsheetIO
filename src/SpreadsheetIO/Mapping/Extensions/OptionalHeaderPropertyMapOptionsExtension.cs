using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Mapping.Extensions
{
    [ExcludeFromCodeCoverage]
    internal class OptionalHeaderPropertyMapOptionsExtension : IPropertyMapOptionsExtension
    {
        public IReadOnlyCollection<Type> AllowedTypes { get; } = Array.Empty<Type>();
    }
}
