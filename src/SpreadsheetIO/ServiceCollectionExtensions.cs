using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Mapping.Internal;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Reading.Internal;
using LanceC.SpreadsheetIO.Reading.Internal.Parsing;
using LanceC.SpreadsheetIO.Reading.Internal.Properties;
using LanceC.SpreadsheetIO.Reading.Internal.Readers;
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
            .AddScoped<IResourceMapManager, ResourceMapManager>()
            .AddScoped<IElementReaderFactory, ElementReaderFactory>()
            .AddScoped<IResourceCreator, ResourceCreator>()
            .AddScoped<IResourcePropertyParser, ResourcePropertyParser>()
            .AddScoped<IResourcePropertyParserStrategy, BooleanResourcePropertyParserStrategy>()
            .AddScoped<IResourcePropertyParserStrategy, ByteResourcePropertyParserStrategy>()
            .AddScoped<IResourcePropertyParserStrategy, CharacterResourcePropertyParserStrategy>()
            .AddScoped<IResourcePropertyParserStrategy, DateTimeOffsetResourcePropertyParserStrategy>()
            .AddScoped<IResourcePropertyParserStrategy, DateTimeResourcePropertyParserStrategy>()
            .AddScoped<IResourcePropertyParserStrategy, DecimalResourcePropertyParserStrategy>()
            .AddScoped<IResourcePropertyParserStrategy, DoubleResourcePropertyParserStrategy>()
            .AddScoped<IResourcePropertyParserStrategy, FloatResourcePropertyParserStrategy>()
            .AddScoped<IResourcePropertyParserStrategy, IntegerResourcePropertyParserStrategy>()
            .AddScoped<IResourcePropertyParserStrategy, LongResourcePropertyParserStrategy>()
            .AddScoped<IResourcePropertyParserStrategy, ShortResourcePropertyParserStrategy>()
            .AddScoped<IResourcePropertyParserStrategy, SignedByteResourcePropertyParserStrategy>()
            .AddScoped<IResourcePropertyParserStrategy, StringResourcePropertyParserStrategy>()
            .AddScoped<IResourcePropertyParserStrategy, UnsignedIntegerResourcePropertyParserStrategy>()
            .AddScoped<IResourcePropertyParserStrategy, UnsignedLongResourcePropertyParserStrategy>()
            .AddScoped<IResourcePropertyParserStrategy, UnsignedShortResourcePropertyParserStrategy>()
            .AddScoped<IResourcePropertyValueResolver, ResourcePropertyValueResolver>()
            .AddScoped<IResourcePropertyDefaultValueResolver, ResourcePropertyDefaultValueResolver>()
            .AddScoped<IResourcePropertyCollectionFactory, ResourcePropertyCollectionFactory>()
            .AddScoped<ISpreadsheetPageReader, SpreadsheetPageReader>();
    }
}
