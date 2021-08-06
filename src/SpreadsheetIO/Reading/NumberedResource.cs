using System.Diagnostics.CodeAnalysis;

namespace LanceC.SpreadsheetIO.Reading
{
    /// <summary>
    /// Represents a numbered resource from a spreadsheet page.
    /// </summary>
    /// <typeparam name="TResource">The type of resource.</typeparam>
    [ExcludeFromCodeCoverage]
    public record NumberedResource<TResource>(
        uint RowNumber,
        TResource? Resource)
        where TResource : class
    {
        /// <summary>
        /// Gets the row number in the spreadsheet.
        /// </summary>
        public uint RowNumber { get; init; } = RowNumber;

        /// <summary>
        /// Gets the resource.
        /// </summary>
        public TResource? Resource { get; init; } = Resource;
    }
}
