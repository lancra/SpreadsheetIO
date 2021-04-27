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
using LanceC.SpreadsheetIO.Writing;
using LanceC.SpreadsheetIO.Writing.Internal.Serializing;
using LanceC.SpreadsheetIO.Writing.Internal.Writers;
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
            => services.AddScoped<ISpreadsheetFactory, SpreadsheetFactory>()
            .AddSpreadsheetIOMapping()
            .AddSpreadsheetIOReading()
            .AddSpreadsheetIOShared()
            .AddSpreadsheetIOStyling()
            .AddSpreadsheetIOWriting();

        private static IServiceCollection AddSpreadsheetIOMapping(this IServiceCollection services)
            => services
            .AddScoped<IResourceMapManager, ResourceMapManager>();

        private static IServiceCollection AddSpreadsheetIOReading(this IServiceCollection services)
            => services
            .AddScoped<IResourceCreator, ResourceCreator>()
            .AddScoped<IResourcePropertyDefaultValueResolver, ResourcePropertyDefaultValueResolver>()
            .AddScoped<IResourcePropertyValueResolver, ResourcePropertyValueResolver>()
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
            .AddScoped<IResourcePropertyCollectionFactory, ResourcePropertyCollectionFactory>()
            .AddScoped<IElementReaderFactory, ElementReaderFactory>()
            .AddScoped<ISpreadsheetPageMapReader, SpreadsheetPageMapReader>();

        private static IServiceCollection AddSpreadsheetIOShared(this IServiceCollection services)
            => services
            .AddScoped<ISpreadsheetDocumentWrapperFactory, SpreadsheetDocumentWrapperFactory>()
            .AddScoped<ISpreadsheetGenerator, SharedStringTableGenerator>()
            .AddScoped<ISpreadsheetGenerator, StylesheetGenerator>()
            .AddScoped<IStringIndexer, StringIndexer>();

        private static IServiceCollection AddSpreadsheetIOStyling(this IServiceCollection services)
            => services
            .AddScoped<IStylesheetMutator, StylesheetBorderMutator>()
            .AddScoped<IStylesheetMutator, StylesheetFillMutator>()
            .AddScoped<IStylesheetMutator, StylesheetFontMutator>()
            .AddScoped<IStylesheetMutator, StylesheetStyleMutator>()
            .AddScoped<IBorderIndexer, BorderIndexer>()
            .AddScoped<IFillIndexer, FillIndexer>()
            .AddScoped<IFontIndexer, FontIndexer>()
            .AddScoped<IStyleIndexer, StyleIndexer>();

        private static IServiceCollection AddSpreadsheetIOWriting(this IServiceCollection services)
            => services
            .AddScoped<IResourcePropertySerializer, ResourcePropertySerializer>()
            .AddScoped<IResourcePropertySerializerStrategy, BooleanResourcePropertySerializerStrategy>()
            .AddScoped<IResourcePropertySerializerStrategy, CharacterResourcePropertySerializerStrategy>()
            .AddScoped<IResourcePropertySerializerStrategy, DateTimeResourcePropertySerializerStrategy>()
            .AddScoped<IResourcePropertySerializerStrategy, DateTimeOffsetResourcePropertySerializerStrategy>()
            .AddScoped<IResourcePropertySerializerStrategy, DecimalResourcePropertySerializerStrategy>()
            .AddScoped<IResourcePropertySerializerStrategy, DoubleResourcePropertySerializerStrategy>()
            .AddScoped<IResourcePropertySerializerStrategy, IntegerResourcePropertySerializerStrategy>()
            .AddScoped<IResourcePropertySerializerStrategy, StringResourcePropertySerializerStrategy>()
            .AddScoped<ISpreadsheetPageMapWriter, SpreadsheetPageMapWriter>();
    }
}
