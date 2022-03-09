using System.Reflection;
using Ardalis.GuardClauses;
using LanceC.SpreadsheetIO.Mapping2.Options;
using LanceC.SpreadsheetIO.Mapping2.Options.Converters;
using LanceC.SpreadsheetIO.Mapping2.Options.Registrations;
using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Mapping2;

/// <summary>
/// Provides the builder for generating a property map.
/// </summary>
public abstract class PropertyMapBuilder
{
    private readonly IDictionary<Type, IPropertyMapOptionRegistration> _registrations =
        new Dictionary<Type, IPropertyMapOptionRegistration>();

    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyMapBuilder"/> class.
    /// </summary>
    /// <param name="propertyInfo">The information about the resource property.</param>
    protected PropertyMapBuilder(PropertyInfo propertyInfo)
    {
        PropertyInfo = propertyInfo;
        KeyBuilder = new PropertyMapKeyBuilder(propertyInfo);
    }

    /// <summary>
    /// Gets the information about the resource property.
    /// </summary>
    public PropertyInfo PropertyInfo { get; }

    /// <summary>
    /// Gets the unique key builder.
    /// </summary>
    public PropertyMapKeyBuilder KeyBuilder { get; }

    internal PropertyMapResult Build(
        IMapOptionConverter<IPropertyMapOptionRegistration, IPropertyMapOption> propertyOptionConverter,
        ResourceMapBuilder resourceMapBuilder)
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

    internal bool TryAddRegistration(IPropertyMapOptionRegistration registration)
    {
        Guard.Against.Null(registration, nameof(registration));

        if (!IsRegistrationAllowed(registration))
        {
            return false;
        }

        return _registrations.TryAdd(registration.GetType(), registration);
    }

    internal bool TryGetRegistration<TRegistration>(out TRegistration? registration)
        where TRegistration : class, IPropertyMapOptionRegistration
    {
        var hasRegistration = _registrations.TryGetValue(typeof(TRegistration), out var propertyRegistration);
        registration = hasRegistration ? (TRegistration)propertyRegistration! : default;
        return hasRegistration;
    }

    /// <summary>
    /// Adds or updates a map option registration.
    /// </summary>
    /// <typeparam name="TRegistration">The type of map option registration.</typeparam>
    /// <param name="registration">The map option registration to add or update.</param>
    protected void AddOrUpdateRegistration<TRegistration>(TRegistration registration)
        where TRegistration : class, IPropertyMapOptionRegistration
    {
        Guard.Against.Null(registration, nameof(registration));

        if (!IsRegistrationAllowed(registration))
        {
            throw new InvalidOperationException(
                Messages.OptionsExtensionNotAllowedForType(registration.GetType().Name, PropertyInfo.PropertyType.Name));
        }

        _registrations[typeof(TRegistration)] = registration;
    }

    private bool IsRegistrationAllowed(IPropertyMapOptionRegistration registration)
        => !registration.AllowedTypes.Any() ||
        registration.AllowedTypes.Any(t =>
            t == PropertyInfo.PropertyType ||
            t == Nullable.GetUnderlyingType(PropertyInfo.PropertyType));
}
