using Ardalis.GuardClauses;
using LanceC.SpreadsheetIO.Mapping.Options;
using LanceC.SpreadsheetIO.Mapping.Options.Converters;
using LanceC.SpreadsheetIO.Mapping.Options.Registrations;
using LanceC.SpreadsheetIO.Mapping.Validation;

namespace LanceC.SpreadsheetIO.Mapping;

internal abstract class ResourceMapBuilder : IInternalResourceMapBuilder
{
    private readonly IResourceMapBuilderValidator _validator;
    private readonly List<IInternalPropertyMapBuilder> _propertyMapBuilders = new();
    private readonly IDictionary<Type, IResourceMapOptionRegistration> _registrations =
        new Dictionary<Type, IResourceMapOptionRegistration>();

    protected ResourceMapBuilder(Type resourceType, IResourceMapBuilderValidator validator)
    {
        ResourceType = resourceType;
        _validator = validator;
    }

    public Type ResourceType { get; }

    public IReadOnlyCollection<IInternalPropertyMapBuilder> Properties => _propertyMapBuilders;

    public ResourceMapResult Build(
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

        var failedValidationResults = _validator.Validate(this);

        if (!failedConversionResults.Any() &&
            !failedValidationResults.Any())
        {
            var map = new ResourceMap(
                ResourceType,
                new PropertyMapCollection(propertyMapResults.Select(result => result.Value!)
                    .ToList()),
                new MapOptions<IResourceMapOption>(options));
            return ResourceMapResult.Success(map);
        }
        else
        {
            var error = new ResourceMapError(failedConversionResults, failedValidationResults);
            return ResourceMapResult.Failure(error);
        }
    }

    public bool TryGetRegistration<TRegistration>(out TRegistration? registration)
        where TRegistration : class, IResourceMapOptionRegistration
    {
        var hasRegistration = _registrations.TryGetValue(typeof(TRegistration), out var resourceRegistration);
        registration = hasRegistration ? (TRegistration)resourceRegistration! : default;
        return hasRegistration;
    }

    protected void AddProperty(IInternalPropertyMapBuilder propertyMapBuilder)
        => _propertyMapBuilders.Add(propertyMapBuilder);

    protected void AddOrUpdateRegistration<TRegistration>(TRegistration registration)
        where TRegistration : class, IResourceMapOptionRegistration
    {
        _registrations[typeof(TRegistration)] = registration;
    }
}
