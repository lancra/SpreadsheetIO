using LanceC.SpreadsheetIO.Facts.Testing.Fakes;
using LanceC.SpreadsheetIO.Reading.Internal.Readers;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;
using Moq.AutoMock;
using Xunit;
using OpenXml = DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal.Readers;

public class SharedStringTableElementReaderFacts
{
    private readonly AutoMocker _mocker = new();

    private SharedStringTableElementReader CreateSystemUnderTest()
        => _mocker.CreateInstance<SharedStringTableElementReader>();

    public class TheReadNextItemMethod : SharedStringTableElementReaderFacts
    {
        [Fact]
        public void ReturnsTrueWhenTheNextSharedStringItemIsRead()
        {
            // Arrange
            var readerElements = new[]
            {
                new FakeOpenXmlReaderWrapperElement(true, false, typeof(OpenXml.SharedStringTable)),
                new FakeOpenXmlReaderWrapperElement(true, false, typeof(OpenXml.SharedStringItem)),
                new FakeOpenXmlReaderWrapperElement(true, true, typeof(OpenXml.Text)),
                new FakeOpenXmlReaderWrapperElement(false, true, typeof(OpenXml.SharedStringItem)),
                new FakeOpenXmlReaderWrapperElement(false, true, typeof(OpenXml.SharedStringTable)),
            };

            var readerMock = new FakeOpenXmlReaderWrapper(readerElements);
            _mocker.Use<IOpenXmlReaderWrapper>(readerMock);

            var sut = CreateSystemUnderTest();

            // Act
            var hasNextItem = sut.ReadNextItem();

            // Assert
            Assert.True(hasNextItem);
        }

        [Fact]
        public void ReturnsFalseWhenTheEndOfTheSharedStringTableIsReached()
        {
            // Arrange
            var readerElements = new[]
            {
                new FakeOpenXmlReaderWrapperElement(true, false, typeof(OpenXml.SharedStringTable)),
                new FakeOpenXmlReaderWrapperElement(true, false, typeof(OpenXml.SharedStringItem)),
                new FakeOpenXmlReaderWrapperElement(true, true, typeof(OpenXml.Text)),
                new FakeOpenXmlReaderWrapperElement(false, true, typeof(OpenXml.SharedStringItem)),
                new FakeOpenXmlReaderWrapperElement(false, true, typeof(OpenXml.SharedStringTable)),
            };

            var readerMock = new FakeOpenXmlReaderWrapper(readerElements);
            readerMock.Read();
            readerMock.Read();
            _mocker.Use<IOpenXmlReaderWrapper>(readerMock);

            var sut = CreateSystemUnderTest();

            // Act
            var hasNextItem = sut.ReadNextItem();

            // Assert
            Assert.False(hasNextItem);
        }
    }

    public class TheGetItemValueMethod : SharedStringTableElementReaderFacts
    {
        [Fact]
        public void ReturnsContentsOfChildTextElement()
        {
            // Arrange
            var expectedItemValue = "foo";

            var readerElements = new[]
            {
                new FakeOpenXmlReaderWrapperElement(true, false, typeof(OpenXml.SharedStringTable)),
                new FakeOpenXmlReaderWrapperElement(true, false, typeof(OpenXml.SharedStringItem)),
                new FakeOpenXmlReaderWrapperElement(true, true, typeof(OpenXml.Text), text: expectedItemValue),
                new FakeOpenXmlReaderWrapperElement(false, true, typeof(OpenXml.SharedStringItem)),
                new FakeOpenXmlReaderWrapperElement(false, true, typeof(OpenXml.SharedStringTable)),
            };

            var readerMock = new FakeOpenXmlReaderWrapper(readerElements);
            _mocker.Use<IOpenXmlReaderWrapper>(readerMock);

            var sut = CreateSystemUnderTest();
            sut.ReadNextItem();

            // Act
            var actualItemValue = sut.GetItemValue();

            // Assert
            Assert.Equal(expectedItemValue, actualItemValue);
        }

        [Fact]
        public void ThrowsInvalidOperationExceptionWhenTheCurrentElementIsNotSharedStringItemStartElement()
        {
            // Arrange
            var readerElements = new[]
            {
                new FakeOpenXmlReaderWrapperElement(true, false, typeof(OpenXml.SharedStringTable)),
                new FakeOpenXmlReaderWrapperElement(true, false, typeof(OpenXml.SharedStringItem)),
                new FakeOpenXmlReaderWrapperElement(true, true, typeof(OpenXml.Text)),
                new FakeOpenXmlReaderWrapperElement(false, true, typeof(OpenXml.SharedStringItem)),
                new FakeOpenXmlReaderWrapperElement(false, true, typeof(OpenXml.SharedStringTable)),
            };

            var readerMock = new FakeOpenXmlReaderWrapper(readerElements);
            readerMock.Read();
            _mocker.Use<IOpenXmlReaderWrapper>(readerMock);

            var sut = CreateSystemUnderTest();

            // Act
            var exception = Record.Exception(() => sut.GetItemValue());

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
        }
    }

    public class TheDisposeMethod : SharedStringTableElementReaderFacts
    {
        [Fact]
        public void DisposesOpenXmlReader()
        {
            // Arrange
            var readerMock = _mocker.GetMock<IOpenXmlReaderWrapper>();
            var sut = CreateSystemUnderTest();

            // Act
            sut.Dispose();

            // Assert
            readerMock.Verify(reader => reader.Dispose());
        }
    }
}
