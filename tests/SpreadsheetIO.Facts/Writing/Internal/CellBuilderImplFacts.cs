using LanceC.SpreadsheetIO.Shared.Internal.Indexers;
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

        public class TheWithStyleMethodWithStringParameter : CellBuilderImplFacts
        {
            [Fact]
            public void SetsStyleKeyProperty()
            {
                // Arrange
                var name = "Style";
                var sut = CreateSystemUnderTest();

                // Act
                sut.WithStyle(name);

                // Assert
                Assert.NotNull(sut.StyleKey);
                Assert.Equal(name, sut.StyleKey!.Name);
                Assert.Equal(IndexerKeyKind.Custom, sut.StyleKey.Kind);
            }

            [Fact]
            public void ReturnsSelf()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                var cellValueBuilder = sut.WithStyle("Style");

                // Assert
                Assert.Equal(sut, cellValueBuilder);
            }
        }

        public class TheWithStyleMethodWithBuiltInExcelStyleParameter : CellBuilderImplFacts
        {
            [Fact]
            public void SetsStyleKeyProperty()
            {
                // Arrange
                var style = BuiltInExcelStyle.Normal;
                var sut = CreateSystemUnderTest();

                // Act
                sut.WithStyle(style);

                // Assert
                Assert.NotNull(sut.StyleKey);
                Assert.Equal(style.Name, sut.StyleKey!.Name);
                Assert.Equal(IndexerKeyKind.Excel, sut.StyleKey.Kind);
            }

            [Fact]
            public void ReturnsSelf()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                var cellValueBuilder = sut.WithStyle(BuiltInExcelStyle.Normal);

                // Assert
                Assert.Equal(sut, cellValueBuilder);
            }
        }

        public class TheWithStyleMethodWithBuiltInPackageStyleParameter : CellBuilderImplFacts
        {
            [Fact]
            public void SetsStyleKeyProperty()
            {
                // Arrange
                var style = BuiltInPackageStyle.Bold;
                var sut = CreateSystemUnderTest();

                // Act
                sut.WithStyle(style);

                // Assert
                Assert.NotNull(sut.StyleKey);
                Assert.Equal(style.Name, sut.StyleKey!.Name);
                Assert.Equal(IndexerKeyKind.Package, sut.StyleKey.Kind);
            }

            [Fact]
            public void ReturnsSelf()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                var cellValueBuilder = sut.WithStyle(BuiltInPackageStyle.Bold);

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
