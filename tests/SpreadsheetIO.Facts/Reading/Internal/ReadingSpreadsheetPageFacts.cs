using System;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes;
using LanceC.SpreadsheetIO.Mapping.Internal;
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

        public class TheReadMethodWithResourceGenericParameter : ReadingSpreadsheetPageFacts
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

                var sut = CreateSystemUnderTest();

                // Act
                var result = sut.Read<FakeModel>();

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
                worksheetReaderMock.SetupSequence(worksheetReader => worksheetReader.ReadNextRow())
                    .Returns(true)
                    .Returns(true)
                    .Returns(false);

                _mocker.GetMock<IElementReaderFactory>()
                    .Setup(elementReaderFactory => elementReaderFactory.CreateWorksheetReader(It.IsAny<IWorksheetPartWrapper>()))
                    .Returns(worksheetReaderMock.Object);

                var spreadsheetPageMapReaderMock = _mocker.GetMock<ISpreadsheetPageMapReader>();

                var headersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                var headerRowResult = new HeaderRowReadingResult<FakeModel>(headersMock.Object, default);
                spreadsheetPageMapReaderMock
                    .Setup(spreadsheetPageMapReader => spreadsheetPageMapReader.ReadHeaderRow(worksheetReaderMock.Object, map))
                    .Returns(headerRowResult);

                var firstBodyRowResult = new BodyRowReadingResult<FakeModel>(
                    default,
                    new ResourceReadingFailure(
                        2U,
                        Array.Empty<MissingResourcePropertyReadingFailure>(),
                        Array.Empty<InvalidResourcePropertyReadingFailure>()));
                var secondBodyRowResult = new BodyRowReadingResult<FakeModel>(new FakeModel(), default);
                spreadsheetPageMapReaderMock
                    .SetupSequence(spreadsheetPageMapReader => spreadsheetPageMapReader.ReadBodyRow(
                        worksheetReaderMock.Object,
                        map,
                        headersMock.Object))
                    .Returns(firstBodyRowResult)
                    .Returns(secondBodyRowResult);

                var sut = CreateSystemUnderTest();

                // Act
                var result = sut.Read<FakeModel>();

                // Assert
                Assert.Empty(result.Resources);
                Assert.Null(result.HeaderFailure);
                var resourceFailure = Assert.Single(result.ResourceFailures);
                Assert.Equal(firstBodyRowResult.Failure, resourceFailure);
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
                worksheetReaderMock.SetupSequence(worksheetReader => worksheetReader.ReadNextRow())
                    .Returns(true)
                    .Returns(true)
                    .Returns(false);

                _mocker.GetMock<IElementReaderFactory>()
                    .Setup(elementReaderFactory => elementReaderFactory.CreateWorksheetReader(It.IsAny<IWorksheetPartWrapper>()))
                    .Returns(worksheetReaderMock.Object);

                var spreadsheetPageMapReaderMock = _mocker.GetMock<ISpreadsheetPageMapReader>();

                var headersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                var headerRowResult = new HeaderRowReadingResult<FakeModel>(headersMock.Object, default);
                spreadsheetPageMapReaderMock
                    .Setup(spreadsheetPageMapReader => spreadsheetPageMapReader.ReadHeaderRow(worksheetReaderMock.Object, map))
                    .Returns(headerRowResult);

                var firstBodyRowResult = new BodyRowReadingResult<FakeModel>(
                    default,
                    new ResourceReadingFailure(
                        2U,
                        Array.Empty<MissingResourcePropertyReadingFailure>(),
                        Array.Empty<InvalidResourcePropertyReadingFailure>()));
                var secondBodyRowResult = new BodyRowReadingResult<FakeModel>(new FakeModel(), default);
                spreadsheetPageMapReaderMock
                    .SetupSequence(spreadsheetPageMapReader => spreadsheetPageMapReader.ReadBodyRow(
                        worksheetReaderMock.Object,
                        map,
                        headersMock.Object))
                    .Returns(firstBodyRowResult)
                    .Returns(secondBodyRowResult);

                var sut = CreateSystemUnderTest();

                // Act
                var result = sut.Read<FakeModel>();

                // Assert
                var resource = Assert.Single(result.Resources);
                Assert.Equal(secondBodyRowResult.Resource, resource);
                Assert.Null(result.HeaderFailure);
                var resourceFailure = Assert.Single(result.ResourceFailures);
                Assert.Equal(firstBodyRowResult.Failure, resourceFailure);
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
                worksheetReaderMock.SetupSequence(worksheetReader => worksheetReader.ReadNextRow())
                    .Returns(true)
                    .Returns(false);

                _mocker.GetMock<IElementReaderFactory>()
                    .Setup(elementReaderFactory => elementReaderFactory.CreateWorksheetReader(It.IsAny<IWorksheetPartWrapper>()))
                    .Returns(worksheetReaderMock.Object);

                var spreadsheetPageMapReaderMock = _mocker.GetMock<ISpreadsheetPageMapReader>();

                var headersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                var headerRowResult = new HeaderRowReadingResult<FakeModel>(headersMock.Object, default);
                spreadsheetPageMapReaderMock
                    .Setup(spreadsheetPageMapReader => spreadsheetPageMapReader.ReadHeaderRow(worksheetReaderMock.Object, map))
                    .Returns(headerRowResult);

                var bodyRowResult = new BodyRowReadingResult<FakeModel>(new FakeModel(), default);
                spreadsheetPageMapReaderMock
                    .Setup(spreadsheetPageMapReader => spreadsheetPageMapReader.ReadBodyRow(
                        worksheetReaderMock.Object,
                        map,
                        headersMock.Object))
                    .Returns(bodyRowResult);

                var sut = CreateSystemUnderTest();

                // Act
                var result = sut.Read<FakeModel>();

                // Assert
                var resource = Assert.Single(result.Resources);
                Assert.Equal(bodyRowResult.Resource, resource);
                Assert.Null(result.HeaderFailure);
                Assert.Empty(result.ResourceFailures);
            }
        }

        public class TheReadMethodWithResourceAndResourceMapGenericParameters : ReadingSpreadsheetPageFacts
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

                var sut = CreateSystemUnderTest();

                // Act
                var result = sut.Read<FakeModel, FakeModelMap>();

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
                worksheetReaderMock.SetupSequence(worksheetReader => worksheetReader.ReadNextRow())
                    .Returns(true)
                    .Returns(true)
                    .Returns(false);

                _mocker.GetMock<IElementReaderFactory>()
                    .Setup(elementReaderFactory => elementReaderFactory.CreateWorksheetReader(It.IsAny<IWorksheetPartWrapper>()))
                    .Returns(worksheetReaderMock.Object);

                var spreadsheetPageMapReaderMock = _mocker.GetMock<ISpreadsheetPageMapReader>();

                var headersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                var headerRowResult = new HeaderRowReadingResult<FakeModel>(headersMock.Object, default);
                spreadsheetPageMapReaderMock
                    .Setup(spreadsheetPageMapReader => spreadsheetPageMapReader.ReadHeaderRow(worksheetReaderMock.Object, map))
                    .Returns(headerRowResult);

                var firstBodyRowResult = new BodyRowReadingResult<FakeModel>(
                    default,
                    new ResourceReadingFailure(
                        2U,
                        Array.Empty<MissingResourcePropertyReadingFailure>(),
                        Array.Empty<InvalidResourcePropertyReadingFailure>()));
                var secondBodyRowResult = new BodyRowReadingResult<FakeModel>(new FakeModel(), default);
                spreadsheetPageMapReaderMock
                    .SetupSequence(spreadsheetPageMapReader => spreadsheetPageMapReader.ReadBodyRow(
                        worksheetReaderMock.Object,
                        map,
                        headersMock.Object))
                    .Returns(firstBodyRowResult)
                    .Returns(secondBodyRowResult);

                var sut = CreateSystemUnderTest();

                // Act
                var result = sut.Read<FakeModel, FakeModelMap>();

                // Assert
                Assert.Empty(result.Resources);
                Assert.Null(result.HeaderFailure);
                var resourceFailure = Assert.Single(result.ResourceFailures);
                Assert.Equal(firstBodyRowResult.Failure, resourceFailure);
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
                worksheetReaderMock.SetupSequence(worksheetReader => worksheetReader.ReadNextRow())
                    .Returns(true)
                    .Returns(true)
                    .Returns(false);

                _mocker.GetMock<IElementReaderFactory>()
                    .Setup(elementReaderFactory => elementReaderFactory.CreateWorksheetReader(It.IsAny<IWorksheetPartWrapper>()))
                    .Returns(worksheetReaderMock.Object);

                var spreadsheetPageMapReaderMock = _mocker.GetMock<ISpreadsheetPageMapReader>();

                var headersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                var headerRowResult = new HeaderRowReadingResult<FakeModel>(headersMock.Object, default);
                spreadsheetPageMapReaderMock
                    .Setup(spreadsheetPageMapReader => spreadsheetPageMapReader.ReadHeaderRow(worksheetReaderMock.Object, map))
                    .Returns(headerRowResult);

                var firstBodyRowResult = new BodyRowReadingResult<FakeModel>(
                    default,
                    new ResourceReadingFailure(
                        2U,
                        Array.Empty<MissingResourcePropertyReadingFailure>(),
                        Array.Empty<InvalidResourcePropertyReadingFailure>()));
                var secondBodyRowResult = new BodyRowReadingResult<FakeModel>(new FakeModel(), default);
                spreadsheetPageMapReaderMock
                    .SetupSequence(spreadsheetPageMapReader => spreadsheetPageMapReader.ReadBodyRow(
                        worksheetReaderMock.Object,
                        map,
                        headersMock.Object))
                    .Returns(firstBodyRowResult)
                    .Returns(secondBodyRowResult);

                var sut = CreateSystemUnderTest();

                // Act
                var result = sut.Read<FakeModel, FakeModelMap>();

                // Assert
                var resource = Assert.Single(result.Resources);
                Assert.Equal(secondBodyRowResult.Resource, resource);
                Assert.Null(result.HeaderFailure);
                var resourceFailure = Assert.Single(result.ResourceFailures);
                Assert.Equal(firstBodyRowResult.Failure, resourceFailure);
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
                worksheetReaderMock.SetupSequence(worksheetReader => worksheetReader.ReadNextRow())
                    .Returns(true)
                    .Returns(false);

                _mocker.GetMock<IElementReaderFactory>()
                    .Setup(elementReaderFactory => elementReaderFactory.CreateWorksheetReader(It.IsAny<IWorksheetPartWrapper>()))
                    .Returns(worksheetReaderMock.Object);

                var spreadsheetPageMapReaderMock = _mocker.GetMock<ISpreadsheetPageMapReader>();

                var headersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                var headerRowResult = new HeaderRowReadingResult<FakeModel>(headersMock.Object, default);
                spreadsheetPageMapReaderMock
                    .Setup(spreadsheetPageMapReader => spreadsheetPageMapReader.ReadHeaderRow(worksheetReaderMock.Object, map))
                    .Returns(headerRowResult);

                var bodyRowResult = new BodyRowReadingResult<FakeModel>(new FakeModel(), default);
                spreadsheetPageMapReaderMock
                    .Setup(spreadsheetPageMapReader => spreadsheetPageMapReader.ReadBodyRow(
                        worksheetReaderMock.Object,
                        map,
                        headersMock.Object))
                    .Returns(bodyRowResult);

                var sut = CreateSystemUnderTest();

                // Act
                var result = sut.Read<FakeModel, FakeModelMap>();

                // Assert
                var resource = Assert.Single(result.Resources);
                Assert.Equal(bodyRowResult.Resource, resource);
                Assert.Null(result.HeaderFailure);
                Assert.Empty(result.ResourceFailures);
            }
        }
    }
}
