using LanceC.SpreadsheetIO.Mapping.Internal;
using LanceC.SpreadsheetIO.Reading.Internal;
using LanceC.SpreadsheetIO.Reading.Internal.Readers;
using LanceC.SpreadsheetIO.Shared.Internal;
using LanceC.SpreadsheetIO.Shared.Internal.Generators;
using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;
using LanceC.SpreadsheetIO.Styling.Internal.Indexers;
using LanceC.SpreadsheetIO.Writing.Internal;
using LanceC.SpreadsheetIO.Writing.Internal.Writers;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Shared.Internal;

public class SpreadsheetFactoryFacts
{
    private readonly AutoMocker _mocker = new();

    private SpreadsheetFactory CreateSystemUnderTest()
        => _mocker.CreateInstance<SpreadsheetFactory>();

    private Mock<IServiceProvider> MockServiceProvider()
    {
        var serviceProviderMock = _mocker.GetMock<IServiceProvider>();

        var serviceScopeMock = _mocker.GetMock<IServiceScope>();
        serviceScopeMock.Setup(serviceScope => serviceScope.ServiceProvider)
            .Returns(serviceProviderMock.Object);

        var serviceScopeFactoryMock = _mocker.GetMock<IServiceScopeFactory>();
        serviceScopeFactoryMock.Setup(serviceScopeFactory => serviceScopeFactory.CreateScope())
            .Returns(serviceScopeMock.Object);

        serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IServiceScopeFactory)))
            .Returns(serviceScopeFactoryMock.Object);

        return serviceProviderMock;
    }

    private void MockForCreate(Mock<IServiceProvider> serviceProviderMock)
    {
        serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IStyleIndexer)))
            .Returns(_mocker.GetMock<IStyleIndexer>().Object);
        serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IStringIndexer)))
            .Returns(_mocker.GetMock<IStringIndexer>().Object);
        serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IEnumerable<ISpreadsheetGenerator>)))
            .Returns(new[] { _mocker.GetMock<ISpreadsheetGenerator>().Object, });
        serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(ISpreadsheetPageMapWriter)))
            .Returns(_mocker.GetMock<ISpreadsheetPageMapWriter>().Object);
        serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IResourceMapManager)))
            .Returns(_mocker.GetMock<IResourceMapManager>().Object);
    }

    private void MockForOpenRead(Mock<IServiceProvider> serviceProviderMock)
    {
        serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IElementReaderFactory)))
            .Returns(_mocker.GetMock<IElementReaderFactory>().Object);
        serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IResourceMapManager)))
            .Returns(_mocker.GetMock<IResourceMapManager>().Object);
        serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IMappedHeaderRowReader)))
            .Returns(_mocker.GetMock<IMappedHeaderRowReader>().Object);
        serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IReadingSpreadsheetPageOperationFactory)))
            .Returns(_mocker.GetMock<IReadingSpreadsheetPageOperationFactory>().Object);
        serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IStringIndexer)))
            .Returns(_mocker.GetMock<IStringIndexer>().Object);
    }

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

            var serviceProviderMock = MockServiceProvider();
            MockForCreate(serviceProviderMock);

            var sut = CreateSystemUnderTest();

            // Act
            var spreadsheet = sut.Create(path);

            // Assert
            Assert.IsType<WritingSpreadsheet>(spreadsheet);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenPathIsNull()
        {
            // Arrange
            var path = default(Uri);
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.Create(path!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
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

            var serviceProviderMock = MockServiceProvider();
            MockForCreate(serviceProviderMock);

            var sut = CreateSystemUnderTest();

            // Act
            var spreadsheet = sut.Create(stream);

            // Assert
            Assert.IsType<WritingSpreadsheet>(spreadsheet);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenStreamIsNull()
        {
            // Arrange
            var stream = default(Stream);
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.Create(stream!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheOpenReadMethodWithPathParameter : SpreadsheetFactoryFacts
    {
        [Fact]
        public void ReturnsReadingSpreadsheet()
        {
            // Arrange
            var path = new Uri(@"C:\Test");

            var worksheetPartMock = _mocker.GetMock<IWorksheetPartWrapper>();
            worksheetPartMock.SetupGet(worksheetPart => worksheetPart.Name)
                .Returns("Sheet1");

            var spreadsheetDocumentMock = _mocker.GetMock<ISpreadsheetDocumentWrapper>();
            spreadsheetDocumentMock.SetupGet(spreadsheetDocument => spreadsheetDocument.WorksheetParts)
                .Returns(new[] { worksheetPartMock.Object, });

            _mocker.GetMock<ISpreadsheetDocumentWrapperFactory>()
                .Setup(spreadsheetDocumentFactory => spreadsheetDocumentFactory.Open(path.LocalPath, false))
                .Returns(spreadsheetDocumentMock.Object);

            var serviceProviderMock = MockServiceProvider();
            MockForOpenRead(serviceProviderMock);

            var sut = CreateSystemUnderTest();

            // Act
            var spreadsheet = sut.OpenRead(path);

            // Assert
            Assert.IsType<ReadingSpreadsheet>(spreadsheet);
            Assert.Single(spreadsheet.Pages);
        }

        [Fact]
        public void PopulatesStringIndexerWhenSharedStringTableIsPresent()
        {
            // Arrange
            var path = new Uri(@"C:\Test");
            var firstExpectedItemValue = "foo";
            var secondExpectedItemValue = "bar";

            var sharedStringTablePartMock = _mocker.GetMock<ISharedStringTablePartWrapper>();

            var spreadsheetDocumentMock = _mocker.GetMock<ISpreadsheetDocumentWrapper>();
            spreadsheetDocumentMock.SetupGet(spreadsheetDocument => spreadsheetDocument.WorksheetParts)
                .Returns(Array.Empty<IWorksheetPartWrapper>());
            spreadsheetDocumentMock.SetupGet(spreadsheetDocument => spreadsheetDocument.SharedStringTablePart)
                .Returns(sharedStringTablePartMock.Object);

            _mocker.GetMock<ISpreadsheetDocumentWrapperFactory>()
                .Setup(spreadsheetDocumentFactory => spreadsheetDocumentFactory.Open(path.LocalPath, false))
                .Returns(spreadsheetDocumentMock.Object);

            var sharedStringTableReaderMock = _mocker.GetMock<ISharedStringTableElementReader>();
            sharedStringTableReaderMock.SetupSequence(sharedStringTableReader => sharedStringTableReader.ReadNextItem())
                .Returns(true)
                .Returns(true)
                .Returns(false);
            sharedStringTableReaderMock.SetupSequence(sharedStringTableReader => sharedStringTableReader.GetItemValue())
                .Returns(firstExpectedItemValue)
                .Returns(secondExpectedItemValue);

            _mocker.GetMock<IElementReaderFactory>()
                .Setup(elementReaderFactory => elementReaderFactory.CreateSharedStringTableReader(
                    sharedStringTablePartMock.Object))
                .Returns(sharedStringTableReaderMock.Object);

            var serviceProviderMock = MockServiceProvider();
            MockForOpenRead(serviceProviderMock);

            var sut = CreateSystemUnderTest();

            // Act
            sut.OpenRead(path);

            // Assert
            var stringIndexerMock = _mocker.GetMock<IStringIndexer>();
            stringIndexerMock.Verify(stringIndexer => stringIndexer.Add(firstExpectedItemValue));
            stringIndexerMock.Verify(stringIndexer => stringIndexer.Add(secondExpectedItemValue));
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenPathIsNull()
        {
            // Arrange
            var path = default(Uri);
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.OpenRead(path!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    public class TheOpenReadMethodWithStreamParameter : SpreadsheetFactoryFacts
    {
        [Fact]
        public void ReturnsReadingSpreadsheet()
        {
            // Arrange
            var stream = Stream.Null;

            var worksheetPartMock = _mocker.GetMock<IWorksheetPartWrapper>();
            worksheetPartMock.SetupGet(worksheetPart => worksheetPart.Name)
                .Returns("Sheet1");

            var spreadsheetDocumentMock = _mocker.GetMock<ISpreadsheetDocumentWrapper>();
            spreadsheetDocumentMock.SetupGet(spreadsheetDocument => spreadsheetDocument.WorksheetParts)
                .Returns(new[] { worksheetPartMock.Object, });

            _mocker.GetMock<ISpreadsheetDocumentWrapperFactory>()
                .Setup(spreadsheetDocumentFactory => spreadsheetDocumentFactory.Open(stream, false))
                .Returns(spreadsheetDocumentMock.Object);

            var serviceProviderMock = MockServiceProvider();
            MockForOpenRead(serviceProviderMock);

            var sut = CreateSystemUnderTest();

            // Act
            var spreadsheet = sut.OpenRead(stream);

            // Assert
            Assert.IsType<ReadingSpreadsheet>(spreadsheet);
            Assert.Single(spreadsheet.Pages);
        }

        [Fact]
        public void PopulatesStringIndexerWhenSharedStringTableIsPresent()
        {
            // Arrange
            var stream = Stream.Null;
            var firstExpectedItemValue = "foo";
            var secondExpectedItemValue = "bar";

            var sharedStringTablePartMock = _mocker.GetMock<ISharedStringTablePartWrapper>();

            var spreadsheetDocumentMock = _mocker.GetMock<ISpreadsheetDocumentWrapper>();
            spreadsheetDocumentMock.SetupGet(spreadsheetDocument => spreadsheetDocument.WorksheetParts)
                .Returns(Array.Empty<IWorksheetPartWrapper>());
            spreadsheetDocumentMock.SetupGet(spreadsheetDocument => spreadsheetDocument.SharedStringTablePart)
                .Returns(sharedStringTablePartMock.Object);

            _mocker.GetMock<ISpreadsheetDocumentWrapperFactory>()
                .Setup(spreadsheetDocumentFactory => spreadsheetDocumentFactory.Open(stream, false))
                .Returns(spreadsheetDocumentMock.Object);

            var sharedStringTableReaderMock = _mocker.GetMock<ISharedStringTableElementReader>();
            sharedStringTableReaderMock.SetupSequence(sharedStringTableReader => sharedStringTableReader.ReadNextItem())
                .Returns(true)
                .Returns(true)
                .Returns(false);
            sharedStringTableReaderMock.SetupSequence(sharedStringTableReader => sharedStringTableReader.GetItemValue())
                .Returns(firstExpectedItemValue)
                .Returns(secondExpectedItemValue);

            _mocker.GetMock<IElementReaderFactory>()
                .Setup(elementReaderFactory => elementReaderFactory.CreateSharedStringTableReader(
                    sharedStringTablePartMock.Object))
                .Returns(sharedStringTableReaderMock.Object);

            var serviceProviderMock = MockServiceProvider();
            MockForOpenRead(serviceProviderMock);

            var sut = CreateSystemUnderTest();

            // Act
            sut.OpenRead(stream);

            // Assert
            var stringIndexerMock = _mocker.GetMock<IStringIndexer>();
            stringIndexerMock.Verify(stringIndexer => stringIndexer.Add(firstExpectedItemValue));
            stringIndexerMock.Verify(stringIndexer => stringIndexer.Add(secondExpectedItemValue));
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenStreamIsNull()
        {
            // Arrange
            var stream = default(Stream);
            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.OpenRead(stream!));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }
}
