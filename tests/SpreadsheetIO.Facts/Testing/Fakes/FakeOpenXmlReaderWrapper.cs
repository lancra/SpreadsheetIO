using DocumentFormat.OpenXml;
using LanceC.SpreadsheetIO.Shared.Internal.Wrappers;

namespace LanceC.SpreadsheetIO.Facts.Testing.Fakes
{
    public class FakeOpenXmlReaderWrapper : IOpenXmlReaderWrapper
    {
        private const string ExceptionMessage = "The reader is not currently pointed at an element.";

        private readonly IReadOnlyList<FakeOpenXmlReaderWrapperElement> _elements;

        private int _currentElementIndex = -1;
        private FakeOpenXmlReaderWrapperElement? _currentElement;

        public FakeOpenXmlReaderWrapper(IReadOnlyList<FakeOpenXmlReaderWrapperElement> elements)
        {
            _elements = elements;
        }

        public bool IsStartElement
            => _currentElement is not null ? _currentElement.IsStartElement : throw new InvalidOperationException(ExceptionMessage);

        public bool IsEndElement
            => _currentElement is not null ? _currentElement.IsEndElement : throw new InvalidOperationException(ExceptionMessage);

        public Type ElementType
            => _currentElement is not null ? _currentElement.ElementType : throw new InvalidOperationException(ExceptionMessage);

        public IReadOnlyCollection<OpenXmlAttribute> Attributes
            => _currentElement is not null ? _currentElement.Attributes : throw new InvalidOperationException(ExceptionMessage);

        public bool Read()
        {
            _currentElementIndex++;

            var isEnd = _currentElementIndex >= _elements.Count;
            _currentElement = !isEnd ? _elements[_currentElementIndex] : default;

            return !isEnd;
        }

        public string GetText()
            => _currentElement is not null ? _currentElement.Text : throw new InvalidOperationException(ExceptionMessage);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
