using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Facts.Testing.Moq;
using LanceC.SpreadsheetIO.Mapping2;
using LanceC.SpreadsheetIO.Mapping2.Options;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Reading.Failures;
using LanceC.SpreadsheetIO.Reading.Internal;
using LanceC.SpreadsheetIO.Reading.Internal.Properties;
using LanceC.SpreadsheetIO.Reading.Internal.Readers;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal;

public class ReadingSpreadsheetPageFacts
{
    private readonly AutoMocker _mocker = new();

    private ReadingSpreadsheetPage CreateSystemUnderTest()
        => _mocker.CreateInstance<ReadingSpreadsheetPage>();

    private void MockOperationRead(params (bool HasRow, ResourceReadingResult<FakeModel>? ResourceResult)[] values)
    {
        var operationMock = _mocker.GetMock<IReadingSpreadsheetPageOperation<FakeModel>>();

        var callbackReturnValues = values
            .Select(value => new CallbackReturnValue<bool>(
                value.HasRow,
                () => operationMock.SetupGet(operation => operation.CurrentResult)
                    .Returns(value.ResourceResult)));

        operationMock.Setup(operation => operation.ReadNext())
            .ReturnsCallbackSequence(values
                .Select(value => new CallbackReturnValue<bool>(
                    value.HasRow,
                    () => operationMock.SetupGet(operation => operation.CurrentResult)
                        .Returns(value.ResourceResult)))
                .ToArray());
    }

    public class TheReadAllMethod : ReadingSpreadsheetPageFacts
    {
        [Fact]
        public void ReturnsNoResourcesWhenHeaderRowReadingFailureIsSet()
        {
            // Arrange
            var map = ResourceMapCreator.Create<FakeModel>(Array.Empty<PropertyMap>());
            _mocker.GetMock<ICartographer>()
                .Setup(cartographer => cartographer.GetMap<FakeModel>())
                .Returns(map);

            var worksheetReaderMock = _mocker.GetMock<IWorksheetElementReader>();
            _mocker.GetMock<IElementReaderFactory>()
                .Setup(elementReaderFactory => elementReaderFactory.CreateWorksheetReader(It.IsAny<IWorksheetPartWrapper>()))
                .Returns(worksheetReaderMock.Object);

            var headersMock = _mocker.GetMock<IResourcePropertyHeaders>();
            var headerRowResult = new HeaderRowReadingResult<FakeModel>(
                headersMock.Object,
                new HeaderReadingFailure(
                    true,
                    Array.Empty<MissingHeaderReadingFailure>(),
                    Array.Empty<InvalidHeaderReadingFailure>()));
            _mocker.GetMock<IMappedHeaderRowReader>()
                .Setup(mappedHeaderRowReader => mappedHeaderRowReader.Read<FakeModel>(worksheetReaderMock.Object, map))
                .Returns(headerRowResult);

            var operationMock = _mocker.GetMock<IReadingSpreadsheetPageOperation<FakeModel>>();
            operationMock.SetupGet(operation => operation.HeaderFailure)
                .Returns(headerRowResult.Failure);

            _mocker.GetMock<IReadingSpreadsheetPageOperationFactory>()
                .Setup(operationFactory => operationFactory.Create(worksheetReaderMock.Object, headerRowResult, map))
                .Returns(operationMock.Object);

            var sut = CreateSystemUnderTest();

            // Act
            var result = sut.ReadAll<FakeModel>();

            // Assert
            Assert.Empty(result.Resources);
            Assert.Equal(headerRowResult.Failure, result.HeaderFailure);
            Assert.Empty(result.ResourceFailures);
        }

