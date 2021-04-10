using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Reading;

namespace LanceC.SpreadsheetIO.Mapping.Extensions
{
    [ExcludeFromCodeCoverage]
    internal class DefaultValuePropertyMapOptionsExtension : IPropertyMapOptionsExtension
    {
        public DefaultValuePropertyMapOptionsExtension(object value, params ResourcePropertyDefaultReadingResolution[] resolutions)
        {
            Value = value;
            Resolutions = resolutions;
        }

        public IReadOnlyCollection<Type> AllowedTypes { get; } = Array.Empty<Type>();

        public object Value { get; }

        public IReadOnlyCollection<ResourcePropertyDefaultReadingResolution> Resolutions { get; }
    }
}
