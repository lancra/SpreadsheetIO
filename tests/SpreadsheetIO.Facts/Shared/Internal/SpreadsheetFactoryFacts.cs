using System;
using System.IO;
using LanceC.SpreadsheetIO.Shared.Internal;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;
using LanceC.SpreadsheetIO.Writing.Internal;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Shared.Internal
{
    public class SpreadsheetFactoryFacts
    {
        private readonly AutoMocker _mocker = new AutoMocker();

        private SpreadsheetFactory CreateSystemUnderTest()
            => _mocker.CreateInstance<SpreadsheetFactory>();

        public class TheCreateMethodWithPathParameter : SpreadsheetFactoryFacts
        {
            [Fact]
            public void ReturnsWritingSpreadsheet()
            {
                // Arrange
                var path = new Uri(@"C:\Test");

                var spreadsheetDocumentMock = _mocker.GetMock<ISpreadsheetDocumentWrapper>();
                _mocker.GetMock<ISpreadsheetDocumentWrapperFactory>()
                    .Setup(spreadsheetDocumentFactory => spreadsheetDocumentFactory.Create(path.LocalPath))
                    .Returns(spreadsheetDocumentMock.Object);

                var sut = CreateSystemUnderTest();

                // Act
                var spreadsheet = sut.Create(path);

                // Assert
                Assert.IsType<WritingSpreadsheet>(spreadsheet);
            }
        }

        public class TheCreateMethodWithStreamParameter : SpreadsheetFactoryFacts
        {
            [Fact]
            public void ReturnsWritingSpreadsheet()
            {
                // Arrange
                var stream = Stream.Null;

                var spreadsheetDocumentMock = _mocker.GetMock<ISpreadsheetDocumentWrapper>();
                _mocker.GetMock<ISpreadsheetDocumentWrapperFactory>()
                    .Setup(spreadsheetDocumentFactory => spreadsheetDocumentFactory.Create(stream))
                    .Returns(spreadsheetDocumentMock.Object);

                var sut = CreateSystemUnderTest();

                // Act
                var spreadsheet = sut.Create(stream);

                // Assert
                Assert.IsType<WritingSpreadsheet>(spreadsheet);
            }
        }
    }
}
