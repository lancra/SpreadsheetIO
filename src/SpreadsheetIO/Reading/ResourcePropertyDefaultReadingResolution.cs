using Ardalis.SmartEnum;

namespace LanceC.SpreadsheetIO.Reading
{
    /// <summary>
    /// Represents the scenario in which a default property should be used in place of a parsed value.
    /// </summary>
    public class ResourcePropertyDefaultReadingResolution : SmartEnum<ResourcePropertyDefaultReadingResolution>
    {
        /// <summary>
        /// Specifies that the default value should be used for an <see cref="ResourcePropertyParseResultKind.Empty"/> parse result.
        /// </summary>
        public static readonly ResourcePropertyDefaultReadingResolution Empty = new(1, "Empty", ResourcePropertyParseResultKind.Empty);

        /// <summary>
        /// Specifies that the default value should be used for an <see cref="ResourcePropertyParseResultKind.Missing"/> parse result.
        /// </summary>
        public static readonly ResourcePropertyDefaultReadingResolution Missing =
            new(2, "Missing", ResourcePropertyParseResultKind.Missing);

        /// <summary>
        /// Specifies that the default value should be used for an <see cref="ResourcePropertyParseResultKind.Invalid"/> parse result.
        /// </summary>
        public static readonly ResourcePropertyDefaultReadingResolution Invalid =
            new(3, "Invalid", ResourcePropertyParseResultKind.Invalid);

        private ResourcePropertyDefaultReadingResolution(int id, string name, ResourcePropertyParseResultKind parseResultKind)
            : base(name, id)
        {
            ParseResultKind = parseResultKind;
        }

        /// <summary>
        /// Gets the corresponding resource property parse result kind.
        /// </summary>
        public ResourcePropertyParseResultKind ParseResultKind { get; }
    }
}
