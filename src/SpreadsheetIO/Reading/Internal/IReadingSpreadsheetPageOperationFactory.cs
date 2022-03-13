using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Reading.Internal.Readers;

namespace LanceC.SpreadsheetIO.Reading.Internal;

/// <summary>
/// Defines the factory for creating read operations on a spreadsheet page.
/// </summary>
internal interface IReadingSpreadsheetPageOperationFactory
{
    /// <summary>
    /// Creates a read operation on a spreadsheet page.
    /// </summary>
    /// <typeparam name="TResource">The type of resource to read.</typeparam>
    /// <param name="worksheetReader">The worksheet element reader.</param>
    /// <param name="headerRowResult">The result from reading the header row.</param>
    /// <param name="map">The resource map.</param>
    /// <returns>The created read operation on a spreadsheet page.</returns>
    IReadingSpreadsheetPageOperation<TResource> Create<TResource>(
        IWorksheetElementReader worksheetReader,
        HeaderRowReadingResult<TResource> headerRowResult,
        ResourceMap map)
        where TResource : class;
}
