using System.Linq;
using Ardalis.GuardClauses;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Extensions;

namespace LanceC.SpreadsheetIO.Reading.Internal
{
    internal class ResourcePropertyDefaultValueResolver : IResourcePropertyDefaultValueResolver
    {
        public bool TryResolve<TResource>(
            PropertyMap<TResource> map,
            ResourcePropertyParseResultKind parseResultKind,
            out object? value)
            where TResource : class
        {
            Guard.Against.Null(map, nameof(map));
            Guard.Against.Null(parseResultKind, nameof(parseResultKind));

            value = null;

            if (parseResultKind == ResourcePropertyParseResultKind.Success)
            {
                return false;
            }

            var defaultValueExtension = map.Options.FindExtension<DefaultValuePropertyMapOptionsExtension>();
            if (defaultValueExtension is null)
            {
                return false;
            }

            if (defaultValueExtension.Resolutions.Any(resolution => resolution.ParseResultKind == parseResultKind))
            {
                value = defaultValueExtension.Value;
                return true;
            }

            return false;
        }
    }
}
