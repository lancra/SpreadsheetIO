using LanceC.SpreadsheetIO.Shared;

namespace LanceC.SpreadsheetIO.Mapping.Options.Extensions;

internal static class PropertyMapOptionsExtensions
{
    public static bool IsRequired(this MapOptions<IPropertyMapOption> options, PropertyElementKind elementKind)
    {
        var option = options.Find<OptionalPropertyMapOption>();
        return option is null ||
            (option.Kind != PropertyElementKind.All && option.Kind != elementKind);
    }

    public static CellDateKind GetDateKind(this MapOptions<IPropertyMapOption> options)
    {
        var option = options.Find<DateKindMapOption>();
        return option?.DateKind ?? CellDateKind.Number;
    }

    public static CellStringKind GetStringKind(this MapOptions<IPropertyMapOption> options)
    {
        var option = options.Find<StringKindMapOption>();
        return option?.StringKind ?? CellStringKind.SharedString;
    }
}
