using LanceC.SpreadsheetIO.Mapping2.Options.Registrations;

namespace LanceC.SpreadsheetIO.Mapping2.Options.Converters;

/// <summary>
/// Defines a strategy for conversion from a mapping option registration to a mapping option.
/// </summary>
/// <typeparam name="TRegistration">The mapping option registration to convert from.</typeparam>
/// <typeparam name="TOption">The mapping option to convert to.</typeparam>
internal interface IMapOptionConversionStrategy<TRegistration, TOption> : IMapOptionConverter<TRegistration, TOption>
    where TRegistration : IMapOptionRegistration
    where TOption : IMapOption
{
    /// <summary>
    /// Gets the registration type to convert.
    /// </summary>
    Type RegistrationType { get; }
}
