using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Mapping;

/// <summary>
/// The exception that is thrown when the resource specified does not have an associated map.
/// </summary>
public class InvalidResourceMapException : Exception
{
    internal InvalidResourceMapException(Type resourceType, ResourceMapError error)
        : base(Messages.InvalidMapForResourceType(resourceType.Name))
    {
        ResourceType = resourceType;
        Error = error;
    }

    /// <summary>
    /// Gets the resource type.
    /// </summary>
    public Type ResourceType { get; }

    /// <summary>
    /// Gets the resource map error.
    /// </summary>
    public ResourceMapError Error { get; }
}
