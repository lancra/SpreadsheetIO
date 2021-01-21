using System;
using System.Collections.Generic;
using LanceC.SpreadsheetIO.Shared.Internal.Generators;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;
using LanceC.SpreadsheetIO.Styling;
using LanceC.SpreadsheetIO.Styling.Internal.Indexers;
using LanceC.SpreadsheetIO.Writing.Internal;
using Moq;
using Moq.AutoMock;
using Xunit;
using OpenXml = DocumentFormat.OpenXml;

namespace LanceC.SpreadsheetIO.Facts.Writing.Internal
{
    public class WritingSpreadsheetFacts
    {
        private readonly AutoMocker _mocker = new AutoMocker();

        private WritingSpreadsheet CreateSystemUnderTest()
        {
            var writerMock = _mocker.GetMock<IOpenXmlWriterWrapper>();
            writerMock.Setup(writer => writer.WriteElement(It.IsAny<OpenXml.OpenXmlElement>()))
                .Returns(writerMock.Object);
            writerMock.Setup(writer => writer.WriteStartElement(It.IsAny<OpenXml.OpenXmlElement>()))
                .Returns(writerMock.Object);
            writerMock.Setup(writer => writer.WriteEndElement())
                .Returns(writerMock.Object);

            _mocker.GetMock<IWorksheetPartWrapper>()
                .Setup(worksheetPart => worksheetPart.CreateWriter())
                .Returns(writerMock.Object);

            return _mocker.CreateInstance<WritingSpreadsheet>();
        }

        public class ThePagesProperty : WritingSpreadsheetFacts
        {
            [Fact]
            public void ReturnsSpreadsheetPages()
            {
                // Arrange
                var count = 10;
                _mocker.GetMock<IWritingSpreadsheetPageCollectionModifiable>()
                    .SetupGet(spreadsheetPages => spreadsheetPages.Count)
                    .Returns(count);

                var sut = CreateSystemUnderTest();

                // Act
                var spreadsheetPages = sut.Pages;

                // Assert
                Assert.Equal(count, spreadsheetPages.Count);
            }
        }

        public class TheAddPageMethod : WritingSpreadsheetFacts
        {
            [Fact]
            public void ReturnsSpreadsheetPage()
            {
                // Arrange
                var name = "Name";

                var worksheetPartMock = _mocker.GetMock<IWorksheetPartWrapper>();

                _mocker.GetMock<ISpreadsheetDocumentWrapper>()
                    .Setup(spreadsheetDocument => spreadsheetDocument.AddWorksheetPart(name))
                    .Returns(worksheetPartMock.Object);

                var sut = CreateSystemUnderTest();

                // Act
                var spreadsheetPage = sut.AddPage(name);

                // Assert
                Assert.IsType<WritingSpreadsheetPage>(spreadsheetPage);
            }

            [Fact]
            public void AddsPageToPagesCollection()
            {
                // Arrange
                var name = "Name";

                var worksheetPartMock = _mocker.GetMock<IWorksheetPartWrapper>();

                _mocker.GetMock<ISpreadsheetDocumentWrapper>()
                    .Setup(spreadsheetDocument => spreadsheetDocument.AddWorksheetPart(name))
                    .Returns(worksheetPartMock.Object);

                var sut = CreateSystemUnderTest();

                // Act
                var spreadsheetPage = sut.AddPage(name);

                // Assert
                _mocker.GetMock<IWritingSpreadsheetPageCollectionModifiable>()
                    .Verify(spreadsheetPages => spreadsheetPages.Add(spreadsheetPage));
            }

            [Fact]
            public void ThrowsArgumentExceptionWhenNameIsEmpty()
            {
                // Arrange
                var name = string.Empty;
                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut.AddPage(name));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentException>(exception);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenNameIsNull()
            {
                // Arrange
                var name = default(string?);
                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut.AddPage(name!));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }
        }

