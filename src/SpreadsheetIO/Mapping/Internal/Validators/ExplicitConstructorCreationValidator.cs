using LanceC.SpreadsheetIO.Mapping.Extensions;
using LanceC.SpreadsheetIO.Mapping.Validation;
using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Mapping.Internal.Validators
{
    internal class ExplicitConstructorCreationValidator :
        ResourceMapOptionsExtensionValidator<ExplicitConstructorResourceMapOptionsExtension>
    {
        public override ResourceMapValidationResult Validate<TResource>(ResourceMap<TResource> map)
        {
            var constructorCreationExtension = map.Options.FindExtension<ExplicitConstructorResourceMapOptionsExtension>();

            var constructorParameterTypes = new List<Type>();
            var unmappedPropertyNames = new List<string>();
            foreach (var propertyName in constructorCreationExtension!.PropertyNames)
            {
                var propertyMap = map.Properties.SingleOrDefault(p => p.Property.Name == propertyName);
                if (propertyMap is null)
                {
                    unmappedPropertyNames.Add(propertyName);
                    continue;
                }

                constructorParameterTypes.Add(propertyMap.Property.PropertyType);
            }

            if (unmappedPropertyNames.Any())
            {
                return ResourceMapValidationResult.Failure(
                    Messages.MissingMapForResourceProperties(typeof(TResource).Name, string.Join(',', unmappedPropertyNames)));
            }

            var constructor = typeof(TResource).GetConstructor(constructorParameterTypes.ToArray());
            if (constructor is null)
            {
                return ResourceMapValidationResult.Failure(Messages.MissingResourceConstructor(typeof(TResource).Name));
            }

            return ResourceMapValidationResult.Success();
        }
    }
}
