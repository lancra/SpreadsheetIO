using System.Reflection;
using Ardalis.GuardClauses;
using LanceC.SpreadsheetIO.Mapping.Options;
using LanceC.SpreadsheetIO.Mapping.Options.Converters;
using LanceC.SpreadsheetIO.Mapping.Options.Registrations;
using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Mapping;

internal abstract class PropertyMapBuilder : IInternalPropertyMapBuilder
{
    private readonly IDictionary<Type, IPropertyMapOptionRegistration> _registrations =
        new Dictionary<Type, IPropertyMapOptionRegistration>();

    protected PropertyMapBuilder(PropertyInfo propertyInfo, IMapBuilderFactory mapBuilderFactory)
    {
        PropertyInfo = propertyInfo;
        KeyBuilder = mapBuilderFactory.CreateForPropertyKey(propertyInfo);
    }

    public PropertyInfo PropertyInfo { get; }

    public IInternalPropertyMapKeyBuilder KeyBuilder { get; }

    public PropertyMapResult Build(
        IMapOptionConverter<IPropertyMapOptionRegistration, IPropertyMapOption> propertyOptionConverter,
        IInternalResourceMapBuilder resourceMapBuilder)
    {
        Guard.Against.Null(propertyOptionConverter, nameof(propertyOptionConverter));
        Guard.Against.Null(resourceMapBuilder, nameof(resourceMapBuilder));

        var options = new Dictionary<Type, IPropertyMapOption>();
        var failedConversionResults = new List<MapOptionConversionResult>();

        foreach (var registration in _registrations.Values)
        {
            var conversionResult = propertyOptionConverter.ConvertToOption(registration, resourceMapBuilder);
            if (conversionResult.IsValid)
            {
                options.Add(conversionResult.Option!.GetType(), conversionResult.Option);
            }
            else
            {
                failedConversionResults.Add(conversionResult);
            }
        }

        if (!failedConversionResults.Any())
        {
            var map = new PropertyMap(PropertyInfo, KeyBuilder.Key, new MapOptions<IPropertyMapOption>(options));
            return PropertyMapResult.Success(map);
        }
        else
        {
            var error = new PropertyMapError(failedConversionResults);
            return PropertyMapResult.Failure(error);
        }
    }

    public bool TryAddRegistration(IPropertyMapOptionRegistration registration)
    {
        Guard.Against.Null(registration, nameof(registration));

        if (!IsRegistrationAllowed(registration))
        {
            return false;
        }

        return _registrations.TryAdd(registration.GetType(), registration);
    }

    public bool TryGetRegistration<TRegistration>(out TRegistration? registration)
        where TRegistration : class, IPropertyMapOptionRegistration
    {
        var hasRegistration = _registrations.TryGetValue(typeof(TRegistration), out var propertyRegistration);
        registration = hasRegistration ? (TRegistration)propertyRegistration! : default;
        return hasRegistration;
    }

    protected void AddOrUpdateRegistration<TRegistration>(TRegistration registration)
        where TRegistration : class, IPropertyMapOptionRegistration
    {
        Guard.Against.Null(registration, nameof(registration));

        if (!IsRegistrationAllowed(registration))
        {
            throw new InvalidOperationException(
                Messages.MapOptionRegistrationNotAllowedForType(registration.GetType(), PropertyInfo.PropertyType.Name));
        }

        _registrations[typeof(TRegistration)] = registration;
    }

    private bool IsRegistrationAllowed(IPropertyMapOptionRegistration registration)
        => !registration.AllowedTypes.Any() ||
        registration.AllowedTypes.Any(t =>
            t == PropertyInfo.PropertyType ||
            t == Nullable.GetUnderlyingType(PropertyInfo.PropertyType));
}
