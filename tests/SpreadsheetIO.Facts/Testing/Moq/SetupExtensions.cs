using Moq.Language.Flow;

namespace LanceC.SpreadsheetIO.Facts.Testing.Moq;

public static class SetupExtensions
{
    public static IReturnsResult<TMock> ReturnsCallbackSequence<TMock, TResult>(
        this ISetup<TMock, TResult> setup,
        params CallbackReturnValue<TResult>[] callbackReturnValues)
        where TMock : class
    {
        var callIndex = -1;
        var currentValue = default(TResult);

        return setup
            .Callback(
                () =>
                {
                    callIndex++;
                    if (callIndex > callbackReturnValues.Length)
                    {
                        currentValue = default;
                        return;
                    }

                    var callbackReturnValue = callbackReturnValues[callIndex];

                    currentValue = callbackReturnValue.Value;
                    callbackReturnValue.Callback?.Invoke();
                })
            .Returns(() => currentValue!);
    }
}
