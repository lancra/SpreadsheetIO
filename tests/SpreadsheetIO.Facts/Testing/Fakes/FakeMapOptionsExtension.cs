using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Facts.Testing.Fakes
{
    public class FakeMapOptionsExtension : IResourceMapOptionsExtension, IPropertyMapOptionsExtension
    {
        public IReadOnlyCollection<Type> AllowedTypes { get; } = Array.Empty<Type>();
    }
}
