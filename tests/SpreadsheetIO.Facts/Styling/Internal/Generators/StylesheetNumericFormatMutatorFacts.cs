using System;
using LanceC.SpreadsheetIO.Styling;
using LanceC.SpreadsheetIO.Styling.Internal.Generators;
using LanceC.SpreadsheetIO.Styling.Internal.Indexers;
using Moq.AutoMock;
using Xunit;
using OpenXml = DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Facts.Styling.Internal.Generators
{
    public class StylesheetNumericFormatMutatorFacts
    {
        private readonly AutoMocker _mocker = new AutoMocker();

        private StylesheetNumericFormatMutator CreateSystemUnderTest()
            => _mocker.CreateInstance<StylesheetNumericFormatMutator>();

        public class TheMutateMethod : StylesheetNumericFormatMutatorFacts
        {
            [Fact]
            public void ModifiesSpreadsheetNumberingFormatsWithIndexedNumericFormats()
            {
                // Arrange
                var stylesheet = new OpenXml.Stylesheet();

                var firstNumericFormatId = 164U;
                var firstNumericFormat = new NumericFormat("#,##0.0000");
                _mocker.GetMock<INumericFormatIndexer>()
                    .SetupGet(numericFormatIndexer => numericFormatIndexer[firstNumericFormat])
                    .Returns(firstNumericFormatId);

                var secondNumericFormatId = 165U;
                var secondNumericFormat = new NumericFormat(@"#,##0.0000;\(#,##0.0000\)");
                _mocker.GetMock<INumericFormatIndexer>()
                    .SetupGet(numericFormatIndexer => numericFormatIndexer[secondNumericFormat])
                    .Returns(secondNumericFormatId);

                _mocker.GetMock<INumericFormatIndexer>()
                    .SetupGet(numericFormatIndexer => numericFormatIndexer.Resources)
                    .Returns(new[] { firstNumericFormat, secondNumericFormat, });

                _mocker.GetMock<INumericFormatIndexer>()
                    .SetupGet(numericFormatIndexer => numericFormatIndexer.NonBuiltInCount)
                    .Returns(2U);

                var sut = CreateSystemUnderTest();

                // Act
                sut.Mutate(stylesheet);

                // Assert
                Assert.NotNull(stylesheet.NumberingFormats);
                Assert.Equal(2U, stylesheet.NumberingFormats.Count.Value);
                Assert.Equal(2, stylesheet.NumberingFormats.ChildElements.Count);

                var firstNumberingFormat = Assert.IsType<OpenXml.NumberingFormat>(stylesheet.NumberingFormats.ChildElements[0]);
                Assert.Equal(firstNumericFormatId, firstNumberingFormat.NumberFormatId.Value);
                Assert.Equal(firstNumericFormat.Code, firstNumberingFormat.FormatCode.Value);

                var secondNumberingFormat = Assert.IsType<OpenXml.NumberingFormat>(stylesheet.NumberingFormats.ChildElements[1]);
                Assert.Equal(secondNumericFormatId, secondNumberingFormat.NumberFormatId.Value);
                Assert.Equal(secondNumericFormat.Code, secondNumberingFormat.FormatCode.Value);
            }

            [Fact]
            public void CreatesEmptySpreadsheetNumberingFormatsWhenNoNumericFormatsAreIndexed()
            {
                // Arrange
                var stylesheet = new OpenXml.Stylesheet();

                _mocker.GetMock<INumericFormatIndexer>()
                    .SetupGet(numericFormatIndexer => numericFormatIndexer.Resources)
                    .Returns(Array.Empty<NumericFormat>());

                var sut = CreateSystemUnderTest();

                // Act
                sut.Mutate(stylesheet);

                // Assert
                Assert.NotNull(stylesheet.NumberingFormats);
                Assert.Equal(0U, stylesheet.NumberingFormats.Count.Value);
                Assert.Equal(0, stylesheet.NumberingFormats.ChildElements.Count);
            }

            [Fact]
            public void DoesNotAddBuiltInNumberingFormat()
            {
                // Arrange
                var stylesheet = new OpenXml.Stylesheet();

                var numericFormat = new NumericFormat("@");
                _mocker.GetMock<INumericFormatIndexer>()
                    .SetupGet(numericFormatIndexer => numericFormatIndexer[numericFormat])
                    .Returns(49U);

                _mocker.GetMock<INumericFormatIndexer>()
                    .SetupGet(numericFormatIndexer => numericFormatIndexer.Resources)
                    .Returns(new[] { numericFormat, });

                var sut = CreateSystemUnderTest();

                // Act
                sut.Mutate(stylesheet);

                // Assert
                Assert.NotNull(stylesheet.NumberingFormats);
                Assert.Equal(0U, stylesheet.NumberingFormats.Count.Value);
                Assert.Equal(0, stylesheet.NumberingFormats.ChildElements.Count);
            }
        }
    }
}
