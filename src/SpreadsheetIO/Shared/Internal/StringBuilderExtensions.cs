using System.Text;

namespace LanceC.SpreadsheetIO.Shared.Internal
{
    internal static class StringBuilderExtensions
    {
        public static StringBuilder AppendIf(this StringBuilder builder, string? value, bool condition)
        {
            if (condition)
            {
                builder.Append(value);
            }

            return builder;
        }
    }
}
