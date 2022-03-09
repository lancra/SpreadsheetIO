using Ardalis.GuardClauses;
using LanceC.SpreadsheetIO.Mapping2;
using LanceC.SpreadsheetIO.Mapping2.Options;

namespace LanceC.SpreadsheetIO.Reading.Internal;

internal class ResourcePropertyDefaultValueResolver : IResourcePropertyDefaultValueResolver
{
    public bool TryResolve(PropertyMap map, ResourcePropertyParseResultKind parseResultKind, out object? value)
    {
        Guard.Against.Null(map, nameof(map));
        Guard.Against.Null(parseResultKind, nameof(parseResultKind));

        value = null;

        if (parseResultKind == ResourcePropertyParseResultKind.Success)
        {
            return false;
        }

        var defaultValueOption = map.Options.Find<DefaultValuePropertyMapOption>();
        if (defaultValueOption is null)
        {
            return false;
        }

        if (defaultValueOption.Resolutions.Any(resolution => resolution.ParseResultKind == parseResultKind))
        {
            value = defaultValueOption.Value;
            return true;
        }

        return false;
    }
}
