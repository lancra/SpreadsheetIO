namespace LanceC.SpreadsheetIO.Mapping;

/// <summary>
/// Defines the internal builder for generating a <see cref="ICartographer"/>.
/// </summary>
internal interface IInternalCartographerBuilder : ICartographerBuilder
{
    /// <summary>
    /// Builds the cartographer.
    /// </summary>
    /// <returns>The cartographer.</returns>
    ICartographer Build();
}
