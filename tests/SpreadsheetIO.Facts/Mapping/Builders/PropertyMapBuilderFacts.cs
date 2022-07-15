using System.Linq.Expressions;
using System.Reflection;
using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Mapping.Builders;
using LanceC.SpreadsheetIO.Mapping.Options;
using LanceC.SpreadsheetIO.Mapping.Options.Converters;
using LanceC.SpreadsheetIO.Mapping.Options.Registrations;
using LanceC.SpreadsheetIO.Properties;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Shared;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Styling;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Mapping.Builders;

public class PropertyMapBuilderFacts
{
    private readonly AutoMocker _mocker = new();

    private PropertyMapBuilder<FakeModel, TProperty> CreateSystemUnderTest<TProperty>(
        Expression<Func<FakeModel, TProperty>> propertyExpression)
    {
        _mocker.Use(propertyExpression);
        return _mocker.CreateInstance<PropertyMapBuilder<FakeModel, TProperty>>(true);
    }

    public class TheConstructor : PropertyMapBuilderFacts
    {
        [Fact]
        public void StoresPropertyInfo()
        {
            // Act
            var sut = CreateSystemUnderTest(model => model.Id);

            // Assert
            Assert.Equal(typeof(FakeModel).GetProperty(nameof(FakeModel.Id)), sut.PropertyInfo);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenPropertyIsNotMemberExpression()
        {
            // Act
            var exception = Record.Exception(() => CreateSystemUnderTest(model => true));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void ThrowsArgumentExceptionWhenPropertyIsNotPropertyInfo()
        {
            // Act
            var exception = Record.Exception(() => CreateSystemUnderTest(model => model.Field));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }
    }

    public class TheBuildMethod : PropertyMapBuilderFacts
    {
        [Fact]
        public void ReturnsSuccessResult()
        {
            // Arrange
            var propertyOptionConverterMock = _mocker
                .GetMock<IMapOptionConverter<IPropertyMapOptionRegistration, IPropertyMapOption>>();
            var resourceMapBuilderMock = _mocker.GetMock<IInternalResourceMapBuilder>();

            propertyOptionConverterMock
                .Setup(converter => converter.ConvertToOption(It.IsAny<HeaderStyleMapOption>(), resourceMapBuilderMock.Object))
                .Returns((HeaderStyleMapOption registration, IInternalResourceMapBuilder _)
                    => MapOptionConversionResult.Success<IPropertyMapOption>(registration, registration));

            var expectedKey = PropertyMapKeyCreator.Create(name: nameof(FakeModel.Id));
            var propertyMapKeyBuilderMock = _mocker.GetMock<IInternalPropertyMapKeyBuilder>();
            propertyMapKeyBuilderMock.SetupGet(builder => builder.Key)
                .Returns(expectedKey);

            _mocker.GetMock<IMapBuilderFactory>()
                .Setup(mapBuilderFactory => mapBuilderFactory.CreateForPropertyKey(It.IsAny<PropertyInfo>()))
                .Returns(propertyMapKeyBuilderMock.Object);

            var sut = CreateSystemUnderTest(model => model.Id);
            sut.UsesHeaderStyle(BuiltInExcelStyle.Normal);
            sut.TryGetRegistration<HeaderStyleMapOption>(out var registration);

            // Act
            var result = sut.Build(propertyOptionConverterMock.Object, resourceMapBuilderMock.Object);

            // Assert
            Assert.True(result.IsValid);
            Assert.NotNull(result.Value);
            Assert.Null(result.Error);

            Assert.Equal(sut.PropertyInfo, result.Value!.Property);
            Assert.Equal(expectedKey, result.Value.Key);
            Assert.Equal(registration, result.Value.Options.Find<HeaderStyleMapOption>());
        }

        [Fact]
        public void ReturnsFailureResultWhenConversionFails()
        {
            // Arrange
            var propertyOptionConverterMock = _mocker
                .GetMock<IMapOptionConverter<IPropertyMapOptionRegistration, IPropertyMapOption>>();
            var resourceMapBuilderMock = _mocker.GetMock<IInternalResourceMapBuilder>();

            var expectedConversionResult = default(MapOptionConversionResult<IPropertyMapOption>);
            propertyOptionConverterMock
                .Setup(converter => converter.ConvertToOption(It.IsAny<HeaderStyleMapOption>(), resourceMapBuilderMock.Object))
                .Returns((HeaderStyleMapOption registration, IInternalResourceMapBuilder _)
                    =>
                    {
                        expectedConversionResult = MapOptionConversionResult.Failure<IPropertyMapOption>(registration, "foo");
                        return expectedConversionResult;
                    });

            var sut = CreateSystemUnderTest(model => model.Id);
            sut.UsesHeaderStyle(BuiltInExcelStyle.Normal);
            sut.TryGetRegistration<HeaderStyleMapOption>(out var registration);

            // Act
            var result = sut.Build(propertyOptionConverterMock.Object, resourceMapBuilderMock.Object);

            // Assert
            Assert.False(result.IsValid);
            Assert.Null(result.Value);
            Assert.NotNull(result.Error);

            var actualConversionResult = Assert.Single(result.Error!.Conversions);
            Assert.Equal(expectedConversionResult, actualConversionResult);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenPropertyOptionConverterIsNull()
        {
            // Arrange
            var propertyOptionConverter = default(IMapOptionConverter<IPropertyMapOptionRegistration, IPropertyMapOption>);
            var resourceMapBuilderMock = _mocker.GetMock<IInternalResourceMapBuilder>();

            var sut = CreateSystemUnderTest(model => model.Id);

            // Act
            var exception = Record.Exception(() => sut.Build(propertyOptionConverter!, resourceMapBuilderMock.Object));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenResourceMapBuilderIsNull()
        {
            // Arrange
            var propertyOptionConverterMock = _mocker
                .GetMock<IMapOptionConverter<IPropertyMapOptionRegistration, IPropertyMapOption>>();
            var resourceMapBuilder = default(IInternalResourceMapBuilder);

            var sut = CreateSystemUnderTest(model => model.Id);

            // Act
            var exception = Record.Exception(() => sut.Build(propertyOptionConverterMock.Object, resourceMapBuilder!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheTryAddRegistrationMethod : PropertyMapBuilderFacts
    {
        [Fact]
        public void ReturnsTrueWhenRegistrationIsNew()
        {
            // Arrange
            var registrationMock = new Mock<IPropertyMapOptionRegistration>();
            registrationMock.SetupGet(registration => registration.AllowedTypes)
                .Returns(new[] { typeof(int), });

            var sut = CreateSystemUnderTest(model => model.Id);

            // Act
            var isAdded = sut.TryAddRegistration(registrationMock.Object);

            // Assert
            Assert.True(isAdded);
        }

        [Fact]
        public void ReturnsFalseWhenRegistrationIsAlreadyAdded()
        {
            // Arrange
            var registrationMock = new Mock<IPropertyMapOptionRegistration>();
            registrationMock.SetupGet(registration => registration.AllowedTypes)
                .Returns(new[] { typeof(int), });

            var sut = CreateSystemUnderTest(model => model.Id);
            sut.TryAddRegistration(registrationMock.Object);

            // Act
            var isAdded = sut.TryAddRegistration(registrationMock.Object);

            // Assert
            Assert.False(isAdded);
        }

        [Fact]
        public void ReturnsFalseWhenRegistrationIsNotAllowed()
        {
            // Arrange
            var registrationMock = new Mock<IPropertyMapOptionRegistration>();
            registrationMock.SetupGet(registration => registration.AllowedTypes)
                .Returns(new[] { typeof(string), });

            var sut = CreateSystemUnderTest(model => model.Id);

            // Act
            var isAdded = sut.TryAddRegistration(registrationMock.Object);

            // Assert
            Assert.False(isAdded);
        }
    }

    public class TheTryGetRegistrationMethod : PropertyMapBuilderFacts
    {
        [Fact]
        public void ReturnsFalseWhenRegistrationIsMissing()
        {
            // Arrange
            var sut = CreateSystemUnderTest(model => model.Id);

            // Act
            var hasRegistration = sut.TryGetRegistration<FakePropertyMapOptionRegistration>(out var registration);

            // Assert
            Assert.False(hasRegistration);
            Assert.Null(registration);
        }
    }

    public class TheHasKeyMethod : PropertyMapBuilderFacts
    {
        [Fact]
        public void ModifiesKeyBuilderUsingProvidedAction()
        {
            // Arrange
            var number = 3U;

            var propertyMapKeyBuilderMock = _mocker.GetMock<IInternalPropertyMapKeyBuilder>();
            _mocker.GetMock<IMapBuilderFactory>()
                .Setup(mapBuilderFactory => mapBuilderFactory.CreateForPropertyKey(It.IsAny<PropertyInfo>()))
                .Returns(propertyMapKeyBuilderMock.Object);

            var sut = CreateSystemUnderTest(model => model.Id);

            // Act
            sut.HasKey(key => key.WithNumber(number));

            // Assert
            propertyMapKeyBuilderMock.Verify(builder => builder.WithNumber(number));
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenKeyActionIsNull()
        {
            // Arrange
            var sut = CreateSystemUnderTest(model => model.Id);

            // Act
            var exception = Record.Exception(() => sut.HasKey(default!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheHasDefaultMethod : PropertyMapBuilderFacts
    {
        [Fact]
        public void AddsOptionRegistration()
        {
            // Arrange
            var value = 21;
            var expectedResolution = ResourcePropertyDefaultReadingResolution.Missing;
            var sut = CreateSystemUnderTest(model => model.Id);

            // Act
            sut.HasDefault(value, expectedResolution);

            // Assert
            Assert.True(sut.TryGetRegistration<DefaultValuePropertyMapOptionRegistration>(out var registration));
            Assert.Equal(value, registration!.Value);
            var actualResolution = Assert.Single(registration.Resolutions);
            Assert.Equal(expectedResolution, actualResolution);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenResolutionCollectionIsNull()
        {
            // Arrange
            var value = 21;
            var resolutions = default(ResourcePropertyDefaultReadingResolution[]);
            var sut = CreateSystemUnderTest(model => model.Id);

            // Act
            var exception = Record.Exception(() => sut.HasDefault(value, resolutions!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenAnyResolutionIsNull()
        {
            // Arrange
            var value = 21;
            var resolutions = new[] { ResourcePropertyDefaultReadingResolution.Missing, default, };
            var sut = CreateSystemUnderTest(model => model.Id);

            // Act
            var exception = Record.Exception(() => sut.HasDefault(value, resolutions!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheIsOptionalMethod : PropertyMapBuilderFacts
    {
        [Fact]
        public void AddsOptionRegistration()
        {
            // Arrange
            var kind = PropertyElementKind.Header;
            var sut = CreateSystemUnderTest(model => model.Id);

            // Act
            sut.IsOptional(kind);

            // Assert
            Assert.True(sut.TryGetRegistration<OptionalPropertyMapOption>(out var registration));
            Assert.Equal(kind, registration!.Kind);
        }

        [Fact]
        public void UsesAllElementKindsWhenProvidedKindIsNull()
        {
            // Arrange
            var sut = CreateSystemUnderTest(model => model.Id);

            // Act
            sut.IsOptional();

            // Assert
            Assert.True(sut.TryGetRegistration<OptionalPropertyMapOption>(out var registration));
            Assert.Equal(PropertyElementKind.All, registration!.Kind);
        }
    }

    public class TheUsesHeaderStyleMethodWithCustomStyleParameters : PropertyMapBuilderFacts
    {
        [Fact]
        public void AddsOptionRegistration()
        {
            // Arrange
            var style = Style.Default;
            var name = "foo";
            var sut = CreateSystemUnderTest(model => model.Id);

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
            var sut = CreateSystemUnderTest(model => model.Id);

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
            var sut = CreateSystemUnderTest(model => model.Id);

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
            var sut = CreateSystemUnderTest(model => model.Id);

            // Act
            var exception = Record.Exception(() => sut.UsesHeaderStyle(style!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheUsesHeaderStyleMethodWithExcelStyleParameters : PropertyMapBuilderFacts
    {
        [Fact]
        public void AddsOptionRegistration()
        {
            // Arrange
            var style = BuiltInExcelStyle.Normal;
            var sut = CreateSystemUnderTest(model => model.Id);

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
            var sut = CreateSystemUnderTest(model => model.Id);

            // Act
            var exception = Record.Exception(() => sut.UsesHeaderStyle(style!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheUsesHeaderStyleMethodWithPackageStyleParameters : PropertyMapBuilderFacts
    {
        [Fact]
        public void AddsOptionRegistration()
        {
            // Arrange
            var style = BuiltInPackageStyle.Bold;
            var sut = CreateSystemUnderTest(model => model.Id);

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
            var sut = CreateSystemUnderTest(model => model.Id);

            // Act
            var exception = Record.Exception(() => sut.UsesHeaderStyle(style!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheUsesBodyStyleMethodWithCustomStyleParameters : PropertyMapBuilderFacts
    {
        [Fact]
        public void AddsOptionRegistration()
        {
            // Arrange
            var style = Style.Default;
            var name = "foo";
            var sut = CreateSystemUnderTest(model => model.Id);

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
            var sut = CreateSystemUnderTest(model => model.Id);

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
            var sut = CreateSystemUnderTest(model => model.Id);

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
            var sut = CreateSystemUnderTest(model => model.Id);

            // Act
            var exception = Record.Exception(() => sut.UsesBodyStyle(style!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheUsesBodyStyleMethodWithExcelStyleParameters : PropertyMapBuilderFacts
    {
        [Fact]
        public void AddsOptionRegistration()
        {
            // Arrange
            var style = BuiltInExcelStyle.Normal;
            var sut = CreateSystemUnderTest(model => model.Id);

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
            var sut = CreateSystemUnderTest(model => model.Id);

            // Act
            var exception = Record.Exception(() => sut.UsesBodyStyle(style!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheUsesBodyStyleMethodWithPackageStyleParameters : PropertyMapBuilderFacts
    {
        [Fact]
        public void AddsOptionRegistration()
        {
            // Arrange
            var style = BuiltInPackageStyle.Bold;
            var sut = CreateSystemUnderTest(model => model.Id);

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
            var sut = CreateSystemUnderTest(model => model.Id);

            // Act
            var exception = Record.Exception(() => sut.UsesBodyStyle(style!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheUsesDateKindMethod : PropertyMapBuilderFacts
    {
        [Fact]
        public void AddsOptionRegistration()
        {
            // Arrange
            var dateKind = CellDateKind.Text;
            var sut = CreateSystemUnderTest(model => model.DateTime);

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
            var sut = CreateSystemUnderTest(model => model.DateTime);

            // Act
            var exception = Record.Exception(() => sut.UsesDateKind(dateKind!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void ThrowsInvalidOperationExceptionWhenPropertyTypeIsNotDate()
        {
            // Arrange
            var dateKind = CellDateKind.Number;
            var sut = CreateSystemUnderTest(model => model.Id);

            // Act
            var exception = Record.Exception(() => sut.UsesDateKind(dateKind!));

            // Assert
            Assert.NotNull(exception);
            var invalidOperationException = Assert.IsType<InvalidOperationException>(exception);
            Assert.Equal(
                Messages.MapOptionRegistrationNotAllowedForType(typeof(DateKindMapOption), typeof(int).Name),
                invalidOperationException.Message);
        }
    }

    public class TheUsesStringKindMethod : PropertyMapBuilderFacts
    {
        [Fact]
        public void AddsOptionRegistration()
        {
            // Arrange
            var stringKind = CellStringKind.InlineString;
            var sut = CreateSystemUnderTest(model => model.Name);

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
            var sut = CreateSystemUnderTest(model => model.Name);

            // Act
            var exception = Record.Exception(() => sut.UsesStringKind(stringKind!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void ThrowsInvalidOperationExceptionWhenPropertyTypeIsNotText()
        {
            // Arrange
            var stringKind = CellStringKind.SharedString;
            var sut = CreateSystemUnderTest(model => model.Id);

            // Act
            var exception = Record.Exception(() => sut.UsesStringKind(stringKind!));

            // Assert
            Assert.NotNull(exception);
            var invalidOperationException = Assert.IsType<InvalidOperationException>(exception);
            Assert.Equal(
                Messages.MapOptionRegistrationNotAllowedForType(typeof(StringKindMapOption), typeof(int).Name),
                invalidOperationException.Message);
        }
    }
}
