using System;
using System.Collections.Generic;
using System.Drawing;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.ResourceMaps;
using LanceC.SpreadsheetIO.Mapping.Internal;
using LanceC.SpreadsheetIO.Shared.Internal.Generators;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;
using LanceC.SpreadsheetIO.Styling;
using LanceC.SpreadsheetIO.Styling.Internal.Indexers;
using LanceC.SpreadsheetIO.Writing.Internal;
using LanceC.SpreadsheetIO.Writing.Internal.Writers;
using Moq;
using Moq.AutoMock;
using Xunit;
using OpenXml = DocumentFormat.OpenXml;

namespace LanceC.SpreadsheetIO.Facts.Writing.Internal
{
    public class WritingSpreadsheetFacts
    {
        private readonly AutoMocker _mocker = new();

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

        public class TheWritePageMethodWithResourceGenericParameter : WritingSpreadsheetFacts
        {
            [Fact]
            public void CallsMapWriterForAddedPage()
            {
                // Arrange
                var name = "Name";
                var resources = new[]
                {
                    new FakeModel { Id = 1, Name = "One", Display = "Uno", },
                    new FakeModel { Id = 2, Name = "Two", Display = "Dos", },
                    new FakeModel { Id = 3, Name = "Three", Display = "Tres", },
                };

                var map = new FakeModelMap()
                    .Map(model => model.Id)
                    .Map(model => model.Name)
                    .Map(model => model.Display);
                _mocker.GetMock<IResourceMapManager>()
                    .Setup(resourceMapManager => resourceMapManager.Single<FakeModel>())
                    .Returns(map);

                var worksheetPartMock = _mocker.GetMock<IWorksheetPartWrapper>();
                _mocker.GetMock<ISpreadsheetDocumentWrapper>()
                    .Setup(spreadsheetDocument => spreadsheetDocument.AddWorksheetPart(name))
                    .Returns(worksheetPartMock.Object);

                var spreadsheetPageMapWriterMock = _mocker.GetMock<ISpreadsheetPageMapWriter>();

                var sut = CreateSystemUnderTest();

                // Act
                var spreadsheetPage = sut.WritePage(name, resources);

                // Assert
                Assert.IsType<WritingSpreadsheetPage>(spreadsheetPage);
                spreadsheetPageMapWriterMock
                    .Verify(spreadsheetPageMapWriter => spreadsheetPageMapWriter.Write(
                        spreadsheetPage,
                        resources,
                        map));
            }

            [Fact]
            public void IndexesHeaderStylesFromPropertyMaps()
            {
                // Arrange
                var name = "Name";
                var resources = new[]
                {
                    new FakeModel { Id = 1, Name = "One", Display = "Uno", },
                    new FakeModel { Id = 2, Name = "Two", Display = "Dos", },
                    new FakeModel { Id = 3, Name = "Three", Display = "Tres", },
                };

                var customStyle = new Style(
                    new Border(
                        new BorderLine(Color.Black, BorderLineKind.Double),
                        BorderLine.Default,
                        BorderLine.Default,
                        BorderLine.Default),
                    Fill.Default,
                    Font.Default,
                    NumericFormat.Default,
                    Alignment.Default);
                var excelStyle = BuiltInExcelStyle.Bad;
                var packageStyle = BuiltInPackageStyle.Bold;
                var map = new FakeModelMap()
                    .Map(model => model.Id, optionsAction => optionsAction.UseHeaderStyle(customStyle, "foo"))
                    .Map(model => model.Name, optionsAction => optionsAction.UseHeaderStyle(excelStyle))
                    .Map(model => model.Display, optionsAction => optionsAction.UseHeaderStyle(packageStyle));
                _mocker.GetMock<IResourceMapManager>()
                    .Setup(resourceMapManager => resourceMapManager.Single<FakeModel>())
                    .Returns(map);

                var worksheetPartMock = _mocker.GetMock<IWorksheetPartWrapper>();
                _mocker.GetMock<ISpreadsheetDocumentWrapper>()
                    .Setup(spreadsheetDocument => spreadsheetDocument.AddWorksheetPart(name))
                    .Returns(worksheetPartMock.Object);

                var spreadsheetPageMapWriterMock = _mocker.GetMock<ISpreadsheetPageMapWriter>();

                var sut = CreateSystemUnderTest();

                // Act
                sut.WritePage(name, resources);

                // Assert
                _mocker.GetMock<IStyleIndexer>()
                    .Verify(styleIndexer => styleIndexer.Add(
                        It.Is<IndexerKey>(styleKey => styleKey.Name == "foo" && styleKey.Kind == IndexerKeyKind.Custom),
                        customStyle));
                _mocker.GetMock<IStyleIndexer>()
                    .Verify(styleIndexer => styleIndexer.Add(excelStyle.IndexerKey, excelStyle.Style));
                _mocker.GetMock<IStyleIndexer>()
                    .Verify(styleIndexer => styleIndexer.Add(packageStyle.IndexerKey, packageStyle.Style));
            }

