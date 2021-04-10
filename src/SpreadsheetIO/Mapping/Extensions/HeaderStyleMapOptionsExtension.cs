using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Styling;

namespace LanceC.SpreadsheetIO.Mapping.Extensions
{
    [ExcludeFromCodeCoverage]
    internal class HeaderStyleMapOptionsExtension : IResourceMapOptionsExtension, IPropertyMapOptionsExtension
    {
        public HeaderStyleMapOptionsExtension(Style style)
        {
            Style = style;
        }

        public IReadOnlyCollection<Type> AllowedTypes { get; } = Array.Empty<Type>();

        public Style Style { get; }
    }
}
