using LanceC.SpreadsheetIO.Mapping.Options;
using LanceC.SpreadsheetIO.Mapping.Options.Converters;
using LanceC.SpreadsheetIO.Mapping.Options.Registrations;

namespace LanceC.SpreadsheetIO.Mapping.Builders;

/// <summary>
/// Defines the internal builder for generating a resource map.
/// </summary>
internal interface IInternalResourceMapBuilder
{
    /// <summary>
    /// Gets the type of resource to map.
    /// </summary>
    Type ResourceType { get; }

    /// <summary>
    /// Gets the property map builders.
    /// </summary>
    IReadOnlyCollection<IInternalPropertyMapBuilder> Properties { get; }

    /// <summary>
    /// Builds a resource map.
    /// </summary>
    /// <param name="resourceOptionConverter">The converter for resource map options.</param>
    /// <param name="propertyOptionConverter">The converter for property map options.</param>
    /// <returns>The resource map build result.</returns>
    ResourceMapResult Build(
        IMapOptionConverter<IResourceMapOptionRegistration, IResourceMapOption> resourceOptionConverter,
        IMapOptionConverter<IPropertyMapOptionRegistration, IPropertyMapOption> propertyOptionConverter);

    /// <summary>
    /// Attempts to retrieve a resource map option registration.
    /// </summary>
    /// <typeparam name="TRegistration">The type of resource map option registration.</typeparam>
    /// <param name="registration">The resource map option registration when one was found; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if the resource map option registration was found; otherwise, <c>false</c>.</returns>
    bool TryGetRegistration<TRegistration>(out TRegistration? registration)
        where TRegistration : class, IResourceMapOptionRegistration;
}
