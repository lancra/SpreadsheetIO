using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Mapping.Internal;
using LanceC.SpreadsheetIO.Shared.Internal;
using LanceC.SpreadsheetIO.Shared.Internal.Generators;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;
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
            .AddScoped<ISpreadsheetDocumentWrapperFactory, SpreadsheetDocumentWrapperFactory>()
            .AddScoped<ISpreadsheetFactory, SpreadsheetFactory>()
            .AddScoped<IStylesheetMutator, StylesheetBorderMutator>()
            .AddScoped<IStylesheetMutator, StylesheetFillMutator>()
            .AddScoped<IStylesheetMutator, StylesheetFontMutator>()
            .AddScoped<IStylesheetMutator, StylesheetStyleMutator>()
            .AddScoped<ISpreadsheetGenerator, SharedStringTableGenerator>()
            .AddScoped<ISpreadsheetGenerator, StylesheetGenerator>()
            .AddScoped<IBorderIndexer, BorderIndexer>()
            .AddScoped<IFillIndexer, FillIndexer>()
            .AddScoped<IFontIndexer, FontIndexer>()
            .AddScoped<IStringIndexer, StringIndexer>()
            .AddScoped<IStyleIndexer, StyleIndexer>()
            .AddScoped<IResourceMapManager, ResourceMapManager>();
    }
}
