using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Mapping.Extensions
{
    /// <summary>
    /// Provides an overridden header row for a resource.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class HeaderRowNumberResourceMapOptionsExtension : IResourceMapOptionsExtension
    {
        internal HeaderRowNumberResourceMapOptionsExtension(uint number)
        {
            Number = number;
        }

        /// <summary>
        /// Gets the header row number.
        /// </summary>
        public uint Number { get; }
    }
}
