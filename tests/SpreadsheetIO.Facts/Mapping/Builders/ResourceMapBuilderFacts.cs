using System.Linq.Expressions;
using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Builders;
using LanceC.SpreadsheetIO.Mapping.Options;
using LanceC.SpreadsheetIO.Mapping.Options.Converters;
using LanceC.SpreadsheetIO.Mapping.Options.Registrations;
using LanceC.SpreadsheetIO.Mapping.Validation;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Shared;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Styling;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping.Builders;

public class ResourceMapBuilderFacts
{
    private readonly AutoMocker _mocker = new();

    private ResourceMapBuilder<FakeModel> CreateSystemUnderTest()
        => _mocker.CreateInstance<ResourceMapBuilder<FakeModel>>();

    public class TheBuildMethod : ResourceMapBuilderFacts
    {
        [Fact]
        public void ReturnsSuccessResult()
        {
            // Arrange
            var resourceOptionConverterMock = _mocker
                .GetMock<IMapOptionConverter<IResourceMapOptionRegistration, IResourceMapOption>>();
            resourceOptionConverterMock
                .Setup(converter => converter.ConvertToOption(
                    It.IsAny<ContinueOnResourceReadingFailureResourceMapOption>(),
                    It.IsAny<ResourceMapBuilder>()))
                .Returns((ContinueOnResourceReadingFailureResourceMapOption registration, ResourceMapBuilder _)
                    => MapOptionConversionResult.Success<IResourceMapOption>(registration, registration));

            var propertyOptionConverterMock = _mocker
                .GetMock<IMapOptionConverter<IPropertyMapOptionRegistration, IPropertyMapOption>>();

            var expectedPropertyMap = PropertyMapCreator.CreateForFakeModel(model => model.Id);
            var propertyBuilderMock = _mocker.GetMock<IInternalPropertyMapBuilder<FakeModel, int>>();
            propertyBuilderMock
                .Setup(propertyBuilder => propertyBuilder.Build(
                    propertyOptionConverterMock.Object,
                    It.IsAny<IInternalResourceMapBuilder>()))
                .Returns(PropertyMapResult.Success(expectedPropertyMap));

            _mocker.GetMock<IMapBuilderFactory>()
                .Setup(mapBuilderFactory => mapBuilderFactory.CreateForProperty((FakeModel model) => model.Id))
                .Returns(propertyBuilderMock.Object);

            _mocker.GetMock<IResourceMapBuilderValidator>()
                .Setup(validator => validator.Validate(It.IsAny<IInternalResourceMapBuilder>()))
                .Returns(Array.Empty<ResourceMapBuilderValidationResult>());

            var sut = CreateSystemUnderTest();
            sut.ContinuesOnResourceReadingFailure();
            sut.Property(model => model.Id);

            sut.TryGetRegistration<ContinueOnResourceReadingFailureResourceMapOption>(out var resourceRegistration);

            // Act
            var result = sut.Build(resourceOptionConverterMock.Object, propertyOptionConverterMock.Object);

            // Assert
            Assert.True(result.IsValid);
            Assert.NotNull(result.Value);
            Assert.Null(result.Error);

            Assert.Equal(sut.ResourceType, result.Value!.ResourceType);
            Assert.Equal(resourceRegistration, result.Value.Options.Find<ContinueOnResourceReadingFailureResourceMapOption>());

            var actualPropertyMap = Assert.Single(result.Value.Properties);
            Assert.Equal(expectedPropertyMap, actualPropertyMap);
        }