            [Fact]
            public void IndexesBodyStylesFromPropertyMaps()
            {
                // Arrange
                var name = "Name";
                var resources = new[]
                {
                    new FakeModel { Id = 1, Name = "One", Display = "Uno", },
                    new FakeModel { Id = 2, Name = "Two", Display = "Dos", },
                    new FakeModel { Id = 3, Name = "Three", Display = "Tres", },
                };

                var customStyle = new Style(
                    new Border(
                        new BorderLine(Color.Black, BorderLineKind.Double),
                        BorderLine.Default,
                        BorderLine.Default,
                        BorderLine.Default),
                    Fill.Default,
                    Font.Default,
                    NumericFormat.Default,
                    Alignment.Default);
                var excelStyle = BuiltInExcelStyle.Bad;
                var packageStyle = BuiltInPackageStyle.Bold;
                var map = new FakeModelMap()
                    .Map(model => model.Id, optionsAction => optionsAction.UseBodyStyle(customStyle, "foo"))
                    .Map(model => model.Name, optionsAction => optionsAction.UseBodyStyle(excelStyle))
                    .Map(model => model.Display, optionsAction => optionsAction.UseBodyStyle(packageStyle));
                _mocker.GetMock<IResourceMapManager>()
                    .Setup(resourceMapManager => resourceMapManager.Single<FakeModel>())
                    .Returns(map);

                var worksheetPartMock = _mocker.GetMock<IWorksheetPartWrapper>();
                _mocker.GetMock<ISpreadsheetDocumentWrapper>()
                    .Setup(spreadsheetDocument => spreadsheetDocument.AddWorksheetPart(name))
                    .Returns(worksheetPartMock.Object);

                var spreadsheetPageMapWriterMock = _mocker.GetMock<ISpreadsheetPageMapWriter>();

                var sut = CreateSystemUnderTest();

                // Act
                sut.WritePage(name, resources);

                // Assert
                _mocker.GetMock<IStyleIndexer>()
                    .Verify(styleIndexer => styleIndexer.Add(
                        It.Is<IndexerKey>(styleKey => styleKey.Name == "foo" && styleKey.Kind == IndexerKeyKind.Custom),
                        customStyle));
                _mocker.GetMock<IStyleIndexer>()
                    .Verify(styleIndexer => styleIndexer.Add(excelStyle.IndexerKey, excelStyle.Style));
                _mocker.GetMock<IStyleIndexer>()
                    .Verify(styleIndexer => styleIndexer.Add(packageStyle.IndexerKey, packageStyle.Style));
            }
        }

        public class TheWritePageMethodWithResourceAndResourceMapGenericParameters : WritingSpreadsheetFacts
        {
            [Fact]
            public void CallsMapWriterForAddedPage()
            {
                // Arrange
                var name = "Name";
                var resources = new[]
                {
                    new FakeModel { Id = 1, Name = "One", Display = "Uno", },
                    new FakeModel { Id = 2, Name = "Two", Display = "Dos", },
                    new FakeModel { Id = 3, Name = "Three", Display = "Tres", },
                };

                var map = new FakeModelMap()
                    .Map(model => model.Id)
                    .Map(model => model.Name)
                    .Map(model => model.Display);
                _mocker.GetMock<IResourceMapManager>()
                    .Setup(resourceMapManager => resourceMapManager.Single<FakeModel, FakeModelMap>())
                    .Returns(map);

                var worksheetPartMock = _mocker.GetMock<IWorksheetPartWrapper>();
                _mocker.GetMock<ISpreadsheetDocumentWrapper>()
                    .Setup(spreadsheetDocument => spreadsheetDocument.AddWorksheetPart(name))
                    .Returns(worksheetPartMock.Object);

                var spreadsheetPageMapWriterMock = _mocker.GetMock<ISpreadsheetPageMapWriter>();

                var sut = CreateSystemUnderTest();

                // Act
                var spreadsheetPage = sut.WritePage<FakeModel, FakeModelMap>(name, resources);

                // Assert
                Assert.IsType<WritingSpreadsheetPage>(spreadsheetPage);
                spreadsheetPageMapWriterMock
                    .Verify(spreadsheetPageMapWriter => spreadsheetPageMapWriter.Write(
                        spreadsheetPage,
                        resources,
                        map));
            }

