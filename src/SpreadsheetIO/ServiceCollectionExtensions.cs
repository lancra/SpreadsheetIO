using System.Diagnostics.CodeAnalysis;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Builders;
using LanceC.SpreadsheetIO.Mapping.Options;
using LanceC.SpreadsheetIO.Mapping.Options.Converters;
using LanceC.SpreadsheetIO.Mapping.Options.Registrations;
using LanceC.SpreadsheetIO.Mapping.Validation;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Reading.Internal;
using LanceC.SpreadsheetIO.Reading.Internal.Creation;
using LanceC.SpreadsheetIO.Reading.Internal.Parsing;
using LanceC.SpreadsheetIO.Reading.Internal.Properties;
using LanceC.SpreadsheetIO.Reading.Internal.Readers;
using LanceC.SpreadsheetIO.Shared.Internal;
using LanceC.SpreadsheetIO.Shared.Internal.Generators;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;
using LanceC.SpreadsheetIO.Styling.Internal;
using LanceC.SpreadsheetIO.Styling.Internal.Generators;
using LanceC.SpreadsheetIO.Styling.Internal.Indexers;
using LanceC.SpreadsheetIO.Writing;
using LanceC.SpreadsheetIO.Writing.Internal.Serializing;
using LanceC.SpreadsheetIO.Writing.Internal.Writers;
using Microsoft.Extensions.DependencyInjection;

namespace LanceC.SpreadsheetIO;

/// <summary>
/// Provides extensions for service descriptor registration.
/// </summary>
[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers spreadsheet service descriptors.
    /// </summary>
    /// <param name="services">The service collection to modify.</param>
    /// <param name="mapOptions">The options to use for resource maps.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddSpreadsheetIO(
        this IServiceCollection services,
        Action<ICartographerBuilder>? mapOptions = default)
        => services.AddScoped<ISpreadsheetFactory, SpreadsheetFactory>()
        .AddSpreadsheetIOMapping(mapOptions)
        .AddSpreadsheetIOReading()
        .AddSpreadsheetIOShared()
        .AddSpreadsheetIOStyling()
        .AddSpreadsheetIOWriting();

    private static IServiceCollection AddSpreadsheetIOMapping(
        this IServiceCollection services,
        Action<ICartographerBuilder>? mapOptions = default)
    {
        if (mapOptions is not null)
        {
            services.AddScoped(provider => new CartographyOptions(mapOptions));
        }

        return services.AddScoped<IInternalCartographerBuilder, CartographerBuilder>()
            .AddScoped<IMapBuilderFactory, MapBuilderFactory>()
            .AddScoped(typeof(IMapOptionConverter<,>), typeof(MapOptionConverter<,>))
            .AddScoped<
                IMapOptionConversionStrategy<IPropertyMapOptionRegistration, IPropertyMapOption>,
                DefaultValuePropertyMapOptionConversionStrategy>()
            .AddScoped<
                IMapOptionConversionStrategy<IResourceMapOptionRegistration, IResourceMapOption>,
                ExplicitConstructorResourceMapOptionConversionStrategy>()
            .AddScoped<
                IMapOptionConversionStrategy<IResourceMapOptionRegistration, IResourceMapOption>,
                ImplicitConstructorResourceMapOptionConversionStrategy>()
            .AddScoped<IResourceMapBuilderValidator, ResourceMapBuilderValidator>()
            .AddScoped<IResourceMapBuilderValidationStrategy, PropertySetterCreationValidationStrategy>()
            .AddScoped<IResourceMapBuilderValidationStrategy, UniquePropertyValidationStrategy>()
            .AddScoped<IResourceMapBuilderValidationStrategy, UniquePropertyKeyNameValidationStrategy>()
            .AddScoped<IResourceMapBuilderValidationStrategy, UniquePropertyKeyNumberValidationStrategy>();
    }

    private static IServiceCollection AddSpreadsheetIOReading(this IServiceCollection services)
        => services
        .AddScoped<IResourceCreator, ResourceCreator>()
        .AddScoped<IResourceCreationStrategy, ConstructorResourceCreationStrategy>()
        .AddScoped<IResourceCreationStrategy, PropertySettersResourceCreationStrategy>()
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
        .AddScoped<IReadingSpreadsheetPageOperationFactory, ReadingSpreadsheetPageOperationFactory>()
        .AddScoped<IMappedHeaderRowReader, MappedHeaderRowReader>()
        .AddScoped<IMappedBodyRowReader, MappedBodyRowReader>();

    private static IServiceCollection AddSpreadsheetIOShared(this IServiceCollection services)
        => services
        .AddScoped<IAssemblyWrapperFactory, AssemblyWrapperFactory>()
        .AddScoped<ISpreadsheetDocumentWrapperFactory, SpreadsheetDocumentWrapperFactory>()
        .AddScoped<ISpreadsheetGenerator, SharedStringTableGenerator>()
        .AddScoped<ISpreadsheetGenerator, StylesheetGenerator>()
        .AddScoped<IStringIndexer, StringIndexer>();

    private static IServiceCollection AddSpreadsheetIOStyling(this IServiceCollection services)
        => services
        .AddScoped<IStylesheetMutator, StylesheetBorderMutator>()
        .AddScoped<IStylesheetMutator, StylesheetFillMutator>()
        .AddScoped<IStylesheetMutator, StylesheetFontMutator>()
        .AddScoped<IStylesheetMutator, StylesheetNumericFormatMutator>()
        .AddScoped<IStylesheetMutator, StylesheetStyleMutator>()
        .AddScoped<IBorderIndexer, BorderIndexer>()
        .AddScoped<IFillIndexer, FillIndexer>()
        .AddScoped<IFontIndexer, FontIndexer>()
        .AddScoped<BuiltInNumericFormats>()
        .AddScoped<ConstantNumericFormats>()
        .AddScoped<INumericFormatIndexer, NumericFormatIndexer>()
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