        [Fact]
        public void ReturnsFailureResultWhenResourceOptionConversionFails()
        {
            // Arrange
            var resourceOptionConverterMock = _mocker
                .GetMock<IMapOptionConverter<IResourceMapOptionRegistration, IResourceMapOption>>();

            var expectedResourceConversionResult = default(MapOptionConversionResult<IResourceMapOption>);
            resourceOptionConverterMock
                .Setup(converter => converter.ConvertToOption(
                    It.IsAny<ContinueOnResourceReadingFailureResourceMapOption>(),
                    It.IsAny<ResourceMapBuilder>()))
                .Returns((ContinueOnResourceReadingFailureResourceMapOption registration, ResourceMapBuilder _)
                    =>
                    {
                        expectedResourceConversionResult = MapOptionConversionResult.Failure<IResourceMapOption>(registration, "foo");
                        return expectedResourceConversionResult;
                    });

            var propertyOptionConverterMock = _mocker
                .GetMock<IMapOptionConverter<IPropertyMapOptionRegistration, IPropertyMapOption>>();

            var expectedPropertyMap = PropertyMapCreator.CreateForFakeModel(model => model.Id);
            var propertyBuilderMock = _mocker.GetMock<IInternalPropertyMapBuilder<FakeModel, int>>();
            propertyBuilderMock
                .Setup(propertyBuilder => propertyBuilder.Build(
                    propertyOptionConverterMock.Object,
                    It.IsAny<IInternalResourceMapBuilder>()))
                .Returns(PropertyMapResult.Success(expectedPropertyMap));

            _mocker.GetMock<IMapBuilderFactory>()
                .Setup(mapBuilderFactory => mapBuilderFactory.CreateForProperty((FakeModel model) => model.Id))
                .Returns(propertyBuilderMock.Object);

            _mocker.GetMock<IResourceMapBuilderValidator>()
                .Setup(validator => validator.Validate(It.IsAny<IInternalResourceMapBuilder>()))
                .Returns(Array.Empty<ResourceMapBuilderValidationResult>());

            var sut = CreateSystemUnderTest();
            sut.ContinuesOnResourceReadingFailure();
            sut.Property(model => model.Id);

            sut.TryGetRegistration<ContinueOnResourceReadingFailureResourceMapOption>(out var resourceRegistration);

            // Act
            var result = sut.Build(resourceOptionConverterMock.Object, propertyOptionConverterMock.Object);

            // Assert
            Assert.False(result.IsValid);
            Assert.Null(result.Value);
            Assert.NotNull(result.Error);

            var actualConversionResult = Assert.Single(result.Error!.Conversions);
            Assert.Equal(expectedResourceConversionResult, actualConversionResult);
        }

        [Fact]
        public void ReturnsFailureResultWhenPropertyMapBuildFails()
        {
            // Arrange
            var resourceOptionConverterMock = _mocker
                .GetMock<IMapOptionConverter<IResourceMapOptionRegistration, IResourceMapOption>>();
            resourceOptionConverterMock
                .Setup(converter => converter.ConvertToOption(
                    It.IsAny<ContinueOnResourceReadingFailureResourceMapOption>(),
                    It.IsAny<ResourceMapBuilder>()))
                .Returns((ContinueOnResourceReadingFailureResourceMapOption registration, ResourceMapBuilder _)
                    => MapOptionConversionResult.Success<IResourceMapOption>(registration, registration));

            var propertyOptionConverterMock = _mocker
                .GetMock<IMapOptionConverter<IPropertyMapOptionRegistration, IPropertyMapOption>>();

            var propertyBuilderMock = _mocker.GetMock<IInternalPropertyMapBuilder<FakeModel, int>>();

            var expectedPropertyConversionResult = MapOptionConversionResult
                .Failure<IPropertyMapOption>(new OptionalPropertyMapOption(PropertyElementKind.All), "foo");
            propertyBuilderMock
                .Setup(propertyBuilder => propertyBuilder.Build(
                    propertyOptionConverterMock.Object,
                    It.IsAny<IInternalResourceMapBuilder>()))
                .Returns(PropertyMapResult.Failure(new PropertyMapError(new[] { expectedPropertyConversionResult, })));

            _mocker.GetMock<IMapBuilderFactory>()
                .Setup(mapBuilderFactory => mapBuilderFactory.CreateForProperty((FakeModel model) => model.Id))
                .Returns(propertyBuilderMock.Object);

            _mocker.GetMock<IResourceMapBuilderValidator>()
                .Setup(validator => validator.Validate(It.IsAny<IInternalResourceMapBuilder>()))
                .Returns(Array.Empty<ResourceMapBuilderValidationResult>());

            var sut = CreateSystemUnderTest();
            sut.ContinuesOnResourceReadingFailure();
            sut.Property(model => model.Id);

            sut.TryGetRegistration<ContinueOnResourceReadingFailureResourceMapOption>(out var resourceRegistration);

            // Act
            var result = sut.Build(resourceOptionConverterMock.Object, propertyOptionConverterMock.Object);

            // Assert
            Assert.False(result.IsValid);
            Assert.Null(result.Value);
            Assert.NotNull(result.Error);

            var actualConversionResult = Assert.Single(result.Error!.Conversions);
            Assert.Equal(expectedPropertyConversionResult, actualConversionResult);
        }