        [Fact]
        public void ReturnsNoResourcesWhenBodyRowReadingFailureIsSetAndExitOnFailureOptionsExtensionIsPresent()
        {
            // Arrange
            var map = ResourceMapCreator.Create<FakeModel>(
                Array.Empty<PropertyMap>(),
                new ExitOnResourceReadingFailureResourceMapOption());
            _mocker.GetMock<ICartographer>()
                .Setup(cartographer => cartographer.GetMap<FakeModel>())
                .Returns(map);

            var worksheetReaderMock = _mocker.GetMock<IWorksheetElementReader>();
            _mocker.GetMock<IElementReaderFactory>()
                .Setup(elementReaderFactory => elementReaderFactory.CreateWorksheetReader(It.IsAny<IWorksheetPartWrapper>()))
                .Returns(worksheetReaderMock.Object);

            var headersMock = _mocker.GetMock<IResourcePropertyHeaders>();
            var headerRowResult = new HeaderRowReadingResult<FakeModel>(headersMock.Object, default);
            _mocker.GetMock<IMappedHeaderRowReader>()
                .Setup(mappedHeaderRowReader => mappedHeaderRowReader.Read<FakeModel>(worksheetReaderMock.Object, map))
                .Returns(headerRowResult);

            var firstResourceResult = new ResourceReadingResult<FakeModel>(
                default,
                new ResourceReadingFailure(
                    2U,
                    Array.Empty<MissingResourcePropertyReadingFailure>(),
                    Array.Empty<InvalidResourcePropertyReadingFailure>()));
            var secondResourceResult = new ResourceReadingResult<FakeModel>(new NumberedResource<FakeModel>(3U, new()), default);

            var operationMock = _mocker.GetMock<IReadingSpreadsheetPageOperation<FakeModel>>();
            operationMock.SetupGet(operation => operation.HeaderFailure)
                .Returns(headerRowResult.Failure);

            MockOperationRead((true, firstResourceResult), (true, secondResourceResult), (false, default));

            _mocker.GetMock<IReadingSpreadsheetPageOperationFactory>()
                .Setup(operationFactory => operationFactory.Create(worksheetReaderMock.Object, headerRowResult, map))
                .Returns(operationMock.Object);

            var sut = CreateSystemUnderTest();

            // Act
            var result = sut.ReadAll<FakeModel>();

            // Assert
            Assert.Empty(result.Resources);
            Assert.Null(result.HeaderFailure);
            var resourceFailure = Assert.Single(result.ResourceFailures);
            Assert.Equal(firstResourceResult.Failure, resourceFailure);
        }

        [Fact]
        public void ReturnsResourceAndBodyRowReadingFailureWhenReadWasPartialSuccess()
        {
            // Arrange
            var map = ResourceMapCreator.Create<FakeModel>(Array.Empty<PropertyMap>());
            _mocker.GetMock<ICartographer>()
                .Setup(cartographer => cartographer.GetMap<FakeModel>())
                .Returns(map);

            var worksheetReaderMock = _mocker.GetMock<IWorksheetElementReader>();
            _mocker.GetMock<IElementReaderFactory>()
                .Setup(elementReaderFactory => elementReaderFactory.CreateWorksheetReader(It.IsAny<IWorksheetPartWrapper>()))
                .Returns(worksheetReaderMock.Object);

            var headersMock = _mocker.GetMock<IResourcePropertyHeaders>();
            var headerRowResult = new HeaderRowReadingResult<FakeModel>(headersMock.Object, default);
            _mocker.GetMock<IMappedHeaderRowReader>()
                .Setup(mappedHeaderRowReader => mappedHeaderRowReader.Read<FakeModel>(worksheetReaderMock.Object, map))
                .Returns(headerRowResult);

            var firstResourceResult = new ResourceReadingResult<FakeModel>(
                default,
                new ResourceReadingFailure(
                    2U,
                    Array.Empty<MissingResourcePropertyReadingFailure>(),
                    Array.Empty<InvalidResourcePropertyReadingFailure>()));
            var secondResourceResult = new ResourceReadingResult<FakeModel>(new NumberedResource<FakeModel>(2U, new()), default);

            var operationMock = _mocker.GetMock<IReadingSpreadsheetPageOperation<FakeModel>>();
            operationMock.SetupGet(operation => operation.HeaderFailure)
                .Returns(headerRowResult.Failure);

            MockOperationRead((true, firstResourceResult), (true, secondResourceResult), (false, default));

            _mocker.GetMock<IReadingSpreadsheetPageOperationFactory>()
                .Setup(operationFactory => operationFactory.Create(worksheetReaderMock.Object, headerRowResult, map))
                .Returns(operationMock.Object);

            var sut = CreateSystemUnderTest();

            // Act
            var result = sut.ReadAll<FakeModel>();

            // Assert
            var resource = Assert.Single(result.Resources);
            Assert.Equal(secondResourceResult.NumberedResource, resource);
            Assert.Null(result.HeaderFailure);
            var resourceFailure = Assert.Single(result.ResourceFailures);
            Assert.Equal(firstResourceResult.Failure, resourceFailure);
        }

