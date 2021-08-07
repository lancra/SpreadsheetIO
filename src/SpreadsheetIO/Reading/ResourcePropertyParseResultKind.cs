using Ardalis.SmartEnum;

namespace LanceC.SpreadsheetIO.Reading
{
    /// <summary>
    /// Represents the kind of value parsed for a resource property.
    /// </summary>
    public class ResourcePropertyParseResultKind : SmartEnum<ResourcePropertyParseResultKind>
    {
        /// <summary>
        /// Specifies that no value is present and the resource property type supports this.
        /// </summary>
        public static readonly ResourcePropertyParseResultKind Empty = new(1, "Empty", true);

        /// <summary>
        /// Specifies that no value is present and the resource property type does not support this.
        /// </summary>
        public static readonly ResourcePropertyParseResultKind Missing = new(2, "Missing", false);

        /// <summary>
        /// Specifies that a value is present that could not be parsed for the resource property type.
        /// </summary>
        public static readonly ResourcePropertyParseResultKind Invalid = new(3, "Invalid", false);

        /// <summary>
        /// Specifies that a value is present that could be parsed for the resource property type.
        /// </summary>
        public static readonly ResourcePropertyParseResultKind Success = new(4, "Success", true);

        private ResourcePropertyParseResultKind(int id, string name, bool valid)
            : base(name, id)
        {
            Valid = valid;
        }

        /// <summary>
        /// Gets the value that determines whether the parsed value is valid for the associated resource property.
        /// </summary>
        public bool Valid { get; }
    }
}
