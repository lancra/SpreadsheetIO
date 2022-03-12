using LanceC.SpreadsheetIO.Mapping2.Options.Registrations;

namespace LanceC.SpreadsheetIO.Mapping2.Options.Converters;

internal abstract class PropertyMapOptionConversionStrategy<TRegistration> :
    IMapOptionConversionStrategy<IPropertyMapOptionRegistration, IPropertyMapOption>
    where TRegistration : class, IPropertyMapOptionRegistration
{
    public Type RegistrationType { get; } = typeof(TRegistration);

    public MapOptionConversionResult<IPropertyMapOption> ConvertToOption(
        IPropertyMapOptionRegistration registration,
        IInternalResourceMapBuilder resourceMapBuilder)
        => ConvertToOption((TRegistration)registration, resourceMapBuilder);

    public abstract MapOptionConversionResult<IPropertyMapOption> ConvertToOption(
        TRegistration registration,
        IInternalResourceMapBuilder resourceMapBuilder);
}
