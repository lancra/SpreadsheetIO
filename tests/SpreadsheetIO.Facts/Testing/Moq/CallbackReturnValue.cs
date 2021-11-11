namespace LanceC.SpreadsheetIO.Facts.Testing.Moq;

public class CallbackReturnValue<TResult>
{
    public CallbackReturnValue(TResult value, Action? callback = default)
    {
        Value = value;
        Callback = callback;
    }

    public TResult Value { get; }

    public Action? Callback { get; }
}