        public class TheAddStyleMethodWithNameAndStyleParameters : WritingSpreadsheetFacts
        {
            [Fact]
            public void AddsStyleToIndexer()
            {
                // Arrange
                var name = "Style";
                var style = new Style(Border.Default, Fill.Default, Font.Default);

                var sut = CreateSystemUnderTest();

                // Act
                sut.AddStyle(name, style);

                // Assert
                _mocker.GetMock<IStyleIndexer>()
                    .Verify(styleIndexer => styleIndexer.Add(
                        It.Is<IndexerKey>(styleKey => styleKey.Name == name && styleKey.Kind == IndexerKeyKind.Custom),
                        style));
            }

            [Fact]
            public void ThrowsArgumentExceptionWhenNameIsEmpty()
            {
                // Arrange
                var name = string.Empty;
                var style = new Style(Border.Default, Fill.Default, Font.Default);

                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut.AddStyle(name, style));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentException>(exception);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenNameIsNull()
            {
                // Arrange
                var name = default(string?);
                var style = new Style(Border.Default, Fill.Default, Font.Default);

                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut.AddStyle(name!, style));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenStyleIsNull()
            {
                // Arrange
                var name = "Name";
                var style = default(Style);

                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut.AddStyle(name, style!));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }
        }

        public class TheAddStyleMethodWithBuiltInExcelStyleParameter : WritingSpreadsheetFacts
        {
            [Fact]
            public void AddsStyleToIndexer()
            {
                // Arrange
                var style = BuiltInExcelStyle.Normal;
                var sut = CreateSystemUnderTest();

                // Act
                sut.AddStyle(style);

                // Assert
                _mocker.GetMock<IStyleIndexer>()
                    .Verify(styleIndexer => styleIndexer.Add(style.IndexerKey, style.Style));
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenStyleIsNull()
            {
                // Arrange
                var style = default(BuiltInExcelStyle);
                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut.AddStyle(style!));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }
        }

        public class TheAddStyleMethodWithBuiltInPackageStyleParameter : WritingSpreadsheetFacts
        {
            [Fact]
            public void AddsStyleToIndexer()
            {
                // Arrange
                var style = BuiltInPackageStyle.Bold;
                var sut = CreateSystemUnderTest();

                // Act
                sut.AddStyle(style);

                // Assert
                _mocker.GetMock<IStyleIndexer>()
                    .Verify(styleIndexer => styleIndexer.Add(style.IndexerKey, style.Style));
            }

            [Fact]
            public void ThrowsArgumentNullExceptionWhenStyleIsNull()
            {
                // Arrange
                var style = default(BuiltInPackageStyle);
                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut.AddStyle(style!));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
            }
        }

        public class TheDisposeMethod : WritingSpreadsheetFacts
        {
            [Fact]
            public void ExecutesSpreadsheetGenerators()
            {
                // Arrange
                var firstSpreadsheetGeneratorMock = new Mock<ISpreadsheetGenerator>();
                var secondSpreadsheetGeneratorMock = new Mock<ISpreadsheetGenerator>();
                _mocker.Use<IEnumerable<ISpreadsheetGenerator>>(
                    new[] { firstSpreadsheetGeneratorMock.Object, secondSpreadsheetGeneratorMock.Object, });

                var spreadsheetDocumentMock = _mocker.GetMock<ISpreadsheetDocumentWrapper>();

                var sut = CreateSystemUnderTest();

                // Act
                sut.Dispose();

                // Assert
                firstSpreadsheetGeneratorMock
                    .Verify(spreadsheetGenerator => spreadsheetGenerator.Generate(spreadsheetDocumentMock.Object));
                secondSpreadsheetGeneratorMock
                    .Verify(spreadsheetGenerator => spreadsheetGenerator.Generate(spreadsheetDocumentMock.Object));
            }

            [Fact]
            public void DisposesSpreadsheetPageCollection()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                sut.Dispose();

                // Assert
                _mocker.GetMock<IWritingSpreadsheetPageCollectionModifiable>()
                    .Verify(spreadsheetPages => spreadsheetPages.Dispose());
            }

            [Fact]
            public void DisposesSpreadsheetDocument()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                sut.Dispose();

                // Assert
                _mocker.GetMock<ISpreadsheetDocumentWrapper>()
                    .Verify(spreadsheetDocument => spreadsheetDocument.Dispose());
            }
        }
    }
}
