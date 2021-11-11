using Ardalis.SmartEnum;

namespace LanceC.SpreadsheetIO.Mapping;

/// <summary>
/// Represents the kind of element within a property.
/// </summary>
public class PropertyElementKind : SmartEnum<PropertyElementKind>
{
    /// <summary>
    /// Specifies all property elements.
    /// </summary>
    public static readonly PropertyElementKind All = new(1, "All");

    /// <summary>
    /// Specifies the header of a property.
    /// </summary>
    public static readonly PropertyElementKind Header = new(2, "Header");

    /// <summary>
    /// Specifies the body of a property.
    /// </summary>
    public static readonly PropertyElementKind Body = new(3, "Body");

    private PropertyElementKind(int id, string name)
        : base(name, id)
    {
    }
}
