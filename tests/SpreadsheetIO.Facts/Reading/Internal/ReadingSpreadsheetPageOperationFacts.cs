using System;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Reading.Failures;
using LanceC.SpreadsheetIO.Reading.Internal;
using LanceC.SpreadsheetIO.Reading.Internal.Properties;
using LanceC.SpreadsheetIO.Reading.Internal.Readers;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal
{
    public class ReadingSpreadsheetPageOperationFacts
    {
        private readonly AutoMocker _mocker = new();

        private ReadingSpreadsheetPageOperation<FakeModel> CreateSystemUnderTest()
            => _mocker.CreateInstance<ReadingSpreadsheetPageOperation<FakeModel>>();

        public class TheHeaderFailureProperty : ReadingSpreadsheetPageOperationFacts
        {
            [Fact]
            public void ReturnsFailureFromHeaderRowReadingResult()
            {
                // Arrange
                var headersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                var headerRowResult = new HeaderRowReadingResult<FakeModel>(
                    headersMock.Object,
                    new HeaderReadingFailure(
                        true,
                        Array.Empty<MissingHeaderReadingFailure>(),
                        Array.Empty<InvalidHeaderReadingFailure>()));
                _mocker.Use(headerRowResult);

                var sut = CreateSystemUnderTest();

                // Act
                var headerFailure = sut.HeaderFailure;

                // Assert
                Assert.Equal(headerRowResult.Failure, headerFailure);
            }
        }

        public class TheReadNextMethod : ReadingSpreadsheetPageOperationFacts
        {
            [Fact]
            public void ReturnsFalseWhenHeaderRowResultFailureIsNotNull()
            {
                // Arrange
                var headersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                var headerRowResult = new HeaderRowReadingResult<FakeModel>(
                    headersMock.Object,
                    new HeaderReadingFailure(
                        true,
                        Array.Empty<MissingHeaderReadingFailure>(),
                        Array.Empty<InvalidHeaderReadingFailure>()));
                _mocker.Use(headerRowResult);

                var sut = CreateSystemUnderTest();

                // Act
                var result = sut.ReadNext();

                // Assert
                Assert.False(result);
            }

            [Theory]
            [InlineData(false)]
            [InlineData(true)]
            public void ReturnsValueFromWorksheetRowReadingResult(bool expectedResult)
            {
                // Arrange
                _mocker.GetMock<IWorksheetElementReader>()
                    .Setup(worksheetReader => worksheetReader.ReadNextRow())
                    .Returns(expectedResult);

                var headersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                var headerRowResult = new HeaderRowReadingResult<FakeModel>(headersMock.Object, default);
                _mocker.Use(headerRowResult);

                var sut = CreateSystemUnderTest();

                // Act
                var actualResult = sut.ReadNext();

                // Assert
                Assert.Equal(expectedResult, actualResult);
            }

            [Fact]
            public void SetsCurrentResultToTheResourceReadingResultFromThePageReaderWhenWorksheetReaderReturnsTrue()
            {
                // Arrange
                var worksheetReaderMock = _mocker.GetMock<IWorksheetElementReader>();
                worksheetReaderMock.Setup(worksheetReader => worksheetReader.ReadNextRow())
                    .Returns(true);

                var headersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                var headerRowResult = new HeaderRowReadingResult<FakeModel>(headersMock.Object, default);
                _mocker.Use(headerRowResult);

                var map = new FakeModelMap();
                _mocker.Use<ResourceMap<FakeModel>>(map);

                var resourceReadingResult = new ResourceReadingResult<FakeModel>(new NumberedResource<FakeModel>(2U, new()), default);
                _mocker.GetMock<ISpreadsheetPageMapReader>()
                    .Setup(spreadsheetPageMapReader => spreadsheetPageMapReader.ReadBodyRow(
                        worksheetReaderMock.Object,
                        map,
                        headersMock.Object))
                    .Returns(resourceReadingResult);

                var sut = CreateSystemUnderTest();

                // Act
                sut.ReadNext();

                // Assert
                Assert.Equal(resourceReadingResult, sut.CurrentResult);
            }

            [Fact]
            public void SetsCurrentResultToNullWhenWorksheetReaderReturnsFalse()
            {
                // Arrange
                _mocker.GetMock<IWorksheetElementReader>()
                    .Setup(worksheetReader => worksheetReader.ReadNextRow())
                    .Returns(false);

                var headersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                var headerRowResult = new HeaderRowReadingResult<FakeModel>(headersMock.Object, default);
                _mocker.Use(headerRowResult);

                var map = new FakeModelMap();
                _mocker.Use<ResourceMap<FakeModel>>(map);

                var sut = CreateSystemUnderTest();

                // Act
                sut.ReadNext();

                // Assert
                Assert.Null(sut.CurrentResult);
            }
        }

        public class TheDisposeMethod : ReadingSpreadsheetPageOperationFacts
        {
            [Fact]
            public void DisposesWorksheetReader()
            {
                // Arrange
                var headersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                var headerRowResult = new HeaderRowReadingResult<FakeModel>(headersMock.Object, default);
                _mocker.Use(headerRowResult);

                var sut = CreateSystemUnderTest();

                // Act
                sut.Dispose();

                // Assert
                _mocker.GetMock<IWorksheetElementReader>()
                    .Verify(worksheetReader => worksheetReader.Dispose());
            }
        }
    }
}
