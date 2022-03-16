using LanceC.SpreadsheetIO.Mapping.Options.Registrations;

namespace LanceC.SpreadsheetIO.Mapping.Options.Converters;

/// <summary>
/// Defines the converter for generating a mapping option from a mapping option registration.
/// </summary>
/// <typeparam name="TRegistration">The mapping option registration to convert from.</typeparam>
/// <typeparam name="TOption">The mapping option to convert to.</typeparam>
internal interface IMapOptionConverter<TRegistration, TOption>
    where TRegistration : IMapOptionRegistration
    where TOption : IMapOption
{
    /// <summary>
    /// Converts a registration to an option.
    /// </summary>
    /// <param name="registration">The registration to convert.</param>
    /// <param name="resourceMapBuilder">The resource map builder containing the registration.</param>
    /// <returns>The option conversion result.</returns>
    MapOptionConversionResult<TOption> ConvertToOption(
        TRegistration registration,
        IInternalResourceMapBuilder resourceMapBuilder);
}
