using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.ResourceMaps;
using LanceC.SpreadsheetIO.Facts.Testing.Moq;
using LanceC.SpreadsheetIO.Mapping.Internal;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Reading.Failures;
using LanceC.SpreadsheetIO.Reading.Internal;
using LanceC.SpreadsheetIO.Reading.Internal.Properties;
using LanceC.SpreadsheetIO.Reading.Internal.Readers;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal
{
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

        public class TheReadAllMethodWithResourceGenericParameter : ReadingSpreadsheetPageFacts
        {
            [Fact]
            public void ReturnsNoResourcesWhenHeaderRowReadingFailureIsSet()
            {
                // Arrange
                var map = new FakeModelMap();
                _mocker.GetMock<IResourceMapManager>()
                    .Setup(resourceMapManager => resourceMapManager.Single<FakeModel>())
                    .Returns(map);

                var worksheetReaderMock = _mocker.GetMock<IWorksheetElementReader>();
                _mocker.GetMock<IElementReaderFactory>()
                    .Setup(elementReaderFactory => elementReaderFactory.CreateWorksheetReader(It.IsAny<IWorksheetPartWrapper>()))
                    .Returns(worksheetReaderMock.Object);

                var headersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                var headerRowResult = new HeaderRowReadingResult<FakeModel>(
                    headersMock.Object,
                    new HeaderReadingFailure(
                        true,
                        Array.Empty<MissingHeaderReadingFailure>(),
                        Array.Empty<InvalidHeaderReadingFailure>()));
                _mocker.GetMock<ISpreadsheetPageMapReader>()
                    .Setup(spreadsheetPageMapReader => spreadsheetPageMapReader.ReadHeaderRow(worksheetReaderMock.Object, map))
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
                var map = new FakeModelMap(optionsBuilderAction => optionsBuilderAction.ExitOnResourceReadingFailure());
                _mocker.GetMock<IResourceMapManager>()
                    .Setup(resourceMapManager => resourceMapManager.Single<FakeModel>())
                    .Returns(map);

                var worksheetReaderMock = _mocker.GetMock<IWorksheetElementReader>();
                _mocker.GetMock<IElementReaderFactory>()
                    .Setup(elementReaderFactory => elementReaderFactory.CreateWorksheetReader(It.IsAny<IWorksheetPartWrapper>()))
                    .Returns(worksheetReaderMock.Object);

                var spreadsheetPageMapReaderMock = _mocker.GetMock<ISpreadsheetPageMapReader>();

                var headersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                var headerRowResult = new HeaderRowReadingResult<FakeModel>(headersMock.Object, default);
                spreadsheetPageMapReaderMock
                    .Setup(spreadsheetPageMapReader => spreadsheetPageMapReader.ReadHeaderRow(worksheetReaderMock.Object, map))
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
                var map = new FakeModelMap();
                _mocker.GetMock<IResourceMapManager>()
                    .Setup(resourceMapManager => resourceMapManager.Single<FakeModel>())
                    .Returns(map);

                var worksheetReaderMock = _mocker.GetMock<IWorksheetElementReader>();
                _mocker.GetMock<IElementReaderFactory>()
                    .Setup(elementReaderFactory => elementReaderFactory.CreateWorksheetReader(It.IsAny<IWorksheetPartWrapper>()))
                    .Returns(worksheetReaderMock.Object);

                var spreadsheetPageMapReaderMock = _mocker.GetMock<ISpreadsheetPageMapReader>();

                var headersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                var headerRowResult = new HeaderRowReadingResult<FakeModel>(headersMock.Object, default);
                spreadsheetPageMapReaderMock
                    .Setup(spreadsheetPageMapReader => spreadsheetPageMapReader.ReadHeaderRow(worksheetReaderMock.Object, map))
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
                var map = new FakeModelMap();
                _mocker.GetMock<IResourceMapManager>()
                    .Setup(resourceMapManager => resourceMapManager.Single<FakeModel>())
                    .Returns(map);

                var worksheetReaderMock = _mocker.GetMock<IWorksheetElementReader>();
                _mocker.GetMock<IElementReaderFactory>()
                    .Setup(elementReaderFactory => elementReaderFactory.CreateWorksheetReader(It.IsAny<IWorksheetPartWrapper>()))
                    .Returns(worksheetReaderMock.Object);

                var spreadsheetPageMapReaderMock = _mocker.GetMock<ISpreadsheetPageMapReader>();

                var headersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                var headerRowResult = new HeaderRowReadingResult<FakeModel>(headersMock.Object, default);
                spreadsheetPageMapReaderMock
                    .Setup(spreadsheetPageMapReader => spreadsheetPageMapReader.ReadHeaderRow(worksheetReaderMock.Object, map))
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

        public class TheReadAllMethodWithResourceAndResourceMapGenericParameters : ReadingSpreadsheetPageFacts
        {
            [Fact]
            public void ReturnsNoResourcesWhenHeaderRowReadingFailureIsSet()
            {
                // Arrange
                var map = new FakeModelMap();
                _mocker.GetMock<IResourceMapManager>()
                    .Setup(resourceMapManager => resourceMapManager.Single<FakeModel, FakeModelMap>())
                    .Returns(map);

                var worksheetReaderMock = _mocker.GetMock<IWorksheetElementReader>();
                _mocker.GetMock<IElementReaderFactory>()
                    .Setup(elementReaderFactory => elementReaderFactory.CreateWorksheetReader(It.IsAny<IWorksheetPartWrapper>()))
                    .Returns(worksheetReaderMock.Object);

                var headersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                var headerRowResult = new HeaderRowReadingResult<FakeModel>(
                    headersMock.Object,
                    new HeaderReadingFailure(
                        true,
                        Array.Empty<MissingHeaderReadingFailure>(),
                        Array.Empty<InvalidHeaderReadingFailure>()));
                _mocker.GetMock<ISpreadsheetPageMapReader>()
                    .Setup(spreadsheetPageMapReader => spreadsheetPageMapReader.ReadHeaderRow(worksheetReaderMock.Object, map))
                    .Returns(headerRowResult);

                var operationMock = _mocker.GetMock<IReadingSpreadsheetPageOperation<FakeModel>>();
                operationMock.SetupGet(operation => operation.HeaderFailure)
                    .Returns(headerRowResult.Failure);

                _mocker.GetMock<IReadingSpreadsheetPageOperationFactory>()
                    .Setup(operationFactory => operationFactory.Create(worksheetReaderMock.Object, headerRowResult, map))
                    .Returns(operationMock.Object);

                var sut = CreateSystemUnderTest();

                // Act
                var result = sut.ReadAll<FakeModel, FakeModelMap>();

                // Assert
                Assert.Empty(result.Resources);
                Assert.Equal(headerRowResult.Failure, result.HeaderFailure);
                Assert.Empty(result.ResourceFailures);
            }

            [Fact]
            public void ReturnsNoResourcesWhenBodyRowReadingFailureIsSetAndExitOnFailureOptionsExtensionIsPresent()
            {
                // Arrange
                var map = new FakeModelMap(optionsBuilderAction => optionsBuilderAction.ExitOnResourceReadingFailure());
                _mocker.GetMock<IResourceMapManager>()
                    .Setup(resourceMapManager => resourceMapManager.Single<FakeModel, FakeModelMap>())
                    .Returns(map);

                var worksheetReaderMock = _mocker.GetMock<IWorksheetElementReader>();
                _mocker.GetMock<IElementReaderFactory>()
                    .Setup(elementReaderFactory => elementReaderFactory.CreateWorksheetReader(It.IsAny<IWorksheetPartWrapper>()))
                    .Returns(worksheetReaderMock.Object);

                var spreadsheetPageMapReaderMock = _mocker.GetMock<ISpreadsheetPageMapReader>();

                var headersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                var headerRowResult = new HeaderRowReadingResult<FakeModel>(headersMock.Object, default);
                spreadsheetPageMapReaderMock
                    .Setup(spreadsheetPageMapReader => spreadsheetPageMapReader.ReadHeaderRow(worksheetReaderMock.Object, map))
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
                var result = sut.ReadAll<FakeModel, FakeModelMap>();

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
                var map = new FakeModelMap();
                _mocker.GetMock<IResourceMapManager>()
                    .Setup(resourceMapManager => resourceMapManager.Single<FakeModel, FakeModelMap>())
                    .Returns(map);

                var worksheetReaderMock = _mocker.GetMock<IWorksheetElementReader>();
                _mocker.GetMock<IElementReaderFactory>()
                    .Setup(elementReaderFactory => elementReaderFactory.CreateWorksheetReader(It.IsAny<IWorksheetPartWrapper>()))
                    .Returns(worksheetReaderMock.Object);

                var spreadsheetPageMapReaderMock = _mocker.GetMock<ISpreadsheetPageMapReader>();

                var headersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                var headerRowResult = new HeaderRowReadingResult<FakeModel>(headersMock.Object, default);
                spreadsheetPageMapReaderMock
                    .Setup(spreadsheetPageMapReader => spreadsheetPageMapReader.ReadHeaderRow(worksheetReaderMock.Object, map))
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
                var result = sut.ReadAll<FakeModel, FakeModelMap>();

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
                var map = new FakeModelMap();
                _mocker.GetMock<IResourceMapManager>()
                    .Setup(resourceMapManager => resourceMapManager.Single<FakeModel, FakeModelMap>())
                    .Returns(map);

                var worksheetReaderMock = _mocker.GetMock<IWorksheetElementReader>();
                _mocker.GetMock<IElementReaderFactory>()
                    .Setup(elementReaderFactory => elementReaderFactory.CreateWorksheetReader(It.IsAny<IWorksheetPartWrapper>()))
                    .Returns(worksheetReaderMock.Object);

                var spreadsheetPageMapReaderMock = _mocker.GetMock<ISpreadsheetPageMapReader>();

                var headersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                var headerRowResult = new HeaderRowReadingResult<FakeModel>(headersMock.Object, default);
                spreadsheetPageMapReaderMock
                    .Setup(spreadsheetPageMapReader => spreadsheetPageMapReader.ReadHeaderRow(worksheetReaderMock.Object, map))
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
                var result = sut.ReadAll<FakeModel, FakeModelMap>();

                // Assert
                var resource = Assert.Single(result.Resources);
                Assert.Equal(resourceResult.NumberedResource, resource);
                Assert.Null(result.HeaderFailure);
                Assert.Empty(result.ResourceFailures);
            }
        }

        public class TheStartReadMethodWithResourceGenericParameter : ReadingSpreadsheetPageFacts
        {
            [Fact]
            public void ReturnsOperationFromFactory()
            {
                // Arrange
                var map = new FakeModelMap();
                _mocker.GetMock<IResourceMapManager>()
                    .Setup(resourceMapManager => resourceMapManager.Single<FakeModel>())
                    .Returns(map);

                var worksheetReaderMock = _mocker.GetMock<IWorksheetElementReader>();
                _mocker.GetMock<IElementReaderFactory>()
                    .Setup(elementReaderFactory => elementReaderFactory.CreateWorksheetReader(It.IsAny<IWorksheetPartWrapper>()))
                    .Returns(worksheetReaderMock.Object);

                var spreadsheetPageMapReaderMock = _mocker.GetMock<ISpreadsheetPageMapReader>();

                var headersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                var headerRowResult = new HeaderRowReadingResult<FakeModel>(headersMock.Object, default);
                spreadsheetPageMapReaderMock
                    .Setup(spreadsheetPageMapReader => spreadsheetPageMapReader.ReadHeaderRow(worksheetReaderMock.Object, map))
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

        public class TheStartReadMethodWithResourceAndResourceMapGenericParameters : ReadingSpreadsheetPageFacts
        {
            [Fact]
            public void ReturnsOperationFromFactory()
            {
                // Arrange
                var map = new FakeModelMap();
                _mocker.GetMock<IResourceMapManager>()
                    .Setup(resourceMapManager => resourceMapManager.Single<FakeModel, FakeModelMap>())
                    .Returns(map);

                var worksheetReaderMock = _mocker.GetMock<IWorksheetElementReader>();
                _mocker.GetMock<IElementReaderFactory>()
                    .Setup(elementReaderFactory => elementReaderFactory.CreateWorksheetReader(It.IsAny<IWorksheetPartWrapper>()))
                    .Returns(worksheetReaderMock.Object);

                var spreadsheetPageMapReaderMock = _mocker.GetMock<ISpreadsheetPageMapReader>();

                var headersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                var headerRowResult = new HeaderRowReadingResult<FakeModel>(headersMock.Object, default);
                spreadsheetPageMapReaderMock
                    .Setup(spreadsheetPageMapReader => spreadsheetPageMapReader.ReadHeaderRow(worksheetReaderMock.Object, map))
                    .Returns(headerRowResult);

                var operationMock = _mocker.GetMock<IReadingSpreadsheetPageOperation<FakeModel>>();

                _mocker.GetMock<IReadingSpreadsheetPageOperationFactory>()
                    .Setup(operationFactory => operationFactory.Create(worksheetReaderMock.Object, headerRowResult, map))
                    .Returns(operationMock.Object);

                var sut = CreateSystemUnderTest();

                // Act
                var operation = sut.StartRead<FakeModel, FakeModelMap>();

                // Assert
                Assert.Equal(operationMock.Object, operation);
            }
        }
    }
}
