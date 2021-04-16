using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml;

namespace LanceC.SpreadsheetIO.Facts.Testing.Fakes
{
    public class FakeOpenXmlReaderWrapperElement
    {
        public FakeOpenXmlReaderWrapperElement(
            bool isStartElement,
            bool isEndElement,
            Type elementType,
            string text = "",
            params OpenXmlAttribute[] attributes)
        {
            IsStartElement = isStartElement;
            IsEndElement = isEndElement;
            ElementType = elementType;
            Text = text;
            Attributes = attributes;
        }

        public bool IsStartElement { get; }

        public bool IsEndElement { get; }

        public Type ElementType { get; }

        public string Text { get; }

        public IReadOnlyCollection<OpenXmlAttribute> Attributes { get; }
    }
}
