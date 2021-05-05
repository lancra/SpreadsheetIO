using System;
using LanceC.SpreadsheetIO.Facts.Testing.Assertions;
using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Writing;
using LanceC.SpreadsheetIO.Writing.Internal.Serializing;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Writing.Internal.Serializing
{
    public class CharacterResourcePropertySerializerStrategyFacts
    {
        private readonly AutoMocker _mocker = new AutoMocker();

        private CharacterResourcePropertySerializerStrategy CreateSystemUnderTest()
            => _mocker.CreateInstance<CharacterResourcePropertySerializerStrategy>();

        public class TheSerializeMethod : CharacterResourcePropertySerializerStrategyFacts
        {
            [Theory]
            [InlineData('\0')]
            [InlineData('A')]
            public void ReturnsCharacterWritingCellValue(char value)
            {
                // Arrange
                var expectedCellValue = new WritingCellValue(value);
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.Character);

                var sut = CreateSystemUnderTest();

                // Act
                var actualCellValue = sut.Serialize(value, map);

                // Assert
                AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
            }

            [Theory]
            [InlineData(null)]
            [InlineData('\0')]
            [InlineData('A')]
            public void ReturnsNullableCharacterWritingCellValue(char? value)
            {
                // Arrange
                var expectedCellValue = new WritingCellValue(value);
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.CharacterNullable);

                var sut = CreateSystemUnderTest();

                // Act
                var actualCellValue = sut.Serialize(value, map);

                // Assert
                AssertWritingCellValues.Equal(expectedCellValue, actualCellValue);
            }

            [Fact]
            public void ThrowsInvalidCastExceptionWhenNonCharacterTypeIsProvided()
            {
                // Arrange
                var map = PropertyMapCreator.CreateForFakeResourcePropertyStrategyModel(model => model.Character);
                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut.Serialize("foo", map));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<InvalidCastException>(exception);
            }
        }
    }
}