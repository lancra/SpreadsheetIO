using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Properties;

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
                    Messages.DuplicateStrategy(
                        "resource property parser",
                        typeof(IResourcePropertyParserStrategy).Name,
                        strategyGrouping.Key.Name));
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
                    Messages.MissingStrategy(typeof(IResourcePropertyParserStrategy).Name, propertyType.Name));
            }

            return strategy!.TryParse(cellValue, map, out value);
        }

        private void AddStrategy(IResourcePropertyParserStrategy strategy)
            => _strategies.Add(strategy.PropertyType, strategy);
    }
}
