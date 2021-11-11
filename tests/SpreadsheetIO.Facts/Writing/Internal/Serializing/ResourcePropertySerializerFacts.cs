using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping.Extensions;
using LanceC.SpreadsheetIO.Writing;
using LanceC.SpreadsheetIO.Writing.Internal.Serializing;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Writing.Internal.Serializing
{
    public class ResourcePropertySerializerFacts
    {
        private readonly AutoMocker _mocker = new();

        private static Mock<IResourcePropertySerializerStrategy> MockStrategy(params Type[] propertyTypes)
        {
            var strategyMock = new Mock<IResourcePropertySerializerStrategy>();
            strategyMock.SetupGet(strategy => strategy.PropertyTypes)
                .Returns(propertyTypes);

            return strategyMock;
        }

        private static Mock<IDefaultResourcePropertySerializerStrategy> MockDefaultStrategy(params Type[] propertyTypes)
        {
            var strategyMock = new Mock<IDefaultResourcePropertySerializerStrategy>();
            strategyMock.SetupGet(strategy => strategy.PropertyTypes)
                .Returns(propertyTypes);

            return strategyMock;
        }

        private ResourcePropertySerializer CreateSystemUnderTest()
            => _mocker.CreateInstance<ResourcePropertySerializer>();

        public class TheConstructor : ResourcePropertySerializerFacts
        {
            [Fact]
            public void DoesNotThrowExceptionWhenThereAreNoDuplicateStrategiesForAnyPropertyType()
            {
                // Arrange
                var stringStrategyMock = MockStrategy(typeof(string));
                var integersStrategyMock = MockStrategy(typeof(int), typeof(uint));
                var decimalStrategyMock = MockStrategy(typeof(decimal));

                _mocker.Use<IEnumerable<IResourcePropertySerializerStrategy>>(
                    new[]
                    {
                        stringStrategyMock.Object,
                        integersStrategyMock.Object,
                        decimalStrategyMock.Object,
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
                var defaultIntegersStrategyMock = MockDefaultStrategy(typeof(int), typeof(uint));
                var integersStrategyMock = MockStrategy(typeof(int), typeof(uint));

                _mocker.Use<IEnumerable<IResourcePropertySerializerStrategy>>(
                    new[]
                    {
                        defaultIntegersStrategyMock.Object,
                        integersStrategyMock.Object,
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

                _mocker.Use<IEnumerable<IResourcePropertySerializerStrategy>>(
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

        public class TheSerializeMethod : ResourcePropertySerializerFacts
        {
            [Fact]
            public void ReturnsCellValueWithResultFromMatchingStrategy()
            {
                // Arrange
                var name = "foo";
                var resource = new FakeModel { Name = name, };
                var map = PropertyMapCreator.CreateForFakeModel(model => model.Name);

                var expectedCellValue = new WritingCellValue(name);

                var integersStrategyMock = MockStrategy(typeof(int), typeof(uint));

                var stringStrategyMock = MockStrategy(typeof(string));
                stringStrategyMock.Setup(strategy => strategy.Serialize(name, map))
                    .Returns(expectedCellValue);

                _mocker.Use<IEnumerable<IResourcePropertySerializerStrategy>>(
                    new[]
                    {
                        integersStrategyMock.Object,
                        stringStrategyMock.Object,
                    });

                var sut = CreateSystemUnderTest();

                // Act
                var actualCellValue = sut.Serialize(resource, map);

                // Assert
                Assert.Equal(expectedCellValue, actualCellValue);
            }

            [Fact]
            public void UsesUnderlyingTypeForNullablePropertyTypeWhenResolvingStrategy()
            {
                // Arrange
                var dateTime = new DateTime(2012, 12, 21);
                var resource = new FakeModel { DateTime = dateTime, };
                var map = PropertyMapCreator.CreateForFakeModel(model => model.DateTime);

                var expectedCellValue = new WritingCellValue(dateTime);

                var nullableDateTimeStrategyMock = MockStrategy(typeof(DateTime?));

                var dateTimeStrategyMock = MockStrategy(typeof(DateTime));
                dateTimeStrategyMock.Setup(strategy => strategy.Serialize(dateTime, map))
                    .Returns(expectedCellValue);

                _mocker.Use<IEnumerable<IResourcePropertySerializerStrategy>>(
                    new[]
                    {
                        nullableDateTimeStrategyMock.Object,
                        dateTimeStrategyMock.Object,
                    });

                var sut = CreateSystemUnderTest();

                // Act
                var actualCellValue = sut.Serialize(resource, map);

                // Assert
                Assert.Equal(expectedCellValue, actualCellValue);
            }

            [Fact]
            public void UsesDefaultValueWhenResourcePropertyIsNull()
            {
                // Arrange
                var defaultName = "bar";

                var resource = new FakeModel { Name = null, };
                var map = PropertyMapCreator
                    .CreateForFakeModel(model => model.Name, new DefaultValuePropertyMapOptionsExtension(defaultName));

                var expectedCellValue = new WritingCellValue(defaultName);

                var integersStrategyMock = MockStrategy(typeof(int), typeof(uint));

                var stringStrategyMock = MockStrategy(typeof(string));
                stringStrategyMock.Setup(strategy => strategy.Serialize(defaultName, map))
                    .Returns(expectedCellValue);

                _mocker.Use<IEnumerable<IResourcePropertySerializerStrategy>>(
                    new[]
                    {
                        integersStrategyMock.Object,
                        stringStrategyMock.Object,
                    });

                var sut = CreateSystemUnderTest();

                // Act
                var actualCellValue = sut.Serialize(resource, map);

                // Assert
                Assert.Equal(expectedCellValue, actualCellValue);
            }

            [Fact]
            public void CallsStrategyWithNullWhenPropertyIsNullAndHasNoDefaultSpecified()
            {
                // Arrange
                var resource = new FakeModel { Name = null, };
                var map = PropertyMapCreator.CreateForFakeModel(model => model.Name);

                var expectedCellValue = new WritingCellValue();

                var integersStrategyMock = MockStrategy(typeof(int), typeof(uint));

                var stringStrategyMock = MockStrategy(typeof(string));
                stringStrategyMock.Setup(strategy => strategy.Serialize(null, map))
                    .Returns(expectedCellValue);

                _mocker.Use<IEnumerable<IResourcePropertySerializerStrategy>>(
                    new[]
                    {
                        integersStrategyMock.Object,
                        stringStrategyMock.Object,
                    });

                var sut = CreateSystemUnderTest();

                // Act
                var actualCellValue = sut.Serialize(resource, map);

                // Assert
                Assert.Equal(expectedCellValue, actualCellValue);
            }

            [Fact]
            public void ThrowsInvalidOperationExceptionWhenNoMatchingStrategyIsFound()
            {
                // Arrange
                var name = "foo";
                var resource = new FakeModel { Name = name, };
                var map = PropertyMapCreator.CreateForFakeModel(model => model.Name);

                var expectedCellValue = new WritingCellValue(name);

                var integersStrategyMock = MockStrategy(typeof(int), typeof(uint));
                _mocker.Use<IEnumerable<IResourcePropertySerializerStrategy>>(
                    new[]
                    {
                        integersStrategyMock.Object,
                    });

                var sut = CreateSystemUnderTest();

                // Act
                var exception = Record.Exception(() => sut.Serialize(resource, map));

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<InvalidOperationException>(exception);
            }
        }
    }
}
