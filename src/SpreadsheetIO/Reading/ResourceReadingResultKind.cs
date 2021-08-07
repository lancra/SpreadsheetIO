using Ardalis.SmartEnum;

namespace LanceC.SpreadsheetIO.Reading
{
    /// <summary>
    /// Represents the kind of resource reading result.
    /// </summary>
    public class ResourceReadingResultKind : SmartEnum<ResourceReadingResultKind>
    {
        /// <summary>
        /// Specifies that the resource was read successfully.
        /// </summary>
        public static readonly ResourceReadingResultKind Success = new(1, "Success");

        /// <summary>
        /// Specifies that the resource was not read.
        /// </summary>
        public static readonly ResourceReadingResultKind Failure = new(2, "Failure");

        private ResourceReadingResultKind(int id, string name)
            : base(name, id)
        {
        }
    }
}