        [Fact]
        public void ReturnsResourcesAndEmptyBodyRowReadingFailureCollectionWhenReadWasSuccess()
        {
            // Arrange
            var map = ResourceMapCreator.Create<FakeModel>(Array.Empty<PropertyMap>());
            _mocker.GetMock<ICartographer>()
                .Setup(cartographer => cartographer.GetMap<FakeModel>())
                .Returns(map);

            var worksheetReaderMock = _mocker.GetMock<IWorksheetElementReader>();
            _mocker.GetMock<IElementReaderFactory>()
                .Setup(elementReaderFactory => elementReaderFactory.CreateWorksheetReader(It.IsAny<IWorksheetPartWrapper>()))
                .Returns(worksheetReaderMock.Object);

            var headersMock = _mocker.GetMock<IResourcePropertyHeaders>();
            var headerRowResult = new HeaderRowReadingResult<FakeModel>(headersMock.Object, default);
            _mocker.GetMock<IMappedHeaderRowReader>()
                .Setup(mappedHeaderRowReader => mappedHeaderRowReader.Read<FakeModel>(worksheetReaderMock.Object, map))
                .Returns(headerRowResult);

            var resourceResult = new ResourceReadingResult<FakeModel>(new NumberedResource<FakeModel>(2U, new()), default);

            var operationMock = _mocker.GetMock<IReadingSpreadsheetPageOperation<FakeModel>>();
            operationMock.SetupGet(operation => operation.HeaderFailure)
                .Returns(headerRowResult.Failure);

            MockOperationRead((true, resourceResult), (false, default));

            _mocker.GetMock<IReadingSpreadsheetPageOperationFactory>()
                .Setup(operationFactory => operationFactory.Create(worksheetReaderMock.Object, headerRowResult, map))
                .Returns(operationMock.Object);

            var sut = CreateSystemUnderTest();

            // Act
            var result = sut.ReadAll<FakeModel>();

            // Assert
            var resource = Assert.Single(result.Resources);
            Assert.Equal(resourceResult.NumberedResource, resource);
            Assert.Null(result.HeaderFailure);
            Assert.Empty(result.ResourceFailures);
        }
    }

    public class TheStartReadMethod : ReadingSpreadsheetPageFacts
    {
        [Fact]
        public void ReturnsOperationFromFactory()
        {
            // Arrange
            var map = ResourceMapCreator.Create<FakeModel>(Array.Empty<PropertyMap>());
            _mocker.GetMock<ICartographer>()
                .Setup(cartographer => cartographer.GetMap<FakeModel>())
                .Returns(map);

            var worksheetReaderMock = _mocker.GetMock<IWorksheetElementReader>();
            _mocker.GetMock<IElementReaderFactory>()
                .Setup(elementReaderFactory => elementReaderFactory.CreateWorksheetReader(It.IsAny<IWorksheetPartWrapper>()))
                .Returns(worksheetReaderMock.Object);

            var headersMock = _mocker.GetMock<IResourcePropertyHeaders>();
            var headerRowResult = new HeaderRowReadingResult<FakeModel>(headersMock.Object, default);
            _mocker.GetMock<IMappedHeaderRowReader>()
                .Setup(mappedHeaderRowReader => mappedHeaderRowReader.Read<FakeModel>(worksheetReaderMock.Object, map))
                .Returns(headerRowResult);

            var operationMock = _mocker.GetMock<IReadingSpreadsheetPageOperation<FakeModel>>();

            _mocker.GetMock<IReadingSpreadsheetPageOperationFactory>()
                .Setup(operationFactory => operationFactory.Create(worksheetReaderMock.Object, headerRowResult, map))
                .Returns(operationMock.Object);

            var sut = CreateSystemUnderTest();

            // Act
            var operation = sut.StartRead<FakeModel>();

            // Assert
            Assert.Equal(operationMock.Object, operation);
        }
    }
}
