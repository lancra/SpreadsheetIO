using System.Collections.Generic;
using DocumentFormat.OpenXml.Spreadsheet;
using LanceC.SpreadsheetIO.Styling.Internal.Generators;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Styling.Internal.Generators
{
    public class StylesheetGeneratorFacts
    {
        private readonly AutoMocker _mocker = new AutoMocker();

        private StylesheetGenerator CreateSystemUnderTest()
            => _mocker.CreateInstance<StylesheetGenerator>();

        public class TheGenerateMethod : StylesheetGeneratorFacts
        {
            [Fact]
            public void ReturnsStylesheet()
            {
                // Arrange
                var sut = CreateSystemUnderTest();

                // Act
                var stylesheet = sut.Generate();

                // Assert
                Assert.NotNull(stylesheet);
            }

            [Fact]
            public void ExecutesStylesheetMutators()
            {
                // Arrange
                var firstStylesheetMutator = new Mock<IStylesheetMutator>();
                firstStylesheetMutator.Setup(stylesheetMutator => stylesheetMutator.Mutate(It.IsAny<Stylesheet>()))
                    .Verifiable();

                var secondStylesheetMutator = new Mock<IStylesheetMutator>();
                secondStylesheetMutator.Setup(stylesheetMutator => stylesheetMutator.Mutate(It.IsAny<Stylesheet>()))
                    .Verifiable();

                _mocker.Use<IEnumerable<IStylesheetMutator>>(new[] { firstStylesheetMutator.Object, secondStylesheetMutator.Object, });

                var sut = CreateSystemUnderTest();

                // Act
                sut.Generate();

                // Assert
                firstStylesheetMutator.Verify();
                secondStylesheetMutator.Verify();
            }
        }
    }
}
