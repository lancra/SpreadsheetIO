using Ardalis.GuardClauses;
using LanceC.SpreadsheetIO.Mapping2.Options.Registrations;
using LanceC.SpreadsheetIO.Properties;

namespace LanceC.SpreadsheetIO.Mapping2.Options.Converters;

internal class MapOptionConverter<TRegistration, TOption> : IMapOptionConverter<TRegistration, TOption>
    where TRegistration : IMapOptionRegistration
    where TOption : IMapOption
{
    private readonly IDictionary<Type, IMapOptionConversionStrategy<TRegistration, TOption>> _strategies =
        new Dictionary<Type, IMapOptionConversionStrategy<TRegistration, TOption>>();

    public MapOptionConverter(IEnumerable<IMapOptionConversionStrategy<TRegistration, TOption>> strategies)
    {
        Guard.Against.Null(strategies, nameof(strategies));

        var strategyGroupings = strategies.GroupBy(strategy => strategy.RegistrationType);
        foreach (var strategyGrouping in strategyGroupings)
        {
            var groupedStrategies = strategyGrouping.ToArray();
            if (groupedStrategies.Length != 1)
            {
                throw new InvalidOperationException(
                    Messages.DuplicateStrategy(
                        "map option converter",
                        typeof(IMapOptionConversionStrategy<IMapOptionRegistration, IMapOption>).Name,
                        strategyGrouping.Key.Name));
            }

            _strategies.Add(strategyGrouping.Key, groupedStrategies[0]);
        }
    }

    public MapOptionConversionResult<TOption> ConvertToOption(TRegistration registration, ResourceMapBuilder resourceMapBuilder)
    {
        if (registration is TOption implicitOption)
        {
            return MapOptionConversionResult.Success(registration, implicitOption);
        }

        if (_strategies.TryGetValue(registration.GetType(), out var strategy))
        {
            return strategy.ConvertToOption(registration, resourceMapBuilder);
        }

        return MapOptionConversionResult.Skipped<TOption>(registration);
    }
}
