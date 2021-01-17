using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Styling.Internal.Generators;
using LanceC.SpreadsheetIO.Styling.Internal.Indexers;
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
        [ExcludeFromCodeCoverage]
        public static IServiceCollection AddSpreadsheetIO(this IServiceCollection services)
            => services
            .AddSingleton<IStylesheetMutator, StylesheetBorderMutator>()
            .AddSingleton<IStylesheetMutator, StylesheetFillMutator>()
            .AddSingleton<IStylesheetMutator, StylesheetFontMutator>()
            .AddSingleton<IStylesheetMutator, StylesheetStyleMutator>()
            .AddSingleton<IBorderIndexer, BorderIndexer>()
            .AddSingleton<IFillIndexer, FillIndexer>()
            .AddSingleton<IFontIndexer, FontIndexer>()
            .AddSingleton<IStringIndexer, StringIndexer>()
            .AddSingleton<IStyleIndexer, StyleIndexer>();
    }
}
