using LanceC.SpreadsheetIO.Mapping2;
using LanceC.SpreadsheetIO.Mapping2.Options;
using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Writing.Internal.Serializing;

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
                Messages.DuplicateStrategy(
                    "resource property serializer",
                    typeof(IResourcePropertySerializerStrategy).Name,
                    strategyPropertyType.Name));
        }
    }

    public WritingCellValue Serialize<TResource>(TResource resource, PropertyMap map)
        where TResource : class
    {
        var propertyType = Nullable.GetUnderlyingType(map.Property.PropertyType) ?? map.Property.PropertyType;

        var hasStrategy = _strategies.TryGetValue(propertyType, out var strategy);
        if (!hasStrategy)
        {
            throw new InvalidOperationException(
                Messages.MissingStrategy(typeof(IResourcePropertySerializerStrategy).Name, propertyType.Name));
        }

        var propertyValue = map.Property.GetValue(resource);
        if (propertyValue is null)
        {
            var defaultValueOption = map.Options.Find<DefaultValuePropertyMapOption>();
            if (defaultValueOption is not null)
            {
                propertyValue = defaultValueOption.Value;
            }
        }

        var cellValue = strategy!.Serialize(propertyValue, map);
        return cellValue;
    }
}
