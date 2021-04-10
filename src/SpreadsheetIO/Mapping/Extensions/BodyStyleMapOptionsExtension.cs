using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Styling;

namespace LanceC.SpreadsheetIO.Mapping.Extensions
{
    [ExcludeFromCodeCoverage]
    internal class BodyStyleMapOptionsExtension : IResourceMapOptionsExtension, IPropertyMapOptionsExtension
    {
        public BodyStyleMapOptionsExtension(Style style)
        {
            Style = style;
        }

        public IReadOnlyCollection<Type> AllowedTypes { get; } = Array.Empty<Type>();

        public Style Style { get; }
    }
}
