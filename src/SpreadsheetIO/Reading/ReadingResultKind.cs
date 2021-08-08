using Ardalis.SmartEnum;

namespace LanceC.SpreadsheetIO.Reading
{
    /// <summary>
    /// Represents the kind of reading result.
    /// </summary>
    public class ReadingResultKind : SmartEnum<ReadingResultKind>
    {
        /// <summary>
        /// Specifies that the header and all resources were read successfully.
        /// </summary>
        public static readonly ReadingResultKind Success = new(1, "Success");

        /// <summary>
        /// Specifies that the header and some resources were read successfully.
        /// </summary>
        public static readonly ReadingResultKind PartialFailure = new(2, "Partial Failure");

        /// <summary>
        /// Specifies that either the header or all resources were not read.
        /// </summary>
        public static readonly ReadingResultKind Failure = new(3, "Failure");

        private ReadingResultKind(int id, string name)
            : base(name, id)
        {
        }
    }
}
