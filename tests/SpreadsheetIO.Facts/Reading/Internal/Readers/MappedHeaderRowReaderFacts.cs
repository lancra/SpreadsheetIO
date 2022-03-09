using LanceC.SpreadsheetIO.Facts.Testing.Creators;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes.Models;
using LanceC.SpreadsheetIO.Mapping2;
using LanceC.SpreadsheetIO.Mapping2.Options;
using LanceC.SpreadsheetIO.Reading.Internal.Properties;
using LanceC.SpreadsheetIO.Reading.Internal.Readers;
using LanceC.SpreadsheetIO.Shared.Internal;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal.Readers;

public class MappedHeaderRowReaderFacts
{
    private readonly AutoMocker _mocker = new();

    private MappedHeaderRowReader CreateSystemUnderTest()
        => _mocker.CreateInstance<MappedHeaderRowReader>();

    private void MockWorksheetElementReaderCells(params FakeWorksheetCell[] cells)
    {
        var readerMock = _mocker.GetMock<IWorksheetElementReader>();
        var readNextSequence = readerMock.SetupSequence(reader => reader.ReadNextCell());
        var getLocationSequence = readerMock.SetupSequence(reader => reader.GetCellLocation());
        var getValueSequence = readerMock.SetupSequence(reader => reader.GetCellValue());

        foreach (var cell in cells)
        {
            readNextSequence.Returns(true);
            getLocationSequence.Returns(cell.Location);
            getValueSequence.Returns(cell.Value);
        }

        readNextSequence.Returns(false);
    }

    public class TheReadMethod : MappedHeaderRowReaderFacts
    {
        public static TheoryData<PropertyElementKind>
            DataForDoesNotGenerateMissingFailureForPropertyMapWithOptionalHeaderOptionsExtension
            => new()
            {
                { PropertyElementKind.All },
                { PropertyElementKind.Header },
            };

