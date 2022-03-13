namespace LanceC.SpreadsheetIO.Mapping.Options.Registrations;

/// <summary>
/// Defines a mapping option registration for properties.
/// </summary>
public interface IPropertyMapOptionRegistration : IMapOptionRegistration
{
    /// <summary>
    /// Gets the property types that the option supports.
    /// </summary>
    IReadOnlyCollection<Type> AllowedTypes { get; }
}
