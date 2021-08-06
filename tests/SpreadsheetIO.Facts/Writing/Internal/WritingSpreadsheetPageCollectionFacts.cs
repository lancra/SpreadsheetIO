using System;
using LanceC.SpreadsheetIO.Writing;
using LanceC.SpreadsheetIO.Writing.Internal;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Writing.Internal
{
    public class WritingSpreadsheetPageCollectionFacts
    {
        private readonly AutoMocker _mocker = new();

        private static Mock<IWritingSpreadsheetPage> MockSpreadsheetPage(string name = "Name")
        {
            var spreadsheetPageMock = new Mock<IWritingSpreadsheetPage>();
            spreadsheetPageMock.SetupGet(spreadsheetPage => spreadsheetPage.Name)
                .Returns(name);
            return spreadsheetPageMock;
        }

        private WritingSpreadsheetPageCollection CreateSystemUnderTest()
            => _mocker.CreateInstance<WritingSpreadsheetPageCollection>();

        public class TheIntegerIndexer : WritingSpreadsheetPageCollectionFacts
        {
            [Fact]
            public void ReturnsPageAtIndex()
            {
                // Arrange
                var spreadsheetPageMock = MockSpreadsheetPage();

                var sut = CreateSystemUnderTest();
                sut.Add(spreadsheetPageMock.Object);

                // Act
                var spreadsheetPage = sut[0];

                // Assert
                Assert.Equal(spreadsheetPageMock.Object, spreadsheetPage);
            }
        }

        public class TheStringIndexer : WritingSpreadsheetPageCollectionFacts
        {
            [Fact]
            public void ReturnsPageForName()
            {
                // Arrange
                var name = "Name";
                var spreadsheetPageMock = MockSpreadsheetPage(name);

                var sut = CreateSystemUnderTest();
                sut.Add(spreadsheetPageMock.Object);

                // Act
                var spreadsheetPage = sut[name];

                // Assert
                Assert.Equal(spreadsheetPageMock.Object, spreadsheetPage);
            }

            [Fact]
            public void ThrowsArgumentExceptionWhenNoPageIsAddedForName()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut["Name"]);

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentException>(exception);
            }
        }

        public class TheAddMethod : WritingSpreadsheetPageCollectionFacts
        {
            [Fact]
            public void AddsPage()
            {
                // Arrange
                var spreadsheetPageMock = MockSpreadsheetPage();
                var sut = CreateSystemUnderTest();

                // Act
                sut.Add(spreadsheetPageMock.Object);

                // Assert
                Assert.Single(sut);
            }

            [Fact]
            public void ThrowsArgumentExceptionWhenAnotherPageWithTheSameNameHasAlreadyBeenAdded()
            {
                // Arrange
                var originalSpreadsheetPageMock = MockSpreadsheetPage(name: "Name");
                var spreadsheetPageMock = MockSpreadsheetPage(name: "Name");

                var sut = CreateSystemUnderTest();
                sut.Add(originalSpreadsheetPageMock.Object);

                // Act
                var exception = Record.Exception(() => sut.Add(spreadsheetPageMock.Object));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentException>(exception);
            }
        }

        public class TheDisposeMethod : WritingSpreadsheetPageCollectionFacts
        {
            [Fact]
            public void DisposesPages()
            {
                // Arrange
                var firstSpreadsheetPageMock = MockSpreadsheetPage(name: "One");
                var secondSpreadsheetPageMock = MockSpreadsheetPage(name: "Two");

                var sut = CreateSystemUnderTest();
                sut.Add(firstSpreadsheetPageMock.Object);
                sut.Add(secondSpreadsheetPageMock.Object);

                // Act
                sut.Dispose();

                // Assert
                firstSpreadsheetPageMock.Verify(spreadsheetPage => spreadsheetPage.Dispose());
                secondSpreadsheetPageMock.Verify(spreadsheetPage => spreadsheetPage.Dispose());
            }
        }
    }
}