        [Fact]
        public void ReturnsReadingResultWithoutFailuresWhenAllRequiredPropertyMapsAreFound()
        {
            // Arrange
            var readerMock = _mocker.GetMock<IWorksheetElementReader>();
            readerMock.Setup(reader => reader.ReadToRow(1U))
                .Returns(true);

            MockWorksheetElementReaderCells(
                new FakeWorksheetCell(new CellLocation("A1"), nameof(FakeModel.Id)),
                new FakeWorksheetCell(new CellLocation("B1"), nameof(FakeModel.Name)),
                new FakeWorksheetCell(new CellLocation("C1"), nameof(FakeModel.Decimal)));

            var map = ResourceMapCreator.Create<FakeModel>(
                new[]
                {
                    PropertyMapCreator2.CreateForFakeModel(model => model.Id),
                    PropertyMapCreator2.CreateForFakeModel(model => model.Name),
                    PropertyMapCreator2.CreateForFakeModel(model => model.Decimal),
                });

            var idPropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Id));
            var namePropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Name));
            var decimalPropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Decimal));

            var resourcePropertyHeadersMock = _mocker.GetMock<IResourcePropertyHeaders>();
            resourcePropertyHeadersMock
                .Setup(resourcePropertyHeaders => resourcePropertyHeaders.ContainsMap(It.IsAny<PropertyMap>()))
                .Returns(true);

            _mocker.GetMock<IResourcePropertyCollectionFactory>()
                .Setup(resourcePropertyCollectionFactory => resourcePropertyCollectionFactory.CreateHeaders())
                .Returns(resourcePropertyHeadersMock.Object);

            var sut = CreateSystemUnderTest();

            // Act
            var readingResult = sut.Read<FakeModel>(readerMock.Object, map);

            // Assert
            resourcePropertyHeadersMock.Verify(resourcePropertyHeaders => resourcePropertyHeaders.Add(idPropertyMap, 1U));
            resourcePropertyHeadersMock.Verify(resourcePropertyHeaders => resourcePropertyHeaders.Add(namePropertyMap, 2U));
            resourcePropertyHeadersMock.Verify(resourcePropertyHeaders => resourcePropertyHeaders.Add(decimalPropertyMap, 3U));

            Assert.Null(readingResult.Failure);
        }

        [Fact]
        public void ReturnsReadingResultWithMissingFailureWhenRequiredPropertyMapNotFound()
        {
            // Arrange
            var readerMock = _mocker.GetMock<IWorksheetElementReader>();
            readerMock.Setup(reader => reader.ReadToRow(1U))
                .Returns(true);

            MockWorksheetElementReaderCells(new FakeWorksheetCell(new CellLocation("A1"), nameof(FakeModel.Id)));

            var map = ResourceMapCreator.Create<FakeModel>(
                new[]
                {
                    PropertyMapCreator2.CreateForFakeModel(model => model.Id),
                    PropertyMapCreator2.CreateForFakeModel(model => model.Name),
                });

            var idPropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Id));
            var namePropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Name));

            var resourcePropertyHeadersMock = _mocker.GetMock<IResourcePropertyHeaders>();
            resourcePropertyHeadersMock.Setup(resourcePropertyHeaders => resourcePropertyHeaders.ContainsMap(idPropertyMap))
                .Returns(true);
            resourcePropertyHeadersMock.Setup(resourcePropertyHeaders => resourcePropertyHeaders.ContainsMap(namePropertyMap))
                .Returns(false);

            _mocker.GetMock<IResourcePropertyCollectionFactory>()
                .Setup(resourcePropertyCollectionFactory => resourcePropertyCollectionFactory.CreateHeaders())
                .Returns(resourcePropertyHeadersMock.Object);

            var sut = CreateSystemUnderTest();

            // Act
            var readingResult = sut.Read<FakeModel>(readerMock.Object, map);

            // Assert
            resourcePropertyHeadersMock.Verify(resourcePropertyHeaders => resourcePropertyHeaders.Add(idPropertyMap, 1U));

            Assert.NotNull(readingResult.Failure);
            var missingFailure = Assert.Single(readingResult.Failure!.MissingHeaders);
            Assert.Equal(nameof(FakeModel.Name), missingFailure.MapKey.Name);
        }

        [Fact]
        public void ReturnsReadingResultWithInvalidFailureWhenMultipleConflictingPropertyMapsFoundForSameColumn()
        {
            // Arrange
            var readerMock = _mocker.GetMock<IWorksheetElementReader>();
            readerMock.Setup(reader => reader.ReadToRow(1U))
                .Returns(true);

            MockWorksheetElementReaderCells(
                new FakeWorksheetCell(new CellLocation("A1"), nameof(FakeModel.Id)),
                new FakeWorksheetCell(new CellLocation("B1"), nameof(FakeModel.Name)));

            var map = ResourceMapCreator.Create<FakeModel>(
                new[]
                {
                    PropertyMapCreator2.CreateForFakeModel(model => model.Id),
                    PropertyMapCreator2.CreateForFakeModel(model => model.Name),
                    PropertyMapCreator2.CreateForFakeModel(model => model.Decimal, keyAction: key => key.WithoutName().WithNumber(2U)),
                });

            var idPropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Id));
            var namePropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Name));
            var decimalPropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Decimal));

            var resourcePropertyHeadersMock = _mocker.GetMock<IResourcePropertyHeaders>();
            resourcePropertyHeadersMock.Setup(resourcePropertyHeaders => resourcePropertyHeaders.ContainsMap(idPropertyMap))
                .Returns(true);
            resourcePropertyHeadersMock.Setup(resourcePropertyHeaders => resourcePropertyHeaders.ContainsMap(namePropertyMap))
                .Returns(false);
            resourcePropertyHeadersMock.Setup(resourcePropertyHeaders => resourcePropertyHeaders.ContainsMap(decimalPropertyMap))
                .Returns(false);

            _mocker.GetMock<IResourcePropertyCollectionFactory>()
                .Setup(resourcePropertyCollectionFactory => resourcePropertyCollectionFactory.CreateHeaders())
                .Returns(resourcePropertyHeadersMock.Object);

            var sut = CreateSystemUnderTest();

            // Act
            var readingResult = sut.Read<FakeModel>(readerMock.Object, map);

            // Assert
            resourcePropertyHeadersMock.Verify(resourcePropertyHeaders => resourcePropertyHeaders.Add(idPropertyMap, 1U));

            Assert.NotNull(readingResult.Failure);
            var invalidFailure = Assert.Single(readingResult.Failure!.InvalidHeaders);
            Assert.Equal(nameof(FakeModel.Name), invalidFailure.NameMapKey.Name);
            Assert.Equal(default, invalidFailure.NameMapKey.Number);
            Assert.Equal(nameof(FakeModel.Decimal), invalidFailure.NumberMapKey.Name);
            Assert.Equal(2U, invalidFailure.NumberMapKey.Number);
        }

        [Fact]
        public void ReturnsReadingResultWithMissingHeaderRowFailureWhenSpecifiedRowNotFound()
        {
            // Arrange
            var readerMock = _mocker.GetMock<IWorksheetElementReader>();
            readerMock.Setup(reader => reader.ReadToRow(1U))
                .Returns(false);

            var map = ResourceMapCreator.Create<FakeModel>(
                new[]
                {
                    PropertyMapCreator2.CreateForFakeModel(model => model.Id),
                    PropertyMapCreator2.CreateForFakeModel(model => model.Name),
                });

            var resourcePropertyHeadersMock = _mocker.GetMock<IResourcePropertyHeaders>();

            _mocker.GetMock<IResourcePropertyCollectionFactory>()
                .Setup(resourcePropertyCollectionFactory => resourcePropertyCollectionFactory.CreateHeaders())
                .Returns(resourcePropertyHeadersMock.Object);

            var sut = CreateSystemUnderTest();

            // Act
            var readingResult = sut.Read<FakeModel>(readerMock.Object, map);

            // Assert
            resourcePropertyHeadersMock
                .Verify(
                    resourcePropertyHeaders => resourcePropertyHeaders.Add(It.IsAny<PropertyMap>(), It.IsAny<uint>()),
                    Times.Never);

            Assert.NotNull(readingResult.Failure);
            Assert.True(readingResult.Failure!.MissingHeaderRow);
        }

        [Fact]
        public void UsesOverriddenHeaderRowNumberWhenOptionsExtensionIsDefined()
        {
            // Arrange
            var headerRowNumber = 2U;

            var readerMock = _mocker.GetMock<IWorksheetElementReader>();
            readerMock.Setup(reader => reader.ReadToRow(headerRowNumber))
                .Returns(true);

            MockWorksheetElementReaderCells(new FakeWorksheetCell(new CellLocation("A1"), nameof(FakeModel.Id)));

            var map = ResourceMapCreator.Create<FakeModel>(
                new[]
                {
                    PropertyMapCreator2.CreateForFakeModel(model => model.Id),
                },
                new HeaderRowNumberResourceMapOption(headerRowNumber));

            var idPropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Id));

            var resourcePropertyHeadersMock = _mocker.GetMock<IResourcePropertyHeaders>();
            resourcePropertyHeadersMock.Setup(resourcePropertyHeaders => resourcePropertyHeaders.ContainsMap(idPropertyMap))
                .Returns(true);

            _mocker.GetMock<IResourcePropertyCollectionFactory>()
                .Setup(resourcePropertyCollectionFactory => resourcePropertyCollectionFactory.CreateHeaders())
                .Returns(resourcePropertyHeadersMock.Object);

            var sut = CreateSystemUnderTest();

            // Act
            var readingResult = sut.Read<FakeModel>(readerMock.Object, map);

            // Assert
            resourcePropertyHeadersMock.Verify(resourcePropertyHeaders => resourcePropertyHeaders.Add(idPropertyMap, 1U));

            Assert.Null(readingResult.Failure);
        }

        [Theory]
        [MemberData(nameof(DataForDoesNotGenerateMissingFailureForPropertyMapWithOptionalHeaderOptionsExtension))]
        public void DoesNotGenerateMissingFailureForPropertyMapWithOptionalHeaderOptionsExtension(PropertyElementKind kind)
        {
            // Arrange
            var readerMock = _mocker.GetMock<IWorksheetElementReader>();
            readerMock.Setup(reader => reader.ReadToRow(1U))
                .Returns(true);

            MockWorksheetElementReaderCells(new FakeWorksheetCell(new CellLocation("A1"), nameof(FakeModel.Id)));

            var map = ResourceMapCreator.Create<FakeModel>(
                new[]
                {
                    PropertyMapCreator2.CreateForFakeModel(model => model.Id),
                    PropertyMapCreator2.CreateForFakeModel(model => model.Name, options: new OptionalPropertyMapOption(kind)),
                });

            var idPropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Id));
            var namePropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Name));

            var resourcePropertyHeadersMock = _mocker.GetMock<IResourcePropertyHeaders>();
            resourcePropertyHeadersMock.Setup(resourcePropertyHeaders => resourcePropertyHeaders.ContainsMap(idPropertyMap))
                .Returns(true);
            resourcePropertyHeadersMock.Setup(resourcePropertyHeaders => resourcePropertyHeaders.ContainsMap(namePropertyMap))
                .Returns(false);

            _mocker.GetMock<IResourcePropertyCollectionFactory>()
                .Setup(resourcePropertyCollectionFactory => resourcePropertyCollectionFactory.CreateHeaders())
                .Returns(resourcePropertyHeadersMock.Object);

            var sut = CreateSystemUnderTest();

            // Act
            var readingResult = sut.Read<FakeModel>(readerMock.Object, map);

            // Assert
            resourcePropertyHeadersMock.Verify(resourcePropertyHeaders => resourcePropertyHeaders.Add(idPropertyMap, 1U));

            Assert.Null(readingResult.Failure);
        }

        [Fact]
        public void SkipsUnmappedCell()
        {
            // Arrange
            var readerMock = _mocker.GetMock<IWorksheetElementReader>();
            readerMock.Setup(reader => reader.ReadToRow(1U))
                .Returns(true);

            MockWorksheetElementReaderCells(
                new FakeWorksheetCell(new CellLocation("A1"), nameof(FakeModel.Id)),
                new FakeWorksheetCell(new CellLocation("B1"), "foo"));

            var map = ResourceMapCreator.Create<FakeModel>(
                new[]
                {
                    PropertyMapCreator2.CreateForFakeModel(model => model.Id),
                });

            var idPropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Id));

            var resourcePropertyHeadersMock = _mocker.GetMock<IResourcePropertyHeaders>();
            resourcePropertyHeadersMock.Setup(resourcePropertyHeaders => resourcePropertyHeaders.ContainsMap(idPropertyMap))
                .Returns(true);

            _mocker.GetMock<IResourcePropertyCollectionFactory>()
                .Setup(resourcePropertyCollectionFactory => resourcePropertyCollectionFactory.CreateHeaders())
                .Returns(resourcePropertyHeadersMock.Object);

            var sut = CreateSystemUnderTest();

            // Act
            var readingResult = sut.Read<FakeModel>(readerMock.Object, map);

            // Assert
            resourcePropertyHeadersMock.Verify(resourcePropertyHeaders => resourcePropertyHeaders.Add(idPropertyMap, 1U));

            Assert.Null(readingResult.Failure);
        }
    }
}
