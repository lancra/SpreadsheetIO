using Ardalis.GuardClauses;
using LanceC.SpreadsheetIO.Mapping2.Options.Registrations;
using LanceC.SpreadsheetIO.Reading;

namespace LanceC.SpreadsheetIO.Mapping2.Options.Converters;

internal class DefaultValuePropertyMapOptionConversionStrategy :
    PropertyMapOptionConversionStrategy<DefaultValuePropertyMapOptionRegistration>
{
    public override MapOptionConversionResult<IPropertyMapOption> ConvertToOption(
        DefaultValuePropertyMapOptionRegistration registration,
        ResourceMapBuilder resourceMapBuilder)
    {
        Guard.Against.Null(registration, nameof(registration));
        Guard.Against.Null(resourceMapBuilder, nameof(resourceMapBuilder));

        var resolutions = registration.Resolutions;
        if (!resolutions.Any())
        {
            resolutions = resourceMapBuilder
                .TryGetRegistration<DefaultPropertyReadingResolutionsResourceMapOptionRegistration>(out var resolutionsRegistration)
                ? resolutionsRegistration!.Resolutions
                : ResourcePropertyDefaultReadingResolution.List;
        }

        var option = new DefaultValuePropertyMapOption(registration.Value, resolutions);
        return MapOptionConversionResult.Success<IPropertyMapOption>(registration, option);
    }
}
