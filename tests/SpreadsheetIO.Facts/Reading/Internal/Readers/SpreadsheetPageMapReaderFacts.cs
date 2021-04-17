using System.Linq;
using LanceC.SpreadsheetIO.Facts.Testing.Fakes;
using LanceC.SpreadsheetIO.Mapping;
using LanceC.SpreadsheetIO.Reading;
using LanceC.SpreadsheetIO.Reading.Internal;
using LanceC.SpreadsheetIO.Reading.Internal.Properties;
using LanceC.SpreadsheetIO.Reading.Internal.Readers;
using LanceC.SpreadsheetIO.Shared.Internal;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace LanceC.SpreadsheetIO.Facts.Reading.Internal.Readers
{
    public class SpreadsheetPageMapReaderFacts
    {
        private readonly AutoMocker _mocker = new AutoMocker();

        private SpreadsheetPageMapReader CreateSystemUnderTest()
            => _mocker.CreateInstance<SpreadsheetPageMapReader>();

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

        public class TheReadHeaderRowMethod : SpreadsheetPageMapReaderFacts
        {
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

                var map = new FakeModelMap()
                    .Map(model => model.Id)
                    .Map(model => model.Name)
                    .Map(model => model.Decimal);

                var idPropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Id));
                var namePropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Name));
                var decimalPropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Decimal));

                var resourcePropertyHeadersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                resourcePropertyHeadersMock
                    .Setup(resourcePropertyHeaders => resourcePropertyHeaders.ContainsMap(It.IsAny<PropertyMap<FakeModel>>()))
                    .Returns(true);

                _mocker.GetMock<IResourcePropertyCollectionFactory>()
                    .Setup(resourcePropertyCollectionFactory => resourcePropertyCollectionFactory.CreateHeaders<FakeModel>())
                    .Returns(resourcePropertyHeadersMock.Object);

                var sut = CreateSystemUnderTest();

                // Act
                var readingResult = sut.ReadHeaderRow(readerMock.Object, map);

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

                var map = new FakeModelMap()
                    .Map(model => model.Id)
                    .Map(model => model.Name);

                var idPropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Id));
                var namePropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Name));

                var resourcePropertyHeadersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                resourcePropertyHeadersMock.Setup(resourcePropertyHeaders => resourcePropertyHeaders.ContainsMap(idPropertyMap))
                    .Returns(true);
                resourcePropertyHeadersMock.Setup(resourcePropertyHeaders => resourcePropertyHeaders.ContainsMap(namePropertyMap))
                    .Returns(false);

                _mocker.GetMock<IResourcePropertyCollectionFactory>()
                    .Setup(resourcePropertyCollectionFactory => resourcePropertyCollectionFactory.CreateHeaders<FakeModel>())
                    .Returns(resourcePropertyHeadersMock.Object);

                var sut = CreateSystemUnderTest();

                // Act
                var readingResult = sut.ReadHeaderRow(readerMock.Object, map);

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

                var map = new FakeModelMap()
                    .Map(model => model.Id)
                    .Map(model => model.Name)
                    .Map(model => model.Decimal, keyAction => keyAction.IgnoreName().UseNumber(2U));

                var idPropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Id));
                var namePropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Name));
                var decimalPropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Decimal));

                var resourcePropertyHeadersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                resourcePropertyHeadersMock.Setup(resourcePropertyHeaders => resourcePropertyHeaders.ContainsMap(idPropertyMap))
                    .Returns(true);
                resourcePropertyHeadersMock.Setup(resourcePropertyHeaders => resourcePropertyHeaders.ContainsMap(namePropertyMap))
                    .Returns(false);
                resourcePropertyHeadersMock.Setup(resourcePropertyHeaders => resourcePropertyHeaders.ContainsMap(decimalPropertyMap))
                    .Returns(false);

                _mocker.GetMock<IResourcePropertyCollectionFactory>()
                    .Setup(resourcePropertyCollectionFactory => resourcePropertyCollectionFactory.CreateHeaders<FakeModel>())
                    .Returns(resourcePropertyHeadersMock.Object);

                var sut = CreateSystemUnderTest();

                // Act
                var readingResult = sut.ReadHeaderRow(readerMock.Object, map);

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

                var map = new FakeModelMap()
                    .Map(model => model.Id)
                    .Map(model => model.Name);

                var resourcePropertyHeadersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();

                _mocker.GetMock<IResourcePropertyCollectionFactory>()
                    .Setup(resourcePropertyCollectionFactory => resourcePropertyCollectionFactory.CreateHeaders<FakeModel>())
                    .Returns(resourcePropertyHeadersMock.Object);

                var sut = CreateSystemUnderTest();

                // Act
                var readingResult = sut.ReadHeaderRow(readerMock.Object, map);

                // Assert
                resourcePropertyHeadersMock
                    .Verify(
                        resourcePropertyHeaders => resourcePropertyHeaders.Add(It.IsAny<PropertyMap<FakeModel>>(), It.IsAny<uint>()),
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

                var map = new FakeModelMap(optionsBuilderAction => optionsBuilderAction.OverrideHeaderRowNumber(headerRowNumber))
                    .Map(model => model.Id);

                var idPropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Id));

                var resourcePropertyHeadersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                resourcePropertyHeadersMock.Setup(resourcePropertyHeaders => resourcePropertyHeaders.ContainsMap(idPropertyMap))
                    .Returns(true);

                _mocker.GetMock<IResourcePropertyCollectionFactory>()
                    .Setup(resourcePropertyCollectionFactory => resourcePropertyCollectionFactory.CreateHeaders<FakeModel>())
                    .Returns(resourcePropertyHeadersMock.Object);

                var sut = CreateSystemUnderTest();

                // Act
                var readingResult = sut.ReadHeaderRow(readerMock.Object, map);

                // Assert
                resourcePropertyHeadersMock.Verify(resourcePropertyHeaders => resourcePropertyHeaders.Add(idPropertyMap, 1U));

                Assert.Null(readingResult.Failure);
            }

            [Fact]
            public void DoesNotGenerateMissingFailureForPropertyMapWithOptionalHeaderOptionsExtension()
            {
                // Arrange
                var readerMock = _mocker.GetMock<IWorksheetElementReader>();
                readerMock.Setup(reader => reader.ReadToRow(1U))
                    .Returns(true);

                MockWorksheetElementReaderCells(new FakeWorksheetCell(new CellLocation("A1"), nameof(FakeModel.Id)));

                var map = new FakeModelMap()
                    .Map(model => model.Id)
                    .Map(model => model.Name, optionsAction => optionsAction.MarkHeaderAsOptional());

                var idPropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Id));
                var namePropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Name));

                var resourcePropertyHeadersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                resourcePropertyHeadersMock.Setup(resourcePropertyHeaders => resourcePropertyHeaders.ContainsMap(idPropertyMap))
                    .Returns(true);
                resourcePropertyHeadersMock.Setup(resourcePropertyHeaders => resourcePropertyHeaders.ContainsMap(namePropertyMap))
                    .Returns(false);

                _mocker.GetMock<IResourcePropertyCollectionFactory>()
                    .Setup(resourcePropertyCollectionFactory => resourcePropertyCollectionFactory.CreateHeaders<FakeModel>())
                    .Returns(resourcePropertyHeadersMock.Object);

                var sut = CreateSystemUnderTest();

                // Act
                var readingResult = sut.ReadHeaderRow(readerMock.Object, map);

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

                var map = new FakeModelMap()
                    .Map(model => model.Id);

                var idPropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Id));

                var resourcePropertyHeadersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                resourcePropertyHeadersMock.Setup(resourcePropertyHeaders => resourcePropertyHeaders.ContainsMap(idPropertyMap))
                    .Returns(true);

                _mocker.GetMock<IResourcePropertyCollectionFactory>()
                    .Setup(resourcePropertyCollectionFactory => resourcePropertyCollectionFactory.CreateHeaders<FakeModel>())
                    .Returns(resourcePropertyHeadersMock.Object);

                var sut = CreateSystemUnderTest();

                // Act
                var readingResult = sut.ReadHeaderRow(readerMock.Object, map);

                // Assert
                resourcePropertyHeadersMock.Verify(resourcePropertyHeaders => resourcePropertyHeaders.Add(idPropertyMap, 1U));

                Assert.Null(readingResult.Failure);
            }
        }

        public class TheReadBodyRowMethod : SpreadsheetPageMapReaderFacts
        {
            [Fact]
            public void ReturnsReadingResultWithoutFailuresWhenAllRequiredPropertyMapsAreFound()
            {
                // Arrange
                var rowNumber = 2U;

                var readerMock = _mocker.GetMock<IWorksheetElementReader>();
                readerMock.Setup(reader => reader.GetRowNumber())
                    .Returns(rowNumber);

                var map = new FakeModelMap()
                    .Map(model => model.Id)
                    .Map(model => model.Name)
                    .Map(model => model.Decimal);

                var idPropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Id));
                var namePropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Name));
                var decimalPropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Decimal));

                var idCell = MockResourceProperty(rowNumber, 1U, "1", 1, idPropertyMap);
                var nameCell = MockResourceProperty(rowNumber, 2U, "foo", "foo", namePropertyMap);
                var decimalCell = MockResourceProperty(rowNumber, 3U, "2.5", 2.5M, decimalPropertyMap);
                MockWorksheetElementReaderCells(idCell, nameCell, decimalCell);

                var columnNumbers = new[]
                {
                    idCell.Location.Column.Number,
                    nameCell.Location.Column.Number,
                    decimalCell.Location.Column.Number,
                };
                var propertyHeadersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                propertyHeadersMock.SetupGet(propertyHeaders => propertyHeaders.ColumnNumbers)
                    .Returns(columnNumbers);

                var propertyValuesMock = _mocker.GetMock<IResourcePropertyValues<FakeModel>>();

                _mocker.GetMock<IResourcePropertyCollectionFactory>()
                    .Setup(resourcePropertyCollectionFactory => resourcePropertyCollectionFactory.CreateValues<FakeModel>())
                    .Returns(propertyValuesMock.Object);

                var expectedModel = new FakeModel
                {
                    Id = 1,
                    Name = "foo",
                    Decimal = 2.5M,
                };
                _mocker.GetMock<IResourceCreator>()
                    .Setup(resourceCreator => resourceCreator.Create(map, propertyValuesMock.Object))
                    .Returns(expectedModel);

                var sut = CreateSystemUnderTest();

                // Act
                var readingResult = sut.ReadBodyRow(readerMock.Object, map, propertyHeadersMock.Object);

                // Assert
                Assert.Equal(expectedModel, readingResult.Resource);
                propertyValuesMock.Verify();

                Assert.Null(readingResult.Failure);
            }

            [Fact]
            public void ReturnsReadingResultWithMissingFailureWhenRequiredPropertyMapNotFoundAndDefaultValueNotResolved()
            {
                // Arrange
                var rowNumber = 2U;

                var readerMock = _mocker.GetMock<IWorksheetElementReader>();
                readerMock.Setup(reader => reader.GetRowNumber())
                    .Returns(rowNumber);

                var nameColumnNumber = 2U;
                object? nameValue = default;

                var map = new FakeModelMap()
                    .Map(model => model.Id)
                    .Map(model => model.Name);

                var idPropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Id));
                var namePropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Name));

                var idCell = MockResourceProperty(rowNumber, 1U, "1", 1, idPropertyMap);
                MockWorksheetElementReaderCells(idCell);

                var columnNumbers = new[]
                {
                    idCell.Location.Column.Number,
                    nameColumnNumber,
                };
                var propertyHeadersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                propertyHeadersMock.SetupGet(propertyHeaders => propertyHeaders.ColumnNumbers)
                    .Returns(columnNumbers);

                propertyHeadersMock.Setup(propertyHeaders => propertyHeaders.GetMap(nameColumnNumber))
                    .Returns(namePropertyMap);

                var resourcePropertyDefaultValueResolverMock = _mocker.GetMock<IResourcePropertyDefaultValueResolver>();
                resourcePropertyDefaultValueResolverMock
                    .Setup(resourcePropertyDefaultValueResolver => resourcePropertyDefaultValueResolver.TryResolve(
                        namePropertyMap,
                        ResourcePropertyParseResultKind.Missing,
                        out nameValue))
                    .Returns(false);

                var propertyValuesMock = _mocker.GetMock<IResourcePropertyValues<FakeModel>>();

                _mocker.GetMock<IResourcePropertyCollectionFactory>()
                    .Setup(resourcePropertyCollectionFactory => resourcePropertyCollectionFactory.CreateValues<FakeModel>())
                    .Returns(propertyValuesMock.Object);

                var sut = CreateSystemUnderTest();

                // Act
                var readingResult = sut.ReadBodyRow(readerMock.Object, map, propertyHeadersMock.Object);

                // Assert
                Assert.Null(readingResult.Resource);
                propertyValuesMock.Verify();

                Assert.NotNull(readingResult.Failure);
                Assert.Equal(rowNumber, readingResult.Failure!.RowNumber);
                Assert.Empty(readingResult.Failure.InvalidProperties);
                var missingFailure = Assert.Single(readingResult.Failure.MissingProperties);
                Assert.Equal(nameColumnNumber, missingFailure.ColumnNumber);
            }

            [Fact]
            public void ReturnsReadingResultWithInvalidFailureWhenValueNotResolved()
            {
                // Arrange
                var rowNumber = 2U;

                var readerMock = _mocker.GetMock<IWorksheetElementReader>();
                readerMock.Setup(reader => reader.GetRowNumber())
                    .Returns(rowNumber);

                var nameColumnNumber = 2U;
                var nameCellValue = "foo";
                object? nameValue = default;

                var map = new FakeModelMap()
                    .Map(model => model.Id)
                    .Map(model => model.Name);

                var idPropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Id));
                var namePropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Name));

                var idCell = MockResourceProperty(rowNumber, 1U, "1", 1, idPropertyMap);
                MockWorksheetElementReaderCells(
                    idCell,
                    new FakeWorksheetCell(new CellLocation(rowNumber, nameColumnNumber), nameCellValue));

                var columnNumbers = new[]
                {
                    idCell.Location.Column.Number,
                    nameColumnNumber,
                };
                var propertyHeadersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                propertyHeadersMock.SetupGet(propertyHeaders => propertyHeaders.ColumnNumbers)
                    .Returns(columnNumbers);
                propertyHeadersMock.Setup(propertyHeaders => propertyHeaders.TryGetMap(nameColumnNumber, out namePropertyMap))
                    .Returns(true);

                _mocker.GetMock<IResourcePropertyValueResolver>()
                    .Setup(propertyValueResolver => propertyValueResolver.TryResolve(nameCellValue, namePropertyMap, out nameValue))
                    .Returns(false);

                var resourcePropertyDefaultValueResolverMock = _mocker.GetMock<IResourcePropertyDefaultValueResolver>();
                resourcePropertyDefaultValueResolverMock
                    .Setup(resourcePropertyDefaultValueResolver => resourcePropertyDefaultValueResolver.TryResolve(
                        namePropertyMap,
                        ResourcePropertyParseResultKind.Missing,
                        out nameValue))
                    .Returns(false);

                var propertyValuesMock = _mocker.GetMock<IResourcePropertyValues<FakeModel>>();

                _mocker.GetMock<IResourcePropertyCollectionFactory>()
                    .Setup(resourcePropertyCollectionFactory => resourcePropertyCollectionFactory.CreateValues<FakeModel>())
                    .Returns(propertyValuesMock.Object);

                var sut = CreateSystemUnderTest();

                // Act
                var readingResult = sut.ReadBodyRow(readerMock.Object, map, propertyHeadersMock.Object);

                // Assert
                Assert.Null(readingResult.Resource);
                propertyValuesMock.Verify();

                Assert.NotNull(readingResult.Failure);
                Assert.Equal(rowNumber, readingResult.Failure!.RowNumber);
                Assert.Empty(readingResult.Failure.MissingProperties);
                var invalidFailure = Assert.Single(readingResult.Failure.InvalidProperties);
                Assert.Equal(nameColumnNumber, invalidFailure.ColumnNumber);
                Assert.Equal(nameCellValue, invalidFailure.Value);
            }

            [Fact]
            public void SkipsCellWithoutAssociatedPropertyMap()
            {
                // Arrange
                var rowNumber = 2U;

                var readerMock = _mocker.GetMock<IWorksheetElementReader>();
                readerMock.Setup(reader => reader.GetRowNumber())
                    .Returns(rowNumber);

                var map = new FakeModelMap()
                    .Map(model => model.Id);

                var idPropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Id));

                var idCell = MockResourceProperty(rowNumber, 1U, "1", 1, idPropertyMap);
                MockWorksheetElementReaderCells(
                    idCell,
                    new FakeWorksheetCell(new CellLocation(rowNumber, 2U), "foo"));

                var columnNumbers = new[]
                {
                    idCell.Location.Column.Number,
                };
                var propertyHeadersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                propertyHeadersMock.SetupGet(propertyHeaders => propertyHeaders.ColumnNumbers)
                    .Returns(columnNumbers);

                var propertyValuesMock = _mocker.GetMock<IResourcePropertyValues<FakeModel>>();

                _mocker.GetMock<IResourcePropertyCollectionFactory>()
                    .Setup(resourcePropertyCollectionFactory => resourcePropertyCollectionFactory.CreateValues<FakeModel>())
                    .Returns(propertyValuesMock.Object);

                var expectedModel = new FakeModel
                {
                    Id = 1,
                };
                _mocker.GetMock<IResourceCreator>()
                    .Setup(resourceCreator => resourceCreator.Create(map, propertyValuesMock.Object))
                    .Returns(expectedModel);

                var sut = CreateSystemUnderTest();

                // Act
                var readingResult = sut.ReadBodyRow(readerMock.Object, map, propertyHeadersMock.Object);

                // Assert
                Assert.Equal(expectedModel, readingResult.Resource);
                propertyValuesMock.Verify();

                Assert.Null(readingResult.Failure);
            }

            [Fact]
            public void UsesResolvedDefaultValueForMissingRequiredPropertyMap()
            {
                // Arrange
                var rowNumber = 2U;

                var readerMock = _mocker.GetMock<IWorksheetElementReader>();
                readerMock.Setup(reader => reader.GetRowNumber())
                    .Returns(rowNumber);

                var idColumnNumber = 1U;
                object? idValue = 1;

                var map = new FakeModelMap()
                    .Map(model => model.Id);

                var idPropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Id));

                MockWorksheetElementReaderCells();

                var propertyHeadersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                propertyHeadersMock.SetupGet(propertyHeaders => propertyHeaders.ColumnNumbers)
                    .Returns(new[] { idColumnNumber, });

                propertyHeadersMock.Setup(propertyHeaders => propertyHeaders.GetMap(idColumnNumber))
                    .Returns(idPropertyMap);

                var resourcePropertyDefaultValueResolverMock = _mocker.GetMock<IResourcePropertyDefaultValueResolver>();
                resourcePropertyDefaultValueResolverMock
                    .Setup(resourcePropertyDefaultValueResolver => resourcePropertyDefaultValueResolver.TryResolve(
                        idPropertyMap,
                        ResourcePropertyParseResultKind.Missing,
                        out idValue))
                    .Returns(true);

                var propertyValuesMock = _mocker.GetMock<IResourcePropertyValues<FakeModel>>();

                _mocker.GetMock<IResourcePropertyCollectionFactory>()
                    .Setup(resourcePropertyCollectionFactory => resourcePropertyCollectionFactory.CreateValues<FakeModel>())
                    .Returns(propertyValuesMock.Object);

                var sut = CreateSystemUnderTest();

                // Act
                var readingResult = sut.ReadBodyRow(readerMock.Object, map, propertyHeadersMock.Object);

                // Assert
                Assert.Null(readingResult.Resource);
                propertyValuesMock.Verify(propertyValues => propertyValues.Add(idPropertyMap, idValue));

                Assert.Null(readingResult.Failure);
            }

            [Fact]
            public void UsesResolvedDefaultValueForEmptyOptionalPropertyMap()
            {
                // Arrange
                var rowNumber = 2U;

                var readerMock = _mocker.GetMock<IWorksheetElementReader>();
                readerMock.Setup(reader => reader.GetRowNumber())
                    .Returns(rowNumber);

                var idColumnNumber = 1U;
                object? idValue = 1;

                var map = new FakeModelMap()
                    .Map(model => model.Id, optionsAction => optionsAction.MarkValueAsOptional());

                var idPropertyMap = map.Properties.Single(p => p.Property.Name == nameof(FakeModel.Id));

                MockWorksheetElementReaderCells();

                var propertyHeadersMock = _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>();
                propertyHeadersMock.SetupGet(propertyHeaders => propertyHeaders.ColumnNumbers)
                    .Returns(new[] { idColumnNumber, });

                propertyHeadersMock.Setup(propertyHeaders => propertyHeaders.GetMap(idColumnNumber))
                    .Returns(idPropertyMap);

                var resourcePropertyDefaultValueResolverMock = _mocker.GetMock<IResourcePropertyDefaultValueResolver>();
                resourcePropertyDefaultValueResolverMock
                    .Setup(resourcePropertyDefaultValueResolver => resourcePropertyDefaultValueResolver.TryResolve(
                        idPropertyMap,
                        ResourcePropertyParseResultKind.Empty,
                        out idValue))
                    .Returns(true);

                var propertyValuesMock = _mocker.GetMock<IResourcePropertyValues<FakeModel>>();

                _mocker.GetMock<IResourcePropertyCollectionFactory>()
                    .Setup(resourcePropertyCollectionFactory => resourcePropertyCollectionFactory.CreateValues<FakeModel>())
                    .Returns(propertyValuesMock.Object);

                var sut = CreateSystemUnderTest();

                // Act
                var readingResult = sut.ReadBodyRow(readerMock.Object, map, propertyHeadersMock.Object);

                // Assert
                Assert.Null(readingResult.Resource);
                propertyValuesMock.Verify(propertyValues => propertyValues.Add(idPropertyMap, idValue));

                Assert.Null(readingResult.Failure);
            }

            private FakeWorksheetCell MockResourceProperty(
                uint rowNumber,
                uint columnNumber,
                string cellValue,
                object? value,
                PropertyMap<FakeModel> map)
            {
                _mocker.GetMock<IResourcePropertyHeaders<FakeModel>>()
                    .Setup(headers => headers.TryGetMap(columnNumber, out map!))
                    .Returns(true);
                _mocker.GetMock<IResourcePropertyValueResolver>()
                    .Setup(valueResolver => valueResolver.TryResolve(cellValue, map, out value))
                    .Returns(true);
                _mocker.GetMock<IResourcePropertyValues<FakeModel>>()
                    .Setup(values => values.Add(map, value))
                    .Verifiable();

                return new FakeWorksheetCell(new CellLocation(rowNumber, columnNumber), cellValue);
            }
        }

        private class FakeWorksheetCell
        {
            public FakeWorksheetCell(CellLocation location, string value)
            {
                Location = location;
                Value = value;
            }

            public CellLocation Location { get; }

            public string Value { get; }
        }
    }
}
