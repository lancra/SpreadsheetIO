using LanceC.SpreadsheetIO.Styling.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace LanceC.SpreadsheetIO
{
    /// <summary>
    /// Provides extensions for service descriptor registration.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers spreadsheet service descriptors.
        /// </summary>
        /// <param name="services">The service collection to modify.</param>
        /// <returns>The modified service collection.</returns>
        public static IServiceCollection AddSpreadsheetIO(this IServiceCollection services)
            => services
            .AddSingleton<IBorderIndexer, BorderIndexer>()
            .AddSingleton<IFillIndexer, FillIndexer>()
            .AddSingleton<IFontIndexer, FontIndexer>()
            .AddSingleton<IStyleIndexer, StyleIndexer>();
    }
}
