namespace LanceC.SpreadsheetIO.Mapping.Options.Extensions;

internal static class ResourceMapOptionsExtensions
{
    public static uint GetHeaderRowNumber(this MapOptions<IResourceMapOption> options)
    {
        var option = options.Find<HeaderRowNumberResourceMapOption>();
        return option?.Number ?? 1U;
    }
}