        [Fact]
        public void ReturnsFailureResultWhenValidationFails()
        {
            // Arrange
            var resourceOptionConverterMock = _mocker
                .GetMock<IMapOptionConverter<IResourceMapOptionRegistration, IResourceMapOption>>();
            resourceOptionConverterMock
                .Setup(converter => converter.ConvertToOption(
                    It.IsAny<ContinueOnResourceReadingFailureResourceMapOption>(),
                    It.IsAny<ResourceMapBuilder>()))
                .Returns((ContinueOnResourceReadingFailureResourceMapOption registration, ResourceMapBuilder _)
                    => MapOptionConversionResult.Success<IResourceMapOption>(registration, registration));

            var propertyOptionConverterMock = _mocker
                .GetMock<IMapOptionConverter<IPropertyMapOptionRegistration, IPropertyMapOption>>();

            var expectedPropertyMap = PropertyMapCreator.CreateForFakeModel(model => model.Id);
            var propertyBuilderMock = _mocker.GetMock<IInternalPropertyMapBuilder<FakeModel, int>>();
            propertyBuilderMock
                .Setup(propertyBuilder => propertyBuilder.Build(
                    propertyOptionConverterMock.Object,
                    It.IsAny<IInternalResourceMapBuilder>()))
                .Returns(PropertyMapResult.Success(expectedPropertyMap));

            _mocker.GetMock<IMapBuilderFactory>()
                .Setup(mapBuilderFactory => mapBuilderFactory.CreateForProperty((FakeModel model) => model.Id))
                .Returns(propertyBuilderMock.Object);

            var expectedValidationResult = ResourceMapBuilderValidationResult.Failure("foo");
            _mocker.GetMock<IResourceMapBuilderValidator>()
                .Setup(validator => validator.Validate(It.IsAny<IInternalResourceMapBuilder>()))
                .Returns(new[] { expectedValidationResult, });

            var sut = CreateSystemUnderTest();
            sut.ContinuesOnResourceReadingFailure();
            sut.Property(model => model.Id);

            sut.TryGetRegistration<ContinueOnResourceReadingFailureResourceMapOption>(out var resourceRegistration);

            // Act
            var result = sut.Build(resourceOptionConverterMock.Object, propertyOptionConverterMock.Object);

            // Assert
            Assert.False(result.IsValid);
            Assert.Null(result.Value);
            Assert.NotNull(result.Error);

            var actualValidationResult = Assert.Single(result.Error!.Validations);
            Assert.Equal(expectedValidationResult, actualValidationResult);
        }

