using System;
using System.Collections.Generic;
using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Reading.Internal.Parsing;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal.Parsing
{
    public class ResourcePropertyParserFacts
    {
        private readonly AutoMocker _mocker = new();

        private static Mock<IResourcePropertyParserStrategy> MockStrategy(Type propertyType)
        {
            var strategyMock = new Mock<IResourcePropertyParserStrategy>();
            strategyMock.SetupGet(strategy => strategy.PropertyType)
                .Returns(propertyType);

            return strategyMock;
        }

        private static Mock<IDefaultResourcePropertyParserStrategy> MockDefaultStrategy(Type propertyType)
        {
            var strategyMock = new Mock<IDefaultResourcePropertyParserStrategy>();
            strategyMock.SetupGet(strategy => strategy.PropertyType)
                .Returns(propertyType);

            return strategyMock;
        }

        private ResourcePropertyParser CreateSystemUnderTest()
            => _mocker.CreateInstance<ResourcePropertyParser>();

        public class TheConstructor : ResourcePropertyParserFacts
        {
            [Fact]
            public void DoesNotThrowExceptionWhenThereAreNoDuplicateStrategiesForAnyPropertyType()
            {
                // Arrange
                var stringStrategyMock = MockStrategy(typeof(string));
                var integerStrategy = MockStrategy(typeof(int));
                var decimalStrategy = MockStrategy(typeof(decimal));

                _mocker.Use<IEnumerable<IResourcePropertyParserStrategy>>(
                    new[]
                    {
                        stringStrategyMock.Object,
                        integerStrategy.Object,
                        decimalStrategy.Object,
                    });

                // Act
                var exception = Record.Exception(() => CreateSystemUnderTest());

                // Assert
                Assert.Null(exception);
            }

            [Fact]
            public void DoesNotThrowExceptionWhenDuplicateStrategyIsDefault()
            {
                // Arrange
                var defaultStringStrategyMock = MockDefaultStrategy(typeof(string));
                var stringStrategyMock = MockStrategy(typeof(string));

                _mocker.Use<IEnumerable<IResourcePropertyParserStrategy>>(
                    new[]
                    {
                        defaultStringStrategyMock.Object,
                        stringStrategyMock.Object,
                    });

                // Act
                var exception = Record.Exception(() => CreateSystemUnderTest());

                // Assert
                Assert.Null(exception);
            }

            [Fact]
            public void ThrowsInvalidOperationExceptionWhenTwoNonDefaultStrategiesAreDefinedForTheSamePropertyType()
            {
                // Arrange
                var firstStringStrategyMock = MockStrategy(typeof(string));
                var secondStringStrategyMock = MockStrategy(typeof(string));

                _mocker.Use<IEnumerable<IResourcePropertyParserStrategy>>(
                    new[]
                    {
                        firstStringStrategyMock.Object,
                        secondStringStrategyMock.Object,
                    });

                // Act
                var exception = Record.Exception(() => CreateSystemUnderTest());

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<InvalidOperationException>(exception);
            }
        }

        public class TheTryParseMethod : ResourcePropertyParserFacts
        {
            [Fact]
            public void ReturnsResultFromMatchingStrategy()
            {
                // Arrange
                var cellValue = "foo";
                var map = PropertyMapCreator.CreateForFakeModel(model => model.Name);

                var expectedResult = ResourcePropertyParseResultKind.Success;
                object? expectedValue = "bar";

                var integerStrategyMock = MockStrategy(typeof(int));

                var stringStrategyMock = MockStrategy(typeof(string));
                stringStrategyMock.Setup(strategy => strategy.TryParse(cellValue, map, out expectedValue))
                    .Returns(expectedResult);

                _mocker.Use<IEnumerable<IResourcePropertyParserStrategy>>(
                    new[]
                    {
                        integerStrategyMock.Object,
                        stringStrategyMock.Object,
                    });

                var sut = CreateSystemUnderTest();

                // Act
                var actualResult = sut.TryParse(cellValue, map, out var actualValue);

                // Assert
                Assert.Equal(expectedResult, actualResult);
                Assert.Equal(expectedValue, actualValue);
            }

            [Fact]
            public void UsesUnderlyingTypeForNullablePropertyTypeWhenResolvingStrategy()
            {
                // Arrange
                var cellValue = "foo";
                var map = PropertyMapCreator.CreateForFakeModel(model => model.DateTime);

                var expectedResult = ResourcePropertyParseResultKind.Success;
                object? expectedValue = new DateTime(2021, 1, 1);

                var nullableDateTimeStrategyMock = MockStrategy(typeof(DateTime?));

                var dateTimeStrategyMock = MockStrategy(typeof(DateTime));
                dateTimeStrategyMock.Setup(strategy => strategy.TryParse(cellValue, map, out expectedValue))
                    .Returns(expectedResult);

                _mocker.Use<IEnumerable<IResourcePropertyParserStrategy>>(
                    new[]
                    {
                        nullableDateTimeStrategyMock.Object,
                        dateTimeStrategyMock.Object,
                    });

                var sut = CreateSystemUnderTest();

                // Act
                var actualResult = sut.TryParse(cellValue, map, out var actualValue);

                // Assert
                Assert.Equal(expectedResult, actualResult);
                Assert.Equal(expectedValue, actualValue);

                nullableDateTimeStrategyMock
                    .Verify(
                        strategy => strategy.TryParse(
                            It.IsAny<string>(),
                            It.IsAny<PropertyMap<FakeModel>>(),
                            out expectedValue),
                        Times.Never);
            }

            [Fact]
            public void ThrowsInvalidOperationExceptionWhenNoMatchingStrategyIsFound()
            {
                // Arrange
                var cellValue = "foo";
                var map = PropertyMapCreator.CreateForFakeModel(model => model.Name);

                var integerStrategyMock = MockStrategy(typeof(int));
                _mocker.Use<IEnumerable<IResourcePropertyParserStrategy>>(new[] { integerStrategyMock.Object, });

                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut.TryParse(cellValue, map, out var value));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<InvalidOperationException>(exception);
            }
        }
    }
}
