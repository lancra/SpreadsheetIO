using LanceC.SpreadsheetIO.Mapping2.Options.Registrations;

namespace LanceC.SpreadsheetIO.Mapping2.Options.Converters;

internal abstract class ResourceMapOptionConversionStrategy<TRegistration> :
    IMapOptionConversionStrategy<IResourceMapOptionRegistration, IResourceMapOption>
    where TRegistration : class, IResourceMapOptionRegistration
{
    public Type RegistrationType { get; } = typeof(TRegistration);

    public MapOptionConversionResult<IResourceMapOption> ConvertToOption(
        IResourceMapOptionRegistration registration,
        ResourceMapBuilder resourceMapBuilder)
        => ConvertToOption((TRegistration)registration, resourceMapBuilder);

    public abstract MapOptionConversionResult<IResourceMapOption> ConvertToOption(
        TRegistration registration,
        ResourceMapBuilder resourceMapBuilder);
}
