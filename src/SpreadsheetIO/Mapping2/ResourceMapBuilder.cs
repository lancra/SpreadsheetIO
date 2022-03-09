using Ardalis.GuardClauses;
using LanceC.SpreadsheetIO.Mapping2.Options;
using LanceC.SpreadsheetIO.Mapping2.Options.Converters;
using LanceC.SpreadsheetIO.Mapping2.Options.Registrations;

namespace LanceC.SpreadsheetIO.Mapping2;

/// <summary>
/// Provides the builder for generating a resource map.
/// </summary>
public abstract class ResourceMapBuilder
{
    private readonly List<PropertyMapBuilder> _propertyMapBuilders = new();
    private readonly IDictionary<Type, IResourceMapOptionRegistration> _registrations =
        new Dictionary<Type, IResourceMapOptionRegistration>();

    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceMapBuilder"/> class.
    /// </summary>
    /// <param name="resourceType">The type of resource to map.</param>
    protected ResourceMapBuilder(Type resourceType)
    {
        ResourceType = resourceType;
    }

    /// <summary>
    /// Gets the type of resource to map.
    /// </summary>
    public Type ResourceType { get; }

    internal IReadOnlyCollection<PropertyMapBuilder> Properties => _propertyMapBuilders;

    internal ResourceMapResult Build(
        IMapOptionConverter<IResourceMapOptionRegistration, IResourceMapOption> resourceOptionConverter,
        IMapOptionConverter<IPropertyMapOptionRegistration, IPropertyMapOption> propertyOptionConverter)
    {
        Guard.Against.Null(resourceOptionConverter, nameof(resourceOptionConverter));
        Guard.Against.Null(propertyOptionConverter, nameof(propertyOptionConverter));

        var resourceRegistrations = new List<IResourceMapOptionRegistration>();
        foreach (var registration in _registrations.Values)
        {
            if (registration is IPropertyMapOptionRegistration propertyRegistration)
            {
                foreach (var propertyMapBuilder in _propertyMapBuilders)
                {
                    propertyMapBuilder.TryAddRegistration(propertyRegistration);
                }
            }
            else
            {
                resourceRegistrations.Add(registration);
            }
        }

        var propertyMapResults = new List<PropertyMapResult>();
        foreach (var propertyMapBuilder in _propertyMapBuilders)
        {
            propertyMapResults.Add(propertyMapBuilder.Build(propertyOptionConverter, this));
        }

        var options = new Dictionary<Type, IResourceMapOption>();
        var failedConversionResults = propertyMapResults.Where(result => !result.IsValid)
            .SelectMany(result => result.Error!.Conversions)
            .ToList();

        foreach (var registration in resourceRegistrations)
        {
            var conversionResult = resourceOptionConverter.ConvertToOption(registration, this);
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
            var map = new ResourceMap(
                ResourceType,
                propertyMapResults.Select(result => result.Value!)
                    .ToArray(),
                new MapOptions<IResourceMapOption>(options));
            return ResourceMapResult.Success(map);
        }
        else
        {
            var error = new ResourceMapError(failedConversionResults);
            return ResourceMapResult.Failure(error);
        }
    }

    internal bool TryGetRegistration<TRegistration>(out TRegistration? registration)
        where TRegistration : class, IResourceMapOptionRegistration
    {
        var hasRegistration = _registrations.TryGetValue(typeof(TRegistration), out var resourceRegistration);
        registration = hasRegistration ? (TRegistration)resourceRegistration! : default;
        return hasRegistration;
    }

    /// <summary>
    /// Adds a property map builder to the resource map builder.
    /// </summary>
    /// <param name="propertyMapBuilder">The builder to add.</param>
    protected void AddProperty(PropertyMapBuilder propertyMapBuilder)
        => _propertyMapBuilders.Add(propertyMapBuilder);

    /// <summary>
    /// Adds or updates a map option registration.
    /// </summary>
    /// <typeparam name="TRegistration">The type of map option registration.</typeparam>
    /// <param name="registration">The map option registration to add or update.</param>
    protected void AddOrUpdateRegistration<TRegistration>(TRegistration registration)
        where TRegistration : class, IResourceMapOptionRegistration
    {
        _registrations[typeof(TRegistration)] = registration;
    }
}
