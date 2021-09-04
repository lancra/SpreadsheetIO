using System;
using System.Collections.Generic;
using System.Linq;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Internal.Extensions;
using LanceC.SpreadsheetIO.Properties;
using LanceC.SpreadsheetIO.Reading.Internal.Properties;

namespace LanceC.SpreadsheetIO.Reading.Internal
{
    internal class ResourceCreator : IResourceCreator
    {
        public TResource? Create<TResource>(ResourceMap<TResource> map, IResourcePropertyValues<TResource> values)
            where TResource : class
        {
            var explicitConstructorExtension = map.Options.FindExtension<ExplicitConstructorResourceMapOptionsExtension>();
            if (explicitConstructorExtension is not null)
            {
                return CreateUsingExplicitConstructor(map, values, explicitConstructorExtension);
            }

            var useImplicitConstructor = map.Options.HasExtension<ImplicitConstructorResourceMapOptionsExtension>();
            if (useImplicitConstructor)
            {
                return CreateUsingImplicitConstructor(map, values);
            }

            return CreateUsingPropertySetters(map, values);
        }

        private static TResource? CreateUsingExplicitConstructor<TResource>(
            ResourceMap<TResource> map,
            IResourcePropertyValues<TResource> values,
            ExplicitConstructorResourceMapOptionsExtension constructorCreationExtension)
            where TResource : class
        {
            var constructorParameterTypes = new List<Type>();
            var constructorParameters = new List<object?>();

            foreach (var propertyName in constructorCreationExtension.PropertyNames)
            {
                var propertyMap = map.Properties.SingleOrDefault(p => p.Property.Name == propertyName);
                if (propertyMap is null)
                {
                    throw new ArgumentException(Messages.MissingResourcePropertyForConstructorParameter(propertyName));
                }

                if (!values.TryGetValue(propertyMap, out var value))
                {
                    return default;
                }

                constructorParameterTypes.Add(propertyMap.Property.PropertyType);
                constructorParameters.Add(value);
            }

            // TODO: Move this check to `ResourceMapOptionsBuilder<TResource>.UseExplicitConstructor(string[])`.
            var constructor = typeof(TResource).GetConstructor(constructorParameterTypes.ToArray());
            if (constructor is null)
            {
                throw new ArgumentException(Messages.MissingResourceConstructor(typeof(TResource).Name));
            }

            var resource = (TResource)constructor.Invoke(constructorParameters.ToArray());
            return resource;
        }

        private static TResource? CreateUsingImplicitConstructor<TResource>(
            ResourceMap<TResource> map,
            IResourcePropertyValues<TResource> values)
            where TResource : class
        {
            var constructorParameterTypes = new List<Type>();
            var constructorParameters = new List<object?>();

            foreach (var propertyMap in map.Properties)
            {
                if (!values.TryGetValue(propertyMap, out var value))
                {
                    return default;
                }

                constructorParameterTypes.Add(propertyMap.Property.PropertyType);
                constructorParameters.Add(value);
            }

            // TODO: Move this check to `ResourceMapOptionsBuilder<TResource>.UseImplicitConstructor()`.
            var constructor = typeof(TResource).GetConstructor(constructorParameterTypes.ToArray());
            if (constructor is null)
            {
                throw new ArgumentException(Messages.MissingResourceConstructor(typeof(TResource).Name));
            }

            var resource = (TResource)constructor.Invoke(constructorParameters.ToArray());
            return resource;
        }

        private static TResource? CreateUsingPropertySetters<TResource>(
            ResourceMap<TResource> map,
            IResourcePropertyValues<TResource> values)
            where TResource : class
        {
            var resource = Activator.CreateInstance<TResource>();

            foreach (var resourceProperty in map.Properties)
            {
                var hasValue = values.TryGetValue(resourceProperty, out var value);
                if (hasValue)
                {
                    resourceProperty.Property.SetValue(resource, value);
                }
            }

            return resource;
        }
    }
}
