namespace LanceC.SpreadsheetIO.Mapping;

internal class CartographyOptions
{
    public CartographyOptions(Action<ICartographerBuilder> configureAction)
    {
        ConfigureAction = configureAction;
    }

    public Action<ICartographerBuilder> ConfigureAction { get; }
}