            [Fact]
            public void IndexesHeaderStylesFromPropertyMaps()
            {
                // Arrange
                var name = "Name";
                var resources = new[]
                {
                    new FakeModel { Id = 1, Name = "One", Display = "Uno", },
                    new FakeModel { Id = 2, Name = "Two", Display = "Dos", },
                    new FakeModel { Id = 3, Name = "Three", Display = "Tres", },
                };

                var customStyle = new Style(
                    new Border(
                        new BorderLine(Color.Black, BorderLineKind.Double),
                        BorderLine.Default,
                        BorderLine.Default,
                        BorderLine.Default),
                    Fill.Default,
                    Font.Default,
                    NumericFormat.Default,
                    Alignment.Default);
                var excelStyle = BuiltInExcelStyle.Bad;
                var packageStyle = BuiltInPackageStyle.Bold;
                var map = new FakeModelMap()
                    .Map(model => model.Id, optionsAction => optionsAction.UseHeaderStyle(customStyle, "foo"))
                    .Map(model => model.Name, optionsAction => optionsAction.UseHeaderStyle(excelStyle))
                    .Map(model => model.Display, optionsAction => optionsAction.UseHeaderStyle(packageStyle));
                _mocker.GetMock<IResourceMapManager>()
                    .Setup(resourceMapManager => resourceMapManager.Single<FakeModel, FakeModelMap>())
                    .Returns(map);

                var worksheetPartMock = _mocker.GetMock<IWorksheetPartWrapper>();
                _mocker.GetMock<ISpreadsheetDocumentWrapper>()
                    .Setup(spreadsheetDocument => spreadsheetDocument.AddWorksheetPart(name))
                    .Returns(worksheetPartMock.Object);

                var spreadsheetPageMapWriterMock = _mocker.GetMock<ISpreadsheetPageMapWriter>();

                var sut = CreateSystemUnderTest();

                // Act
                sut.WritePage<FakeModel, FakeModelMap>(name, resources);

                // Assert
                _mocker.GetMock<IStyleIndexer>()
                    .Verify(styleIndexer => styleIndexer.Add(
                        It.Is<IndexerKey>(styleKey => styleKey.Name == "foo" && styleKey.Kind == IndexerKeyKind.Custom),
                        customStyle));
                _mocker.GetMock<IStyleIndexer>()
                    .Verify(styleIndexer => styleIndexer.Add(excelStyle.IndexerKey, excelStyle.Style));
                _mocker.GetMock<IStyleIndexer>()
                    .Verify(styleIndexer => styleIndexer.Add(packageStyle.IndexerKey, packageStyle.Style));
            }

            [Fact]
            public void IndexesBodyStylesFromPropertyMaps()
            {
                // Arrange
                var name = "Name";
                var resources = new[]
                {
                    new FakeModel { Id = 1, Name = "One", Display = "Uno", },
                    new FakeModel { Id = 2, Name = "Two", Display = "Dos", },
                    new FakeModel { Id = 3, Name = "Three", Display = "Tres", },
                };

                var customStyle = new Style(
                    new Border(
                        new BorderLine(Color.Black, BorderLineKind.Double),
                        BorderLine.Default,
                        BorderLine.Default,
                        BorderLine.Default),
                    Fill.Default,
                    Font.Default,
                    NumericFormat.Default,
                    Alignment.Default);
                var excelStyle = BuiltInExcelStyle.Bad;
                var packageStyle = BuiltInPackageStyle.Bold;
                var map = new FakeModelMap()
                    .Map(model => model.Id, optionsAction => optionsAction.UseBodyStyle(customStyle, "foo"))
                    .Map(model => model.Name, optionsAction => optionsAction.UseBodyStyle(excelStyle))
                    .Map(model => model.Display, optionsAction => optionsAction.UseBodyStyle(packageStyle));
                _mocker.GetMock<IResourceMapManager>()
                    .Setup(resourceMapManager => resourceMapManager.Single<FakeModel, FakeModelMap>())
                    .Returns(map);

                var worksheetPartMock = _mocker.GetMock<IWorksheetPartWrapper>();
                _mocker.GetMock<ISpreadsheetDocumentWrapper>()
                    .Setup(spreadsheetDocument => spreadsheetDocument.AddWorksheetPart(name))
                    .Returns(worksheetPartMock.Object);

                var spreadsheetPageMapWriterMock = _mocker.GetMock<ISpreadsheetPageMapWriter>();

                var sut = CreateSystemUnderTest();

                // Act
                sut.WritePage<FakeModel, FakeModelMap>(name, resources);

                // Assert
                _mocker.GetMock<IStyleIndexer>()
                    .Verify(styleIndexer => styleIndexer.Add(
                        It.Is<IndexerKey>(styleKey => styleKey.Name == "foo" && styleKey.Kind == IndexerKeyKind.Custom),
                        customStyle));
                _mocker.GetMock<IStyleIndexer>()
                    .Verify(styleIndexer => styleIndexer.Add(excelStyle.IndexerKey, excelStyle.Style));
                _mocker.GetMock<IStyleIndexer>()
                    .Verify(styleIndexer => styleIndexer.Add(packageStyle.IndexerKey, packageStyle.Style));
            }
        }

        public class TheAddStyleMethodWithNameAndStyleParameters : WritingSpreadsheetFacts
        {
            [Fact]
            public void AddsStyleToIndexer()
            {
                // Arrange
                var name = "Style";
                var style = Style.Default;

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
                var style = Style.Default;

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
                var style = Style.Default;

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
