using LanceC.SpreadsheetIO.Mapping.Extensions;
using LanceC.SpreadsheetIO.Mapping.Validation;
using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Mapping.Internal.Validators
{
    internal class SingleConstructorCreationValidator : ResourceMapValidator
    {
        public override bool CanValidate<TResource>(ResourceMap<TResource> map)
            => map.Options.HasExtension<ExplicitConstructorResourceMapOptionsExtension>() ||
            map.Options.HasExtension<ImplicitConstructorResourceMapOptionsExtension>();

        public override ResourceMapValidationResult Validate<TResource>(ResourceMap<TResource> map)
        {
            var hasExplicitConstructor = map.Options.HasExtension<ExplicitConstructorResourceMapOptionsExtension>();
            var hasImplicitConstructor = map.Options.HasExtension<ImplicitConstructorResourceMapOptionsExtension>();

            if (hasExplicitConstructor && hasImplicitConstructor)
            {
                return ResourceMapValidationResult.Failure(Messages.DuplicateResourceConstructor(typeof(TResource).Name));
            }

            return ResourceMapValidationResult.Success();
        }
    }
}
