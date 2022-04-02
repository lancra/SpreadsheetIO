using System.Reflection;
using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Builders;
using LanceC.SpreadsheetIO.Mapping.Options;
using LanceC.SpreadsheetIO.Mapping.Options.Converters;
using LanceC.SpreadsheetIO.Mapping.Options.Registrations;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping.Builders;

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
            var configurationMock = new FakeModelMapConfiguration();

            var resourceBuilderMock = _mocker.GetMock<IInternalResourceMapBuilder<FakeModel>>();

            var expectedResourceType = typeof(FakeModel);
            resourceBuilderMock.SetupGet(resourceBuilder => resourceBuilder.ResourceType)
                .Returns(expectedResourceType);

            var expectedResourceMapResult = ResourceMapResult.Success(
                    ResourceMapCreator.Create<FakeModel>(Array.Empty<PropertyMap>()));
            resourceBuilderMock
                .Setup(resourceBuilder => resourceBuilder.Build(
                    It.IsAny<IMapOptionConverter<IResourceMapOptionRegistration, IResourceMapOption>>(),
                    It.IsAny<IMapOptionConverter<IPropertyMapOptionRegistration, IPropertyMapOption>>()))
                .Returns(expectedResourceMapResult);

            _mocker.GetMock<IMapBuilderFactory>()
                .Setup(mapBuilderFactory => mapBuilderFactory.CreateForResource<FakeModel>())
                .Returns(resourceBuilderMock.Object);

            var sut = CreateSystemUnderTest();

            // Act
            sut.ApplyConfiguration(configurationMock);

            // Assert
            var resourceMapResultEntry = Assert.Single(sut.ResourceMaps);
            Assert.Equal(expectedResourceType, resourceMapResultEntry.Key);
            Assert.Equal(expectedResourceMapResult, resourceMapResultEntry.Value);

            Assert.True(configurationMock.Configured);
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

            var resourceBuilderMock = _mocker.GetMock<IInternalResourceMapBuilder<FakeModel>>();

            var expectedResourceType = typeof(FakeModel);
            resourceBuilderMock.SetupGet(resourceBuilder => resourceBuilder.ResourceType)
                .Returns(expectedResourceType);

            var expectedResourceMapResult = ResourceMapResult.Success(
                    ResourceMapCreator.Create<FakeModel>(Array.Empty<PropertyMap>()));
            resourceBuilderMock
                .Setup(resourceBuilder => resourceBuilder.Build(
                    It.IsAny<IMapOptionConverter<IResourceMapOptionRegistration, IResourceMapOption>>(),
                    It.IsAny<IMapOptionConverter<IPropertyMapOptionRegistration, IPropertyMapOption>>()))
                .Returns(expectedResourceMapResult);

            _mocker.GetMock<IMapBuilderFactory>()
                .Setup(mapBuilderFactory => mapBuilderFactory.CreateForResource<FakeModel>())
                .Returns(resourceBuilderMock.Object);

            var sut = CreateSystemUnderTest();

            // Act
            sut.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Assert
            var resourceMapResultEntry = Assert.Single(sut.ResourceMaps);
            Assert.Equal(expectedResourceType, resourceMapResultEntry.Key);
            Assert.Equal(expectedResourceMapResult, resourceMapResultEntry.Value);
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

            var resourceBuilderMock = _mocker.GetMock<IInternalResourceMapBuilder<FakeModel>>();

            var expectedResourceType = typeof(FakeModel);
            resourceBuilderMock.SetupGet(resourceBuilder => resourceBuilder.ResourceType)
                .Returns(expectedResourceType);

            var expectedResourceMapResult = ResourceMapResult.Success(
                    ResourceMapCreator.Create<FakeModel>(Array.Empty<PropertyMap>()));
            resourceBuilderMock
                .Setup(resourceBuilder => resourceBuilder.Build(
                    It.IsAny<IMapOptionConverter<IResourceMapOptionRegistration, IResourceMapOption>>(),
                    It.IsAny<IMapOptionConverter<IPropertyMapOptionRegistration, IPropertyMapOption>>()))
                .Returns(expectedResourceMapResult);

            _mocker.GetMock<IMapBuilderFactory>()
                .Setup(mapBuilderFactory => mapBuilderFactory.CreateForResource<FakeModel>())
                .Returns(resourceBuilderMock.Object);

            var sut = CreateSystemUnderTest();

            // Act
            sut.ApplyConfigurationsFromAssembly(markerType);

            // Assert
            var resourceMapResultEntry = Assert.Single(sut.ResourceMaps);
            Assert.Equal(expectedResourceType, resourceMapResultEntry.Key);
            Assert.Equal(expectedResourceMapResult, resourceMapResultEntry.Value);
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
            var configurationMock = new FakeModelMapConfiguration();

            var resourceBuilderMock = _mocker.GetMock<IInternalResourceMapBuilder<FakeModel>>();

            var expectedResourceType = typeof(FakeModel);
            resourceBuilderMock.SetupGet(resourceBuilder => resourceBuilder.ResourceType)
                .Returns(expectedResourceType);

            var expectedResourceMapResult = ResourceMapResult.Success(
                    ResourceMapCreator.Create<FakeModel>(Array.Empty<PropertyMap>()));
            resourceBuilderMock
                .Setup(resourceBuilder => resourceBuilder.Build(
                    It.IsAny<IMapOptionConverter<IResourceMapOptionRegistration, IResourceMapOption>>(),
                    It.IsAny<IMapOptionConverter<IPropertyMapOptionRegistration, IPropertyMapOption>>()))
                .Returns(expectedResourceMapResult);

            _mocker.GetMock<IMapBuilderFactory>()
                .Setup(mapBuilderFactory => mapBuilderFactory.CreateForResource<FakeModel>())
                .Returns(resourceBuilderMock.Object);

            var sut = CreateSystemUnderTest();

            // Act
            sut.Configure<FakeModel>(builder => configurationMock.Configure(builder));

            // Assert
            var resourceMapResultEntry = Assert.Single(sut.ResourceMaps);
            Assert.Equal(expectedResourceType, resourceMapResultEntry.Key);
            Assert.Equal(expectedResourceMapResult, resourceMapResultEntry.Value);

            Assert.True(configurationMock.Configured);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenBuilderActionIsNull()
        {
            // Arrange
            var builderAction = default(Action<IResourceMapBuilder<FakeModel>>);
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.Configure(builderAction!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }
}
