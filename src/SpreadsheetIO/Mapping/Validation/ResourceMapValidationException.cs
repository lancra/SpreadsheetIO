namespace LanceC.SpreadsheetIO.Mapping.Validation
{
    /// <summary>
    /// Represents a failure during validation of a resource map.
    /// </summary>
    public class ResourceMapValidationException : Exception
    {
        internal ResourceMapValidationException(ResourceMapValidationResult validationResult)
            : base(validationResult.Message)
        {
            ValidationResult = validationResult;
        }

        /// <summary>
        /// Gets the resource map validation result.
        /// </summary>
        public ResourceMapValidationResult ValidationResult { get; }
    }
}
