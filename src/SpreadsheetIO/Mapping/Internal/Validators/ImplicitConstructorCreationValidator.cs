using System.Linq;
using LanceC.SpreadsheetIO.Mapping.Internal.Extensions;
using LanceC.SpreadsheetIO.Mapping.Validation;
using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Mapping.Internal.Validators
{
    internal class ImplicitConstructorCreationValidator :
        ResourceMapOptionsExtensionValidator<ImplicitConstructorResourceMapOptionsExtension>
    {
        public override ResourceMapValidationResult Validate<TResource>(ResourceMap<TResource> map)
        {
            var constructorCreationExtension = map.Options.FindExtension<ImplicitConstructorResourceMapOptionsExtension>();
            var constructorParameterTypes = map.Properties.Select(p => p.Property.PropertyType)
                .ToArray();

            var constructor = typeof(TResource).GetConstructor(constructorParameterTypes);
            if (constructor is null)
            {
                return ResourceMapValidationResult.Failure(Messages.MissingResourceConstructor(typeof(TResource).Name));
            }

            return ResourceMapValidationResult.Success();
        }
    }
}
