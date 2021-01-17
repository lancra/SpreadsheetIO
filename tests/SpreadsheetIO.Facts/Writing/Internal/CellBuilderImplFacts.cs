using LanceC.SpreadsheetIO.Styling;
using LanceC.SpreadsheetIO.Writing;
using LanceC.SpreadsheetIO.Writing.Internal;
using Moq.AutoMock;
using Xunit;
using OpenXml = DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Facts.Writing.Internal
{
    public class CellBuilderImplFacts
    {
        private readonly AutoMocker _mocker = new AutoMocker();

        private CellBuilderImpl CreateSystemUnderTest()
        {
            _mocker.Use(new OpenXml.Cell());
            return _mocker.CreateInstance<CellBuilderImpl>();
        }

        public class TheWithStyleMethod : CellBuilderImplFacts
        {
            [Fact]
            public void SetsStyleProperty()
            {
                // Arrange
                var style = BuiltInExcelStyle.Normal.Style;
                var sut = CreateSystemUnderTest();

                // Act
                sut.WithStyle(style);

                // Assert
                Assert.Equal(style, sut.Style);
            }

            [Fact]
            public void ReturnsSelf()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                var cellValueBuilder = sut.WithStyle(BuiltInExcelStyle.Normal.Style);

                // Assert
                Assert.Equal(sut, cellValueBuilder);
            }
        }

        public class TheWrittenAsMethod : CellBuilderImplFacts
        {
            [Fact]
            public void SetsStringKindProperty()
            {
                // Arrange
                var stringKind = CellStringKind.InlineString;
                var sut = CreateSystemUnderTest();

                // Act
                sut.WrittenAs(stringKind);

                // Assert
                Assert.Equal(stringKind, sut.StringKind);
            }

            [Fact]
            public void ReturnsSelf()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                var cellStringValueBuilder = sut.WrittenAs(CellStringKind.InlineString);

                // Assert
                Assert.Equal(sut, cellStringValueBuilder);
            }
        }
    }
}
