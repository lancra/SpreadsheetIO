using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Mapping2.Options.Registrations;

/// <summary>
/// Provides an explicitly-defined constructor for creating a resource.
/// </summary>
[ExcludeFromCodeCoverage]
internal class ExplicitConstructorResourceMapOptionRegistration : IResourceMapOptionRegistration
{
    public ExplicitConstructorResourceMapOptionRegistration(params string[] propertyNames)
    {
        PropertyNames = propertyNames;
    }

    /// <summary>
    /// Gets the constructor property names.
    /// </summary>
    public IReadOnlyCollection<string> PropertyNames { get; }
}
