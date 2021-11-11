namespace LanceC.SpreadsheetIO.Mapping;

/// <summary>
/// Defines a map to Excel for a resource.
/// </summary>
public interface IResourceMap
{
    /// <summary>
    /// Gets the resource type.
    /// </summary>
    Type ResourceType { get; }
}
