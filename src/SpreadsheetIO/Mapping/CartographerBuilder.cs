using System.Reflection;
using Ardalis.GuardClauses;
using LanceC.SpreadsheetIO.Mapping.Options;
using LanceC.SpreadsheetIO.Mapping.Options.Converters;
using LanceC.SpreadsheetIO.Mapping.Options.Registrations;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;

namespace LanceC.SpreadsheetIO.Mapping;

internal class CartographerBuilder : ICartographerBuilder
{
    private readonly IMapBuilderFactory _mapBuilderFactory;
    private readonly IMapOptionConverter<IResourceMapOptionRegistration, IResourceMapOption> _resourceOptionConverter;
    private readonly IMapOptionConverter<IPropertyMapOptionRegistration, IPropertyMapOption> _propertyOptionConverter;
    private readonly IAssemblyWrapperFactory _assemblyFactory;

    public CartographerBuilder(
        IMapBuilderFactory mapBuilderFactory,
        IMapOptionConverter<IResourceMapOptionRegistration, IResourceMapOption> resourceOptionConverter,
        IMapOptionConverter<IPropertyMapOptionRegistration, IPropertyMapOption> propertyOptionConverter,
        IAssemblyWrapperFactory assemblyFactory)
    {
        _mapBuilderFactory = mapBuilderFactory;
        _resourceOptionConverter = resourceOptionConverter;
        _propertyOptionConverter = propertyOptionConverter;
        _assemblyFactory = assemblyFactory;
    }

    public IDictionary<Type, ResourceMapResult> ResourceMaps { get; } = new Dictionary<Type, ResourceMapResult>();

    public ICartographer Build()
        => new Cartographer(ResourceMaps);

    public ICartographerBuilder ApplyConfiguration<TResource>(IResourceMapConfiguration<TResource> configuration)
        where TResource : class
    {
        Guard.Against.Null(configuration, nameof(configuration));

        return Configure<TResource>(configuration.Configure);
    }

    public ICartographerBuilder ApplyConfigurationsFromAssembly(Assembly assembly)
    {
        Guard.Against.Null(assembly, nameof(assembly));

        return ApplyConfigurationsFromAssemblyImpl(_assemblyFactory.Create(assembly));
    }

    public ICartographerBuilder ApplyConfigurationsFromAssembly(Type markerType)
    {
        Guard.Against.Null(markerType, nameof(markerType));

        return ApplyConfigurationsFromAssemblyImpl(_assemblyFactory.Create(markerType));
    }

    public ICartographerBuilder Configure<TResource>(Action<IResourceMapBuilder<TResource>> builderAction)
        where TResource : class
    {
        Guard.Against.Null(builderAction, nameof(builderAction));

        var resourceMapBuilder = _mapBuilderFactory.CreateForResource<TResource>();
        builderAction(resourceMapBuilder);

        ResourceMaps.Add(
            resourceMapBuilder.ResourceType,
            resourceMapBuilder.Build(_resourceOptionConverter, _propertyOptionConverter));
        return this;
    }

    private ICartographerBuilder ApplyConfigurationsFromAssemblyImpl(IAssemblyWrapper assembly)
    {
        var applyConfigurationMethod = GetType()
            .GetMethod(nameof(ApplyConfiguration))!;

        foreach (var type in assembly.GetConstructibleTypes()
            .OrderBy(t => t.FullName))
        {
            if (type.GetConstructor(Type.EmptyTypes) is null)
            {
                continue;
            }

            foreach (var typeInterface in type.GetInterfaces())
            {
                if (!typeInterface.IsGenericType)
                {
                    continue;
                }

                if (typeInterface.GetGenericTypeDefinition() == typeof(IResourceMapConfiguration<>))
                {
                    var target = applyConfigurationMethod.MakeGenericMethod(typeInterface.GenericTypeArguments[0]);
                    target.Invoke(this, new[] { Activator.CreateInstance(type), });
                }
            }
        }

        return this;
    }
}
