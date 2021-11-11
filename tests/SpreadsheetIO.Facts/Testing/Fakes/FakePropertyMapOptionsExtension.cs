using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Facts.Testing.Fakes;

public class FakePropertyMapOptionsExtension : IPropertyMapOptionsExtension
{
    public IReadOnlyCollection<Type> AllowedTypes { get; } = Array.Empty<Type>();
}
