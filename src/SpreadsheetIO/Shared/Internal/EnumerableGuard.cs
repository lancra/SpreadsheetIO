using System.Diagnostics.CodeAnalysis;

namespace Ardalis.GuardClauses;

[SuppressMessage(
    "Style",
    "IDE0060:Remove unused parameter",
    Justification = "The guard clause parameters are not used in the methods but are required for usage.")]
internal static class EnumerableGuard
{
    public static void NullElements<T>(this IGuardClause guardClause, IEnumerable<T>? input, string parameterName)
    {
        if (input is null)
        {
            return;
        }

        foreach (var element in input)
        {
            Guard.Against.Null(element, parameterName, $"Required input {parameterName} had null elements.");
        }
    }

    public static void NullOrEmptyElements(this IGuardClause guardClause, IEnumerable<string>? input, string parameterName)
    {
        if (input is null)
        {
            return;
        }

        foreach (var element in input)
        {
            Guard.Against.NullOrEmpty(element, parameterName, $"Required input {parameterName} had null or empty elements.");
        }
    }
}
