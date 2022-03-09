namespace LanceC.SpreadsheetIO.Mapping2;

internal class CartographyOptions
{
    public CartographyOptions(Action<ICartographerBuilder> configureAction)
    {
        ConfigureAction = configureAction;
    }

    public Action<ICartographerBuilder> ConfigureAction { get; }
}
