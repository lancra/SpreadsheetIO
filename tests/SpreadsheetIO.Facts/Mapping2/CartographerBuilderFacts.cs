using System.Reflection;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping2;
using LanceC.SpreadsheetIO.Mapping2.Options;
using LanceC.SpreadsheetIO.Mapping2.Options.Converters;
using LanceC.SpreadsheetIO.Mapping2.Options.Registrations;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping2;

public class CartographerBuilderFacts
{
    private readonly AutoMocker _mocker = new();

    private CartographerBuilder CreateSystemUnderTest()
        => _mocker.CreateInstance<CartographerBuilder>();

    public class TheApplyConfigurationMethod : CartographerBuilderFacts
    {
        [Fact]
        public void RegistersResult()
        {
            // Arrange
            var configuration = new FakeModelMapConfiguration();

            var resourceOptionConverterMock = _mocker
                .GetMock<IMapOptionConverter<IResourceMapOptionRegistration, IResourceMapOption>>();
            resourceOptionConverterMock
                .Setup(converter => converter.ConvertToOption(
                    It.IsAny<ExitOnResourceReadingFailureResourceMapOption>(),
                    It.IsAny<ResourceMapBuilder>()))
                .Returns((ExitOnResourceReadingFailureResourceMapOption registration, ResourceMapBuilder _)
                    => MapOptionConversionResult.Success<IResourceMapOption>(registration, registration));

            var propertyOptionConverterMock = _mocker
                .GetMock<IMapOptionConverter<IPropertyMapOptionRegistration, IPropertyMapOption>>();
            propertyOptionConverterMock
                .Setup(converter => converter.ConvertToOption(It.IsAny<HeaderStyleMapOption>(), It.IsAny<ResourceMapBuilder>()))
                .Returns((HeaderStyleMapOption registration, ResourceMapBuilder _)
                    => MapOptionConversionResult.Success<IPropertyMapOption>(registration, registration));
            propertyOptionConverterMock
                .Setup(converter => converter.ConvertToOption(It.IsAny<BodyStyleMapOption>(), It.IsAny<ResourceMapBuilder>()))
                .Returns((BodyStyleMapOption registration, ResourceMapBuilder _)
                    => MapOptionConversionResult.Success<IPropertyMapOption>(registration, registration));

            var sut = CreateSystemUnderTest();

            // Act
            sut.ApplyConfiguration(configuration);

            // Assert
            var resourceMapResultEntry = Assert.Single(sut.ResourceMaps);
            Assert.Equal(typeof(FakeModel), resourceMapResultEntry.Key);
            Assert.True(resourceMapResultEntry.Value.IsValid);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenConfigurationIsNull()
        {
            // Arrange
            var configuration = default(IResourceMapConfiguration<FakeModel>);
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.ApplyConfiguration(configuration!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheApplyConfigurationsFromAssemblyMethodWithAssemblyParameter : CartographerBuilderFacts
    {
        [Fact]
        public void RegistersResult()
        {
            // Arrange
            var assemblyMock = _mocker.GetMock<IAssemblyWrapper>();
            assemblyMock.Setup(assembly => assembly.GetConstructibleTypes())
                .Returns(new[] { typeof(FakeModelMapConfiguration).GetTypeInfo(), });

            _mocker.GetMock<IAssemblyWrapperFactory>()
                .Setup(assemblyFactory => assemblyFactory.Create(It.IsAny<Assembly>()))
                .Returns(assemblyMock.Object);

            var resourceOptionConverterMock = _mocker
                .GetMock<IMapOptionConverter<IResourceMapOptionRegistration, IResourceMapOption>>();
            resourceOptionConverterMock
                .Setup(converter => converter.ConvertToOption(
                    It.IsAny<ExitOnResourceReadingFailureResourceMapOption>(),
                    It.IsAny<ResourceMapBuilder>()))
                .Returns((ExitOnResourceReadingFailureResourceMapOption registration, ResourceMapBuilder _)
                    => MapOptionConversionResult.Success<IResourceMapOption>(registration, registration));

            var propertyOptionConverterMock = _mocker
                .GetMock<IMapOptionConverter<IPropertyMapOptionRegistration, IPropertyMapOption>>();
            propertyOptionConverterMock
                .Setup(converter => converter.ConvertToOption(It.IsAny<HeaderStyleMapOption>(), It.IsAny<ResourceMapBuilder>()))
                .Returns((HeaderStyleMapOption registration, ResourceMapBuilder _)
                    => MapOptionConversionResult.Success<IPropertyMapOption>(registration, registration));
            propertyOptionConverterMock
                .Setup(converter => converter.ConvertToOption(It.IsAny<BodyStyleMapOption>(), It.IsAny<ResourceMapBuilder>()))
                .Returns((BodyStyleMapOption registration, ResourceMapBuilder _)
                    => MapOptionConversionResult.Success<IPropertyMapOption>(registration, registration));

            var sut = CreateSystemUnderTest();

            // Act
            sut.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Assert
            var resourceMapResultEntry = Assert.Single(sut.ResourceMaps);
            Assert.Equal(typeof(FakeModel), resourceMapResultEntry.Key);
            Assert.True(resourceMapResultEntry.Value.IsValid);
        }

        [Fact]
        public void SkipsTypeWithoutEmptyConstructor()
        {
            // Arrange
            var assemblyMock = _mocker.GetMock<IAssemblyWrapper>();
            assemblyMock.Setup(assembly => assembly.GetConstructibleTypes())
                .Returns(new[] { typeof(CartographerBuilder).GetTypeInfo(), });

            _mocker.GetMock<IAssemblyWrapperFactory>()
                .Setup(assemblyFactory => assemblyFactory.Create(It.IsAny<Assembly>()))
                .Returns(assemblyMock.Object);

            var sut = CreateSystemUnderTest();

            // Act
            sut.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Assert
            Assert.Empty(sut.ResourceMaps);
        }

        [Fact]
        public void SkipsNonGenericInterfaces()
        {
            // Arrange
            var assemblyMock = _mocker.GetMock<IAssemblyWrapper>();
            assemblyMock.Setup(assembly => assembly.GetConstructibleTypes())
                .Returns(new[] { typeof(FakeResourceMapOptionRegistration).GetTypeInfo(), });

            _mocker.GetMock<IAssemblyWrapperFactory>()
                .Setup(assemblyFactory => assemblyFactory.Create(It.IsAny<Assembly>()))
                .Returns(assemblyMock.Object);

            var sut = CreateSystemUnderTest();

            // Act
            sut.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Assert
            Assert.Empty(sut.ResourceMaps);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenAssemblyIsNull()
        {
            // Arrange
            var assembly = default(Assembly);
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.ApplyConfigurationsFromAssembly(assembly!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheApplyConfigurationsFromAssemblyMethodWithTypeParameter : CartographerBuilderFacts
    {
        [Fact]
        public void RegistersResult()
        {
            // Arrange
            var markerType = typeof(string);

            var assemblyMock = _mocker.GetMock<IAssemblyWrapper>();
            assemblyMock.Setup(assembly => assembly.GetConstructibleTypes())
                .Returns(new[] { typeof(FakeModelMapConfiguration).GetTypeInfo(), });

            _mocker.GetMock<IAssemblyWrapperFactory>()
                .Setup(assemblyFactory => assemblyFactory.Create(markerType))
                .Returns(assemblyMock.Object);

            var resourceOptionConverterMock = _mocker
                .GetMock<IMapOptionConverter<IResourceMapOptionRegistration, IResourceMapOption>>();
            resourceOptionConverterMock
                .Setup(converter => converter.ConvertToOption(
                    It.IsAny<ExitOnResourceReadingFailureResourceMapOption>(),
                    It.IsAny<ResourceMapBuilder>()))
                .Returns((ExitOnResourceReadingFailureResourceMapOption registration, ResourceMapBuilder _)
                    => MapOptionConversionResult.Success<IResourceMapOption>(registration, registration));

            var propertyOptionConverterMock = _mocker
                .GetMock<IMapOptionConverter<IPropertyMapOptionRegistration, IPropertyMapOption>>();
            propertyOptionConverterMock
                .Setup(converter => converter.ConvertToOption(It.IsAny<HeaderStyleMapOption>(), It.IsAny<ResourceMapBuilder>()))
                .Returns((HeaderStyleMapOption registration, ResourceMapBuilder _)
                    => MapOptionConversionResult.Success<IPropertyMapOption>(registration, registration));
            propertyOptionConverterMock
                .Setup(converter => converter.ConvertToOption(It.IsAny<BodyStyleMapOption>(), It.IsAny<ResourceMapBuilder>()))
                .Returns((BodyStyleMapOption registration, ResourceMapBuilder _)
                    => MapOptionConversionResult.Success<IPropertyMapOption>(registration, registration));

            var sut = CreateSystemUnderTest();

            // Act
            sut.ApplyConfigurationsFromAssembly(markerType);

            // Assert
            var resourceMapResultEntry = Assert.Single(sut.ResourceMaps);
            Assert.Equal(typeof(FakeModel), resourceMapResultEntry.Key);
            Assert.True(resourceMapResultEntry.Value.IsValid);
        }

        [Fact]
        public void SkipsTypeWithoutEmptyConstructor()
        {
            // Arrange
            var markerType = typeof(string);

            var assemblyMock = _mocker.GetMock<IAssemblyWrapper>();
            assemblyMock.Setup(assembly => assembly.GetConstructibleTypes())
                .Returns(new[] { typeof(CartographerBuilder).GetTypeInfo(), });

            _mocker.GetMock<IAssemblyWrapperFactory>()
                .Setup(assemblyFactory => assemblyFactory.Create(markerType))
                .Returns(assemblyMock.Object);

            var sut = CreateSystemUnderTest();

            // Act
            sut.ApplyConfigurationsFromAssembly(markerType);

            // Assert
            Assert.Empty(sut.ResourceMaps);
        }

        [Fact]
        public void SkipsNonGenericInterfaces()
        {
            // Arrange
            var markerType = typeof(string);

            var assemblyMock = _mocker.GetMock<IAssemblyWrapper>();
            assemblyMock.Setup(assembly => assembly.GetConstructibleTypes())
                .Returns(new[] { typeof(FakeResourceMapOptionRegistration).GetTypeInfo(), });

            _mocker.GetMock<IAssemblyWrapperFactory>()
                .Setup(assemblyFactory => assemblyFactory.Create(markerType))
                .Returns(assemblyMock.Object);

            var sut = CreateSystemUnderTest();

            // Act
            sut.ApplyConfigurationsFromAssembly(markerType);

            // Assert
            Assert.Empty(sut.ResourceMaps);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenMarkerTypeIsNull()
        {
            // Arrange
            var markerType = default(Type);
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.ApplyConfigurationsFromAssembly(markerType!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheConfigureMethod : CartographerBuilderFacts
    {
        [Fact]
        public void RegistersResult()
        {
            // Arrange
            var resourceOptionConverterMock = _mocker
                .GetMock<IMapOptionConverter<IResourceMapOptionRegistration, IResourceMapOption>>();
            resourceOptionConverterMock
                .Setup(converter => converter.ConvertToOption(
                    It.IsAny<ExitOnResourceReadingFailureResourceMapOption>(),
                    It.IsAny<ResourceMapBuilder>()))
                .Returns((ExitOnResourceReadingFailureResourceMapOption registration, ResourceMapBuilder _)
                    => MapOptionConversionResult.Success<IResourceMapOption>(registration, registration));

            var propertyOptionConverterMock = _mocker
                .GetMock<IMapOptionConverter<IPropertyMapOptionRegistration, IPropertyMapOption>>();
            propertyOptionConverterMock
                .Setup(converter => converter.ConvertToOption(It.IsAny<HeaderStyleMapOption>(), It.IsAny<ResourceMapBuilder>()))
                .Returns((HeaderStyleMapOption registration, ResourceMapBuilder _)
                    => MapOptionConversionResult.Success<IPropertyMapOption>(registration, registration));
            propertyOptionConverterMock
                .Setup(converter => converter.ConvertToOption(It.IsAny<BodyStyleMapOption>(), It.IsAny<ResourceMapBuilder>()))
                .Returns((BodyStyleMapOption registration, ResourceMapBuilder _)
                    => MapOptionConversionResult.Success<IPropertyMapOption>(registration, registration));

            var sut = CreateSystemUnderTest();

            // Act
            sut.Configure<FakeModel>(builder => new FakeModelMapConfiguration().Configure(builder));

            // Assert
            var resourceMapResultEntry = Assert.Single(sut.ResourceMaps);
            Assert.Equal(typeof(FakeModel), resourceMapResultEntry.Key);
            Assert.True(resourceMapResultEntry.Value.IsValid);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenBuilderActionIsNull()
        {
            // Arrange
            var builderAction = default(Action<ResourceMapBuilder<FakeModel>>);
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.Configure(builderAction!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }
}
