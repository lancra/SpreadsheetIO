using LanceC.SpreadsheetIO.Mapping.Options.Registrations;

namespace LanceC.SpreadsheetIO.Facts.Testing.Fakes;

public class FakePropertyMapOptionRegistration : IPropertyMapOptionRegistration
{
    public IReadOnlyCollection<Type> AllowedTypes { get; } = Array.Empty<Type>();
}
