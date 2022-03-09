using LanceC.SpreadsheetIO.Mapping2;
using LanceC.SpreadsheetIO.Reading.Internal.Parsing;

namespace LanceC.SpreadsheetIO.Reading.Internal;

internal class ResourcePropertyValueResolver : IResourcePropertyValueResolver
{
    private readonly IResourcePropertyParser _resourcePropertyParser;
    private readonly IResourcePropertyDefaultValueResolver _resourcePropertyDefaultValueResolver;

    public ResourcePropertyValueResolver(
        IResourcePropertyParser resourcePropertyParser,
        IResourcePropertyDefaultValueResolver resourcePropertyDefaultValueResolver)
    {
        _resourcePropertyParser = resourcePropertyParser;
        _resourcePropertyDefaultValueResolver = resourcePropertyDefaultValueResolver;
    }

    public bool TryResolve(string cellValue, PropertyMap map, out object? value)
    {
        var parseResultKind = _resourcePropertyParser.TryParse(cellValue, map!, out var parseValue);
        var hasDefaultValue = _resourcePropertyDefaultValueResolver.TryResolve(map!, parseResultKind, out var defaultValue);

        value = !hasDefaultValue ? parseValue : defaultValue;
        var isResolved = parseResultKind.Valid || hasDefaultValue;
        return isResolved;
    }
}
