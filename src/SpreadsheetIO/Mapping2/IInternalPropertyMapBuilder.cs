using System.Reflection;
using LanceC.SpreadsheetIO.Mapping2.Options;
using LanceC.SpreadsheetIO.Mapping2.Options.Converters;
using LanceC.SpreadsheetIO.Mapping2.Options.Registrations;

namespace LanceC.SpreadsheetIO.Mapping2;

/// <summary>
/// Defines the internal builder for generating a property map.
/// </summary>
internal interface IInternalPropertyMapBuilder
{
    /// <summary>
    /// Gets the information about the resource property.
    /// </summary>
    PropertyInfo PropertyInfo { get; }

    /// <summary>
    /// Gets the property map key builder.
    /// </summary>
    IInternalPropertyMapKeyBuilder KeyBuilder { get; }

    /// <summary>
    /// Builds a property map.
    /// </summary>
    /// <param name="propertyOptionConverter">The converter for property map options.</param>
    /// <param name="resourceMapBuilder">The parent resource map builder.</param>
    /// <returns>The property map build result.</returns>
    PropertyMapResult Build(
        IMapOptionConverter<IPropertyMapOptionRegistration, IPropertyMapOption> propertyOptionConverter,
        IInternalResourceMapBuilder resourceMapBuilder);

    /// <summary>
    /// Attempts to add a property map option registration.
    /// </summary>
    /// <param name="registration">The property map option registration to add.</param>
    /// <returns><c>true</c> if the property map option registration was added; otherwise, <c>false</c>.</returns>
    bool TryAddRegistration(IPropertyMapOptionRegistration registration);

    /// <summary>
    /// Attempts to retrieve a property map option registration.
    /// </summary>
    /// <typeparam name="TRegistration">The type of property map option registration.</typeparam>
    /// <param name="registration">The property map option registration when one was found; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if the property map option registration was found; otherwise, <c>false</c>.</returns>
    bool TryGetRegistration<TRegistration>(out TRegistration? registration)
        where TRegistration : class, IPropertyMapOptionRegistration;
}