        [Fact]
        public void AttemptsToAddResourceOptionRegistrationsToPropertiesWhenApplicable()
        {
            // Arrange
            var resourceOptionConverterMock = _mocker
                .GetMock<IMapOptionConverter<IResourceMapOptionRegistration, IResourceMapOption>>();
            var propertyOptionConverterMock = _mocker
                .GetMock<IMapOptionConverter<IPropertyMapOptionRegistration, IPropertyMapOption>>();

            var expectedIdPropertyMap = PropertyMapCreator.CreateForFakeModel(model => model.Id);
            var idPropertyBuilderMock = _mocker.GetMock<IInternalPropertyMapBuilder<FakeModel, int>>();
            idPropertyBuilderMock
                .Setup(propertyBuilder => propertyBuilder.Build(
                    propertyOptionConverterMock.Object,
                    It.IsAny<IInternalResourceMapBuilder>()))
                .Returns(PropertyMapResult.Success(expectedIdPropertyMap));

            var expectedNamePropertyMap = PropertyMapCreator.CreateForFakeModel(model => model.Name);
            var namePropertyBuilderMock = _mocker.GetMock<IInternalPropertyMapBuilder<FakeModel, string?>>();
            namePropertyBuilderMock
                .Setup(propertyBuilder => propertyBuilder.Build(
                    propertyOptionConverterMock.Object,
                    It.IsAny<IInternalResourceMapBuilder>()))
                .Returns(PropertyMapResult.Success(expectedNamePropertyMap));

            var mapBuilderFactoryMock = _mocker.GetMock<IMapBuilderFactory>();
            mapBuilderFactoryMock.Setup(mapBuilderFactory => mapBuilderFactory.CreateForProperty((FakeModel model) => model.Id))
                .Returns(idPropertyBuilderMock.Object);
            mapBuilderFactoryMock.Setup(mapBuilderFactory => mapBuilderFactory.CreateForProperty((FakeModel model) => model.Name))
                .Returns(namePropertyBuilderMock.Object);

            _mocker.GetMock<IResourceMapBuilderValidator>()
                .Setup(validator => validator.Validate(It.IsAny<IInternalResourceMapBuilder>()))
                .Returns(Array.Empty<ResourceMapBuilderValidationResult>());

            var sut = CreateSystemUnderTest();
            sut.UsesHeaderStyle(BuiltInExcelStyle.Normal);
            sut.Property(model => model.Id);
            sut.Property(model => model.Name);

            sut.TryGetRegistration<HeaderStyleMapOption>(out var registration);

            // Act
            sut.Build(resourceOptionConverterMock.Object, propertyOptionConverterMock.Object);

            // Assert
            idPropertyBuilderMock.Verify(propertyBuilder => propertyBuilder.TryAddRegistration(registration!));
            namePropertyBuilderMock.Verify(propertyBuilder => propertyBuilder.TryAddRegistration(registration!));
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenResourceOptionConverterIsNull()
        {
            // Arrange
            var resourceOptionConverter = default(IMapOptionConverter<IResourceMapOptionRegistration, IResourceMapOption>);
            var propertyOptionConverterMock = _mocker
                .GetMock<IMapOptionConverter<IPropertyMapOptionRegistration, IPropertyMapOption>>();

            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.Build(resourceOptionConverter!, propertyOptionConverterMock.Object));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenPropertyOptionConverterIsNull()
        {
            // Arrange
            var resourceOptionConverterMock = _mocker
                .GetMock<IMapOptionConverter<IResourceMapOptionRegistration, IResourceMapOption>>();
            var propertyOptionConverter = default(IMapOptionConverter<IPropertyMapOptionRegistration, IPropertyMapOption>);

            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.Build(resourceOptionConverterMock.Object, propertyOptionConverter!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheTryGetRegistrationMethod : ResourceMapBuilderFacts
    {
        [Fact]
        public void ReturnsFalseWhenRegistrationIsMissing()
        {
            // Arrange
            var sut = CreateSystemUnderTest();

            // Act
            var hasRegistration = sut.TryGetRegistration<FakeResourceMapOptionRegistration>(out var registration);

            // Assert
            Assert.False(hasRegistration);
            Assert.Null(registration);
        }
    }

    public class TheContinuesOnResourceReadingFailureMethod : ResourceMapBuilderFacts
    {
        [Fact]
        public void AddsOptionRegistration()
        {
            // Arrange
            var sut = CreateSystemUnderTest();

            // Act
            sut.ContinuesOnResourceReadingFailure();

            // Assert
            Assert.True(sut.TryGetRegistration<ContinueOnResourceReadingFailureResourceMapOption>(out var _));
        }
    }

    public class TheHasDefaultPropertyReadingResolutionsMethod : ResourceMapBuilderFacts
    {
        [Fact]
        public void AddsOptionRegistration()
        {
            // Arrange
            var resolutions = new[]
            {
                ResourcePropertyDefaultReadingResolution.Empty,
                ResourcePropertyDefaultReadingResolution.Missing,
            };

            var sut = CreateSystemUnderTest();

            // Act
            sut.HasDefaultPropertyReadingResolutions(resolutions);

            // Assert
            Assert.True(sut.TryGetRegistration<DefaultPropertyReadingResolutionsResourceMapOptionRegistration>(out var registration));
            Assert.Equal(resolutions, registration!.Resolutions);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenResolutionCollectionIsEmpty()
        {
            // Arrange
            var resolutions = Array.Empty<ResourcePropertyDefaultReadingResolution>();
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.HasDefaultPropertyReadingResolutions(resolutions));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenResolutionCollectionIsNull()
        {
            // Arrange
            var resolutions = default(ResourcePropertyDefaultReadingResolution[]);
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.HasDefaultPropertyReadingResolutions(resolutions!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenAnyResolutionIsNull()
        {
            // Arrange
            var resolutions = new[] { ResourcePropertyDefaultReadingResolution.Empty, default, };
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.HasDefaultPropertyReadingResolutions(resolutions!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheHasHeaderRowNumberMethod : ResourceMapBuilderFacts
    {
        [Fact]
        public void AddsOptionRegistration()
        {
            // Arrange
            var number = 2U;
            var sut = CreateSystemUnderTest();

            // Act
            sut.HasHeaderRowNumber(number);

            // Assert
            Assert.True(sut.TryGetRegistration<HeaderRowNumberResourceMapOption>(out var registration));
            Assert.Equal(number, registration!.Number);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenNumberIsZero()
        {
            // Arrange
            var number = 0U;
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.HasHeaderRowNumber(number));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }
    }

    public class ThePropertyMethod : ResourceMapBuilderFacts
    {
        [Fact]
        public void AddsPropertyMapBuilder()
        {
            // Arrange
            var propertyMapBuilderMock = _mocker.GetMock<IInternalPropertyMapBuilder<FakeModel, int>>();

            _mocker.GetMock<IMapBuilderFactory>()
                .Setup(mapBuilderFactory => mapBuilderFactory.CreateForProperty((FakeModel model) => model.Id))
                .Returns(propertyMapBuilderMock.Object);

            var sut = CreateSystemUnderTest();

            // Act
            sut.Property(model => model.Id);

            // Assert
            var propertyMapBuilder = Assert.Single(sut.Properties);
            Assert.Equal(propertyMapBuilderMock.Object, propertyMapBuilder);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenPropertyExpressionIsNull()
        {
            // Arrange
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.Property(default(Expression<Func<FakeModel, int>>)!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheUsesHeaderStyleMethodWithCustomStyleParameters : ResourceMapBuilderFacts
    {
        [Fact]
        public void AddsOptionRegistration()
        {
            // Arrange
            var style = Style.Default;
            var name = "foo";
            var sut = CreateSystemUnderTest();

            // Act
            sut.UsesHeaderStyle(style, name);

            // Assert
            Assert.True(sut.TryGetRegistration<HeaderStyleMapOption>(out var registration));
            Assert.Equal(IndexerKeyKind.Custom, registration!.Key.Kind);
            Assert.Equal(name, registration.Key.Name);
            Assert.Equal(style, registration.Style);
        }

        [Fact]
        public void UsesGuidWhenNameIsEmpty()
        {
            // Arrange
            var style = Style.Default;
            var name = string.Empty;
            var sut = CreateSystemUnderTest();

            // Act
            sut.UsesHeaderStyle(style, name);

            // Assert
            Assert.True(sut.TryGetRegistration<HeaderStyleMapOption>(out var registration));
            Assert.Equal(IndexerKeyKind.Custom, registration!.Key.Kind);
            Assert.True(Guid.TryParse(registration.Key.Name, out var _));
            Assert.Equal(style, registration.Style);
        }

        [Fact]
        public void UsesGuidWhenNameIsNull()
        {
            // Arrange
            var style = Style.Default;
            var name = default(string);
            var sut = CreateSystemUnderTest();

            // Act
            sut.UsesHeaderStyle(style, name!);

            // Assert
            Assert.True(sut.TryGetRegistration<HeaderStyleMapOption>(out var registration));
            Assert.Equal(IndexerKeyKind.Custom, registration!.Key.Kind);
            Assert.True(Guid.TryParse(registration.Key.Name, out var _));
            Assert.Equal(style, registration.Style);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenStyleIsNull()
        {
            // Arrange
            var style = default(Style);
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.UsesHeaderStyle(style!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheUsesHeaderStyleMethodWithExcelStyleParameters : ResourceMapBuilderFacts
    {
        [Fact]
        public void AddsOptionRegistration()
        {
            // Arrange
            var style = BuiltInExcelStyle.Normal;
            var sut = CreateSystemUnderTest();

            // Act
            sut.UsesHeaderStyle(style);

            // Assert
            Assert.True(sut.TryGetRegistration<HeaderStyleMapOption>(out var registration));
            Assert.Equal(style.IndexerKey, registration!.Key);
            Assert.Equal(style.Style, registration.Style);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenStyleIsNull()
        {
            // Arrange
            var style = default(BuiltInExcelStyle);
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.UsesHeaderStyle(style!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheUsesHeaderStyleMethodWithPackageStyleParameters : ResourceMapBuilderFacts
    {
        [Fact]
        public void AddsOptionRegistration()
        {
            // Arrange
            var style = BuiltInPackageStyle.Bold;
            var sut = CreateSystemUnderTest();

            // Act
            sut.UsesHeaderStyle(style);

            // Assert
            Assert.True(sut.TryGetRegistration<HeaderStyleMapOption>(out var registration));
            Assert.Equal(style.IndexerKey, registration!.Key);
            Assert.Equal(style.Style, registration.Style);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenStyleIsNull()
        {
            // Arrange
            var style = default(BuiltInPackageStyle);
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.UsesHeaderStyle(style!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheUsesBodyStyleMethodWithCustomStyleParameters : ResourceMapBuilderFacts
    {
        [Fact]
        public void AddsOptionRegistration()
        {
            // Arrange
            var style = Style.Default;
            var name = "foo";
            var sut = CreateSystemUnderTest();

            // Act
            sut.UsesBodyStyle(style, name);

            // Assert
            Assert.True(sut.TryGetRegistration<BodyStyleMapOption>(out var registration));
            Assert.Equal(IndexerKeyKind.Custom, registration!.Key.Kind);
            Assert.Equal(name, registration.Key.Name);
            Assert.Equal(style, registration.Style);
        }

        [Fact]
        public void UsesGuidWhenNameIsEmpty()
        {
            // Arrange
            var style = Style.Default;
            var name = string.Empty;
            var sut = CreateSystemUnderTest();

            // Act
            sut.UsesBodyStyle(style, name);

            // Assert
            Assert.True(sut.TryGetRegistration<BodyStyleMapOption>(out var registration));
            Assert.Equal(IndexerKeyKind.Custom, registration!.Key.Kind);
            Assert.True(Guid.TryParse(registration.Key.Name, out var _));
            Assert.Equal(style, registration.Style);
        }

        [Fact]
        public void UsesGuidWhenNameIsNull()
        {
            // Arrange
            var style = Style.Default;
            var name = default(string);
            var sut = CreateSystemUnderTest();

            // Act
            sut.UsesBodyStyle(style, name!);

            // Assert
            Assert.True(sut.TryGetRegistration<BodyStyleMapOption>(out var registration));
            Assert.Equal(IndexerKeyKind.Custom, registration!.Key.Kind);
            Assert.True(Guid.TryParse(registration.Key.Name, out var _));
            Assert.Equal(style, registration.Style);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenStyleIsNull()
        {
            // Arrange
            var style = default(Style);
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.UsesBodyStyle(style!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheUsesBodyStyleMethodWithExcelStyleParameters : ResourceMapBuilderFacts
    {
        [Fact]
        public void AddsOptionRegistration()
        {
            // Arrange
            var style = BuiltInExcelStyle.Normal;
            var sut = CreateSystemUnderTest();

            // Act
            sut.UsesBodyStyle(style);

            // Assert
            Assert.True(sut.TryGetRegistration<BodyStyleMapOption>(out var registration));
            Assert.Equal(style.IndexerKey, registration!.Key);
            Assert.Equal(style.Style, registration.Style);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenStyleIsNull()
        {
            // Arrange
            var style = default(BuiltInExcelStyle);
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.UsesBodyStyle(style!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheUsesBodyStyleMethodWithPackageStyleParameters : ResourceMapBuilderFacts
    {
        [Fact]
        public void AddsOptionRegistration()
        {
            // Arrange
            var style = BuiltInPackageStyle.Bold;
            var sut = CreateSystemUnderTest();

            // Act
            sut.UsesBodyStyle(style);

            // Assert
            Assert.True(sut.TryGetRegistration<BodyStyleMapOption>(out var registration));
            Assert.Equal(style.IndexerKey, registration!.Key);
            Assert.Equal(style.Style, registration.Style);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenStyleIsNull()
        {
            // Arrange
            var style = default(BuiltInPackageStyle);
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.UsesBodyStyle(style!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheUsesDateKindMethod : ResourceMapBuilderFacts
    {
        [Fact]
        public void AddsOptionRegistration()
        {
            // Arrange
            var dateKind = CellDateKind.Text;
            var sut = CreateSystemUnderTest();

            // Act
            sut.UsesDateKind(dateKind);

            // Assert
            Assert.True(sut.TryGetRegistration<DateKindMapOption>(out var registration));
            Assert.Equal(dateKind, registration!.DateKind);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenDateKindIsNull()
        {
            // Arrange
            var dateKind = default(CellDateKind);
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.UsesDateKind(dateKind!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheUsesStringKindMethod : ResourceMapBuilderFacts
    {
        [Fact]
        public void AddsOptionRegistration()
        {
            // Arrange
            var stringKind = CellStringKind.InlineString;
            var sut = CreateSystemUnderTest();

            // Act
            sut.UsesStringKind(stringKind);

            // Assert
            Assert.True(sut.TryGetRegistration<StringKindMapOption>(out var registration));
            Assert.Equal(stringKind, registration!.StringKind);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenStringKindIsNull()
        {
            // Arrange
            var stringKind = default(CellStringKind);
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.UsesStringKind(stringKind!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheUsesExplicitConstructorMethod : ResourceMapBuilderFacts
    {
        [Fact]
        public void AddsOptionRegistration()
        {
            // Arrange
            var expectedPropertyName = "foo";
            var sut = CreateSystemUnderTest();

            // Act
            sut.UsesExplicitConstructor(expectedPropertyName);

            // Assert
            Assert.True(sut.TryGetRegistration<ExplicitConstructorResourceMapOptionRegistration>(out var registration));
            var actualPropertyName = Assert.Single(registration!.PropertyNames);
            Assert.Equal(expectedPropertyName, actualPropertyName);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenPropertyNameCollectionIsEmpty()
        {
            // Arrange
            var propertyNames = Array.Empty<string>();
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.UsesExplicitConstructor(propertyNames));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenAnyPropertyNameIsEmpty()
        {
            // Arrange
            var propertyNames = new[] { "foo", string.Empty, };
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.UsesExplicitConstructor(propertyNames));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenPropertyNameCollectionIsNull()
        {
            // Arrange
            var propertyNames = default(string[]);
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.UsesExplicitConstructor(propertyNames!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenAnyPropertyNameIsNull()
        {
            // Arrange
            var propertyNames = new[] { "foo", default, };
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.UsesExplicitConstructor(propertyNames!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheUsesImplicitConstructorMethod : ResourceMapBuilderFacts
    {
        [Fact]
        public void AddsOptionRegistration()
        {
            // Arrange
            var sut = CreateSystemUnderTest();

            // Act
            sut.UsesImplicitConstructor();

            // Assert
            Assert.True(sut.TryGetRegistration<ImplicitConstructorResourceMapOptionRegistration>(out var _));
        }
    }
}
