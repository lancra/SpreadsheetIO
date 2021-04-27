using System;
using System.Collections.Generic;
using System.Linq;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Extensions;

namespace LanceC.SpreadsheetIO.Writing.Internal.Serializing
{
    internal class ResourcePropertySerializer : IResourcePropertySerializer
    {
        private readonly IDictionary<Type, IResourcePropertySerializerStrategy> _strategies =
            new Dictionary<Type, IResourcePropertySerializerStrategy>();

        public ResourcePropertySerializer(IEnumerable<IResourcePropertySerializerStrategy> strategies)
        {
            var strategyPropertyTypes = strategies.SelectMany(strategy => strategy.PropertyTypes)
                .Distinct();
            foreach (var strategyPropertyType in strategyPropertyTypes)
            {
                var supportedStrategies = strategies.Where(strategy => strategy.PropertyTypes.Contains(strategyPropertyType))
                    .ToArray();

                if (supportedStrategies.Length == 1)
                {
                    _strategies.Add(strategyPropertyType, supportedStrategies[0]);
                    continue;
                }

                var nonDefaultSupportedStrategies = supportedStrategies
                    .Where(strategy => strategy is not IDefaultResourcePropertySerializerStrategy)
                    .ToArray();
                if (nonDefaultSupportedStrategies.Length == 1)
                {
                    _strategies.Add(strategyPropertyType, nonDefaultSupportedStrategies[0]);
                    continue;
                }

                throw new InvalidOperationException(
                    $"The {typeof(ResourcePropertySerializer).Name} cannot be constructed because there are multiple " +
                    $"{typeof(IResourcePropertySerializerStrategy).Name} instances registered for the {strategyPropertyType} " +
                    "property type.");
            }
        }

        public WritingCellValue Serialize<TResource>(TResource resource, PropertyMap<TResource> map)
            where TResource : class
        {
            var propertyType = Nullable.GetUnderlyingType(map.Property.PropertyType) ?? map.Property.PropertyType;

            var hasStrategy = _strategies.TryGetValue(propertyType, out var strategy);
            if (!hasStrategy)
            {
                throw new InvalidOperationException(
                    $"No {typeof(IResourcePropertySerializerStrategy).Name} was defined for the {propertyType.Name} property type.");
            }

            var propertyValue = map.Property.GetValue(resource);
            if (propertyValue is null)
            {
                var defaultValueExtension = map.Options.FindExtension<DefaultValuePropertyMapOptionsExtension>();
                if (defaultValueExtension is not null)
                {
                    propertyValue = defaultValueExtension.Value;
                }
            }

            var cellValue = strategy!.Serialize(propertyValue, map);
            return cellValue;
        }
    }
}
