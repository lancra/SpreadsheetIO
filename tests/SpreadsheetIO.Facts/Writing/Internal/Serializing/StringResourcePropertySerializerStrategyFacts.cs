using LanceC.SpreadsheetIO.Facts.Testing.Assertions;
using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Writing;
using LanceC.SpreadsheetIO.Writing.Internal.Serializing;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Writing.Internal.Serializing;

public class StringResourcePropertySerializerStrategyFacts
{
    private readonly AutoMocker _mocker = new();

    private StringResourcePropertySerializerStrategy CreateSystemUnderTest()
        => _mocker.CreateInstance<StringResourcePropertySerializerStrategy>();

    public class ThePropertyTypesProperty : StringResourcePropertySerializerStrategyFacts
    {
        [Fact]
        public void ReturnsDecimalType()
        {
            // Arrange
            var sut = CreateSystemUnderTest();

            // Act
            var propertyTypes = sut.PropertyTypes;

            // Assert
            var propertyType = Assert.Single(propertyTypes);
            Assert.Equal(typeof(string), propertyType);
        }
    }

    public class TheSerializeMethod : StringResourcePropertySerializerStrategyFacts
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("foo")]
        public void ReturnsStringWritingCellValue(string value)
        {
            // Arrange
            var expectedCellValue = new WritingCellValue(value);
            var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.String);

            var sut = CreateSystemUnderTest();

            // Act
            var actualCellValue = sut.Serialize(value, map);

            // Assert
            AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
        }
    }
}
