using System;
using System.Collections.Generic;
using System.Linq;
using LanceC.SpreadsheetIO.Mapping;

namespace LanceC.SpreadsheetIO.Reading.Internal.Parsing
{
    internal class ResourcePropertyParser : IResourcePropertyParser
    {
        private readonly IDictionary<Type, IResourcePropertyParserStrategy> _strategies =
            new Dictionary<Type, IResourcePropertyParserStrategy>();

        public ResourcePropertyParser(IEnumerable<IResourcePropertyParserStrategy> strategies)
        {
            var strategyGroupings = strategies.GroupBy(strategy => strategy.PropertyType);
            foreach (var strategyGrouping in strategyGroupings)
            {
                var groupedStrategies = strategyGrouping.ToArray();
                if (groupedStrategies.Length == 1)
                {
                    AddStrategy(groupedStrategies[0]);
                    continue;
                }

                var nonDefaultGroupedStrategies = groupedStrategies
                    .Where(strategy => strategy is not IDefaultResourcePropertyParserStrategy)
                    .ToArray();
                if (nonDefaultGroupedStrategies.Length == 1)
                {
                    AddStrategy(nonDefaultGroupedStrategies[0]);
                    continue;
                }

                throw new InvalidOperationException(
                    $"The ${typeof(ResourcePropertyParser).Name} cannot be constructed because there are multiple " +
                    $"{typeof(IResourcePropertyParserStrategy).Name} instances registered for the {strategyGrouping.Key.Name} " +
                    "property type.");
            }
        }

        public ResourcePropertyParseResultKind TryParse<TResource>(string cellValue, PropertyMap<TResource> map, out object? value)
            where TResource : class
        {
            var propertyType = Nullable.GetUnderlyingType(map.Property.PropertyType) ?? map.Property.PropertyType;

            var hasStrategy = _strategies.TryGetValue(propertyType, out var strategy);
            if (!hasStrategy)
            {
                throw new InvalidOperationException(
                    $"No {typeof(IResourcePropertyParserStrategy).Name} was defined for the {propertyType.Name} property type.");
            }

            return strategy!.TryParse(cellValue, map, out value);
        }

        private void AddStrategy(IResourcePropertyParserStrategy strategy)
            => _strategies.Add(strategy.PropertyType, strategy);
    }
}
