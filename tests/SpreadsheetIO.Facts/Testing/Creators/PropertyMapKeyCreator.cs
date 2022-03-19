using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Facts.Testing.Creators;

public static class PropertyMapKeyCreator
{
    public static PropertyMapKey Create(
        string name = "Name",
        uint? number = default,
        bool isNameIgnored = false,
        IReadOnlyCollection<string>? alternateNames = default)
        => new(
            name,
            number,
            isNameIgnored,
            alternateNames ?? Array.Empty<string>());
}
