using System.Collections.Generic;
using System.Linq;
using LanceC.SpreadsheetIO.Mapping.Internal.Extensions;
using LanceC.SpreadsheetIO.Mapping.Validation;
using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Mapping.Internal.Validators
{
    internal class PropertySetterCreationValidator : ResourceMapValidator
    {
        public override bool CanValidate<TResource>(ResourceMap<TResource> map)
            => !map.Options.HasExtension<ExplicitConstructorResourceMapOptionsExtension>() &&
            !map.Options.HasExtension<ImplicitConstructorResourceMapOptionsExtension>();

        public override ResourceMapValidationResult Validate<TResource>(ResourceMap<TResource> map)
        {
            var invalidPropertyNames = new List<string>();
            foreach (var propertyMap in map.Properties)
            {
                if (propertyMap.Property.GetSetMethod() is null)
                {
                    invalidPropertyNames.Add(propertyMap.Key.Name);
                }
            }

            if (invalidPropertyNames.Any())
            {
                return ResourceMapValidationResult.Failure(
                    Messages.InvalidPropertyMapsForSetterCreation(typeof(TResource).Name, string.Join(',', invalidPropertyNames)));
            }

            return ResourceMapValidationResult.Success();
        }
    }
}
