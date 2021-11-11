using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Mapping.Validation
{
    /// <summary>
    /// Represents the result of validating a resource map.
    /// </summary>
    public class ResourceMapValidationResult
    {
        private ResourceMapValidationResult(
            bool isValid,
            string message,
            IReadOnlyCollection<ResourceMapValidationResult> innerValidationResults)
        {
            IsValid = isValid;
            Message = message;
            InnerValidationResults = innerValidationResults;
        }

        /// <summary>
        /// Gets the value that determines whether the resource map is valid.
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// Gets the validation message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the inner validation results.
        /// </summary>
        public IReadOnlyCollection<ResourceMapValidationResult> InnerValidationResults { get; }

        /// <summary>
        /// Creates the result for a successful validation.
        /// </summary>
        /// <returns>The validation result.</returns>
        public static ResourceMapValidationResult Success()
            => new(true, string.Empty, Array.Empty<ResourceMapValidationResult>());

        /// <summary>
        /// Creates the result for a failed validation.
        /// </summary>
        /// <param name="message">The validation message.</param>
        /// <returns>The validation result.</returns>
        public static ResourceMapValidationResult Failure(string message)
            => new(false, message, Array.Empty<ResourceMapValidationResult>());

        /// <summary>
        /// Creates the aggregate from multiple validation results.
        /// </summary>
        /// <param name="validationResults">The validation results to aggregate.</param>
        /// <returns>The validation result.</returns>
        public static ResourceMapValidationResult Aggregate(IEnumerable<ResourceMapValidationResult> validationResults)
        {
            if (validationResults is null || !validationResults.Any())
            {
                return Success();
            }

            var includedValidationResults = validationResults.Where(validationResult => !validationResult.IsValid)
                .ToArray();
            var isValid = !includedValidationResults.Any();

            return new(isValid, isValid ? string.Empty : Messages.FailedResourceMapValidation, includedValidationResults);
        }
    }
}
