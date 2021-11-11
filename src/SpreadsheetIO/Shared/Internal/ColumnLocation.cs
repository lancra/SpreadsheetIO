using Ardalis.GuardClauses;

namespace LanceC.SpreadsheetIO.Shared.Internal
{
    internal class ColumnLocation
    {
        private const int AlphabetCount = 26;
        private const int AsciiOffset = 64;

        public ColumnLocation(uint number)
        {
            Guard.Against.Zero(number, nameof(number));

            Number = number;
            Letter = GetColumnLetter(number);
        }

        public ColumnLocation(string letter)
        {
            Guard.Against.NullOrEmpty(letter, nameof(letter));
            Guard.Against.NonAlphabetic(letter, nameof(letter));

            Number = GetColumnNumber(letter);
            Letter = letter;
        }

        public uint Number { get; }

        public string Letter { get; }

        private static string GetColumnLetter(uint number)
        {
            var dividend = Convert.ToInt32(number);
            var columnLetter = string.Empty;
            while (dividend > 0)
            {
                var modulo = (dividend - 1) % AlphabetCount;
                columnLetter = Convert.ToChar(AsciiOffset + 1 + modulo).ToString() + columnLetter;
                dividend = Convert.ToInt32((dividend - modulo) / AlphabetCount);
            }

            return columnLetter;
        }

        private static uint GetColumnNumber(string letter)
        {
            var columnNumber = 0U;
            var multiplier = 1;

            foreach (char character in letter.ToCharArray().Reverse())
            {
                var characterMultiplier = character - AsciiOffset;
                var characterNumber = Convert.ToUInt32(characterMultiplier * multiplier);
                columnNumber += characterNumber;

                multiplier *= AlphabetCount;
            }

            return columnNumber;
        }
    }
}
