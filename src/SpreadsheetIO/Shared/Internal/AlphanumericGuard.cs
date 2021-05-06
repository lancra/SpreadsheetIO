using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using LanceC.SpreadsheetIO.Properties;

namespace Ardalis.GuardClauses
{
    [SuppressMessage(
        "Style",
        "IDE0060:Remove unused parameter",
        Justification = "The guard clause parameters are not used in the methods but are required for usage.")]
    internal static class AlphanumericGuard
    {
        public static void NonAlphabetic(this IGuardClause guardClause, string input, string parameterName)
        {
            var regex = new Regex("^[a-zA-Z]*$");
            if (!regex.IsMatch(input))
            {
                throw new ArgumentException(Messages.NonAlphabeticInput(parameterName), parameterName);
            }
        }

        public static void NonAlphanumeric(this IGuardClause guardClause, string input, string parameterName)
        {
            var regex = new Regex("^[a-zA-Z0-9]*$");
            if (!regex.IsMatch(input))
            {
                throw new ArgumentException(Messages.NonAlphanumericInput(parameterName), parameterName);
            }
        }

        public static void NonNumeric(this IGuardClause guardClause, string input, string parameterName)
        {
            var regex = new Regex("^[0-9]*$");
            if (!regex.IsMatch(input))
            {
                throw new ArgumentException(Messages.NonNumericInput(parameterName), parameterName);
            }
        }
    }
}
