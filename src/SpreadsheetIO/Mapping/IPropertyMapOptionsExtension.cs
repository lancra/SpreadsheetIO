namespace LanceC.SpreadsheetIO.Mapping
{
    /// <summary>
    /// Defines an extension for property map options.
    /// </summary>
    public interface IPropertyMapOptionsExtension
    {
        /// <summary>
        /// Gets the property types that the extension supports.
        /// </summary>
        IReadOnlyCollection<Type> AllowedTypes { get; }
    }
}
