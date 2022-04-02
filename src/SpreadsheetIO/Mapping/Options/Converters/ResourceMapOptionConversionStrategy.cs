using LanceC.SpreadsheetIO.Mapping.Builders;
using LanceC.SpreadsheetIO.Mapping.Options.Registrations;

namespace LanceC.SpreadsheetIO.Mapping.Options.Converters;

internal abstract class ResourceMapOptionConversionStrategy<TRegistration> :
    IMapOptionConversionStrategy<IResourceMapOptionRegistration, IResourceMapOption>
    where TRegistration : class, IResourceMapOptionRegistration
{
    public Type RegistrationType { get; } = typeof(TRegistration);

    public MapOptionConversionResult<IResourceMapOption> ConvertToOption(
        IResourceMapOptionRegistration registration,
        IInternalResourceMapBuilder resourceMapBuilder)
        => ConvertToOption((TRegistration)registration, resourceMapBuilder);

    public abstract MapOptionConversionResult<IResourceMapOption> ConvertToOption(
        TRegistration registration,
        IInternalResourceMapBuilder resourceMapBuilder);
}
