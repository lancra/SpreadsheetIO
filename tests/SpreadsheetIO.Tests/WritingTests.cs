using System.Drawing;
using LanceC.SpreadsheetIO.Styling;
using LanceC.SpreadsheetIO.Tests.Testing;
using LanceC.SpreadsheetIO.Tests.Testing.Fakes;
using LanceC.SpreadsheetIO.Tests.Testing.Fixtures;
using LanceC.SpreadsheetIO.Writing;
using Xunit;
using Xunit.Abstractions;

namespace LanceC.SpreadsheetIO.Tests;

public class WritingTests : IDisposable
{
    private readonly FileExcelFixture _excelFixture;

    public WritingTests(ITestOutputHelper output)
    {
        _excelFixture = new(output);
    }

    [Fact]
    [ExcelSourceFile("ManualSingle.xlsx")]
    public void ManualSpreadsheetPage()
    {
        // Act
        using (var spreadsheet = _excelFixture.CreateSpreadsheet())
        {
            spreadsheet.AddPage("Manual")
                .AddCell("Foo")
                .AddCell("Bar")
                .AdvanceColumn()
                .AddCell("Baz")
                .AdvanceRow()
                .AddCell("One")
                .AdvanceColumn()
                .AddCell("Two")
                .AddCell("Three")
                .AdvanceRow()
                .AdvanceRow()
                .AdvanceColumn()
                .AddCell("Four")
                .AddCell("Five")
                .AddCell("Six");
        }

        // Assert
        _excelFixture.EquivalentToSource();
    }

    [Fact]
    [ExcelSourceFile("ManualSingleNumbers.xlsx")]
    public void ManualSpreadsheetPageWithNumbersOnly()
    {
        // Act
        using (var spreadsheet = _excelFixture.CreateSpreadsheet())
        {
            spreadsheet.AddPage("Manual Numbers")
                .AddCell(new WritingCell(new(11)))
                .AddCell(new WritingCell(new(21L)))
                .AddCell(new WritingCell(new(31M)))
                .AdvanceRow()
                .AdvanceColumn()
                .AddCell(new WritingCell(new(22D)))
                .AdvanceRows(1)
                .AddCell(new WritingCell(new(13U)))
                .AdvanceToColumn(3)
                .AddCell(new WritingCell(new(33)))
                .AdvanceToRow(4)
                .AddCell(new WritingCell(new(14)));
        }

        // Assert
        _excelFixture.EquivalentToSource();
    }

    [Fact]
    [ExcelSourceFile("ManualMultiple.xlsx")]
    public void ManualSpreadsheetPages()
    {
        // Act
        using (var spreadsheet = _excelFixture.CreateSpreadsheet())
        {
            spreadsheet.AddPage("One")
                .AddCell("One")
                .AddCell("Two")
                .AddCell("Three")
                .AdvanceToRow(3)
                .AddCell("Uno")
                .AddCell("Dos")
                .AddCell("Tres")
                .AdvanceToRow(6)
                .AdvanceToColumn("D")
                .AddCell("Un")
                .AddCell("Deux")
                .AddCell("Trois");

            spreadsheet.AddPage("Two")
                .AdvanceRows(1)
                .AddCell("One")
                .AdvanceColumns(2)
                .AddCell("Two")
                .AdvanceToColumn(6)
                .AddCell("Three");
        }

        // Assert
        _excelFixture.EquivalentToSource();
    }

    [Fact]
    [ExcelSourceFile("MapSimple.xlsx")]
    public void MappedSimpleSpreadsheetPage()
    {
        // Arrange
        var models = new[]
        {
            new FakeModel
            {
                Id = 1,
                Name = "One",
                DisplayName = "Uno",
                Date = new DateTime(2021, 1, 1),
                Amount = 1.1100000000000001M,
                Letter = 'O',
            },
            new FakeModel
            {
                Id = 2,
                Name = "Two",
                Date = new DateTime(2021, 2, 22),
                Amount = 22M,
                Letter = 'T',
            },
            new FakeModel
            {
                Id = 3,
                Name = "Three",
                DisplayName = "Tres",
                Date = new DateTime(2021, 3, 3),
                Amount = 3M,
                Letter = 'T',
            },
        };

        // Act
        using (var spreadsheet = _excelFixture.CreateSpreadsheet(map => map.ApplyConfiguration(new FakeModelMapConfiguration())))
        {
            spreadsheet.WritePage("Map", models);
        }

        // Assert
        _excelFixture.EquivalentToSource();
    }

    [Fact]
    [ExcelSourceFile("Write1900Dates.xlsx")]
    public void Mapped1900Dates()
    {
        // Arrange
        var models = new[]
        {
            new FakeDateModel(new DateTime(1900, 3, 1)),
            new FakeDateModel(new DateTime(1900, 2, 28)),
            new FakeDateModel(new DateTime(1900, 2, 27)),
            new FakeDateModel(new DateTime(1900, 1, 2)),
            new FakeDateModel(new DateTime(1900, 1, 1)),
            new FakeDateModel(new DateTime(1899, 12, 31)),
        };

        // Act
        using (var spreadsheet = _excelFixture.CreateSpreadsheet(map => map.ApplyConfiguration(new FakeDateModelMapConfiguration())))
        {
            spreadsheet.WritePage("1900 Dates", models);
        }

        // Assert
        _excelFixture.EquivalentToSource();
    }

    [Fact]
    [ExcelSourceFile("StyleAlignment.xlsx")]
    public void StylingAlignments()
    {
        // Arrange
        static Style CreateHorizontalStyle(HorizontalAlignmentKind horizontalAlignmentKind)
            => Style.Default with { Alignment = Alignment.Default with { HorizontalKind = horizontalAlignmentKind, } };
        static Style CreateVerticalStyle(VerticalAlignmentKind verticalAlignmentKind)
            => Style.Default with { Alignment = Alignment.Default with { VerticalKind = verticalAlignmentKind, } };
        static Style CreateStyle(HorizontalAlignmentKind horizontalAlignmentKind, VerticalAlignmentKind verticalAlignmentKind)
            => Style.Default with { Alignment = new(horizontalAlignmentKind, verticalAlignmentKind), };

        // Act
        using (var spreadsheet = _excelFixture.CreateSpreadsheet())
        {
            spreadsheet
                .AddStyle("HLeft", CreateHorizontalStyle(HorizontalAlignmentKind.Left))
                .AddStyle("HCenter", CreateHorizontalStyle(HorizontalAlignmentKind.Center))
                .AddStyle("HRight", CreateHorizontalStyle(HorizontalAlignmentKind.Right))
                .AddStyle("HFill", CreateHorizontalStyle(HorizontalAlignmentKind.Fill))
                .AddStyle("HJustify", CreateHorizontalStyle(HorizontalAlignmentKind.Justify))
                .AddStyle("HCenterContinuous", CreateHorizontalStyle(HorizontalAlignmentKind.CenterContinuous))
                .AddStyle("HDistributed", CreateHorizontalStyle(HorizontalAlignmentKind.Distributed))
                .AddStyle("HJustifyDistributed", CreateHorizontalStyle(HorizontalAlignmentKind.JustifyDistributed))
                .AddStyle("VTop", CreateVerticalStyle(VerticalAlignmentKind.Top))
                .AddStyle("VCenter", CreateVerticalStyle(VerticalAlignmentKind.Center))
                .AddStyle("VBottom", CreateVerticalStyle(VerticalAlignmentKind.Bottom))
                .AddStyle("VJustify", CreateVerticalStyle(VerticalAlignmentKind.Justify))
                .AddStyle("VDistributed", CreateVerticalStyle(VerticalAlignmentKind.Distributed))
                .AddStyle("HLeftVTop", CreateStyle(HorizontalAlignmentKind.Left, VerticalAlignmentKind.Top))
                .AddStyle("HCenterVCenter", CreateStyle(HorizontalAlignmentKind.Center, VerticalAlignmentKind.Center))
                .AddStyle("HRightVJustify", CreateStyle(HorizontalAlignmentKind.Right, VerticalAlignmentKind.Justify));

            spreadsheet.AddPage("Alignment")
                .AddCell("HLeft", "HLeft")
                .AddCell("HCenter", "HCenter")
                .AddCell("HRight", "HRight")
                .AddCell("HFill", "HFill")
                .AddCell("HJustify", "HJustify")
                .AddCell("HCenterContinuous", "HCenterContinuous")
                .AddCell("HDistributed", "HDistributed")
                .AddCell("HJustifyDistributed", "HJustifyDistributed")
                .AdvanceRow()
                .AddCell("VTop", "VTop")
                .AddCell("VCenter", "VCenter")
                .AddCell("VBottom", "VBottom")
                .AddCell("VJustify", "VJustify")
                .AddCell("VDistributed", "VDistributed")
                .AdvanceRow()
                .AddCell("HLeftVTop", "HLeftVTop")
                .AddCell("HCenterVCenter", "HCenterVCenter")
                .AddCell("HRightVJustify", "HRightVJustify");
        }

        // Assert
        _excelFixture.EquivalentToSource();
    }

    [Fact]
    [ExcelSourceFile("StyleBorder.xlsx")]
    public void StylingBorders()
    {
        // Arrange
        static Style CreateLeftStyle(Color color, BorderLineKind borderLineKind)
            => Style.Default with { Border = Border.Default with { LeftLine = new BorderLine(color, borderLineKind), }, };
        static Style CreateRightStyle(Color color, BorderLineKind borderLineKind)
            => Style.Default with { Border = Border.Default with { RightLine = new BorderLine(color, borderLineKind), }, };
        static Style CreateTopStyle(Color color, BorderLineKind borderLineKind)
            => Style.Default with { Border = Border.Default with { TopLine = new BorderLine(color, borderLineKind), }, };
        static Style CreateBottomStyle(Color color, BorderLineKind borderLineKind)
            => Style.Default with { Border = Border.Default with { BottomLine = new BorderLine(color, borderLineKind), }, };
        static Style CreateStyle(Color color, BorderLineKind borderLineKind)
            => Style.Default with { Border = new(new(color, borderLineKind)), };

        // Act
        using (var spreadsheet = _excelFixture.CreateSpreadsheet())
        {
            spreadsheet
                .AddStyle("LBlackThin", CreateLeftStyle(Color.Black, BorderLineKind.Thin))
                .AddStyle("LRedThin", CreateLeftStyle(Color.Red, BorderLineKind.Thin))
                .AddStyle("ROrangeThick", CreateRightStyle(Color.Orange, BorderLineKind.Thick))
                .AddStyle("RYellowDashed", CreateRightStyle(Color.Yellow, BorderLineKind.Dashed))
                .AddStyle("TGreenDotted", CreateTopStyle(Color.Green, BorderLineKind.Dotted))
                .AddStyle("TBlueDotted", CreateTopStyle(Color.Blue, BorderLineKind.Dotted))
                .AddStyle("BPurpleDouble", CreateBottomStyle(Color.Purple, BorderLineKind.Double))
                .AddStyle("BSaddleBrownDouble", CreateBottomStyle(Color.SaddleBrown, BorderLineKind.Double))
                .AddStyle("OBlackThick", CreateStyle(Color.Black, BorderLineKind.Thick))
                .AddStyle("OOliveDashed", CreateStyle(Color.Olive, BorderLineKind.Dashed));

            spreadsheet.AddPage("Border")
                .AddCell("LBlackThin", "LBlackThin")
                .AdvanceColumn()
                .AddCell("LRedThin", "LRedThin")
                .AdvanceRows(2)
                .AddCell("ROrangeThick", "ROrangeThick")
                .AdvanceColumn()
                .AddCell("RYellowDashed", "RYellowDashed")
                .AdvanceRows(2)
                .AddCell("TGreenDotted", "TGreenDotted")
                .AdvanceColumn()
                .AddCell("TBlueDotted", "TBlueDotted")
                .AdvanceRows(2)
                .AddCell("BPurpleDouble", "BPurpleDouble")
                .AdvanceColumn()
                .AddCell("BSaddleBrownDouble", "BSaddleBrownDouble")
                .AdvanceRows(2)
                .AddCell("OBlackThick", "OBlackThick")
                .AdvanceColumn()
                .AddCell("OOliveDashed", "OOliveDashed");
        }

        // Assert
        _excelFixture.EquivalentToSource();
    }

    [Fact]
    [ExcelSourceFile("StyleFill.xlsx")]
    public void StylingFills()
    {
        // Arrange
        static Style CreateStyle(FillKind fillKind, Color foregroundColor)
            => Style.Default with { Fill = new(fillKind, foregroundColor), };

        // Act
        using (var spreadsheet = _excelFixture.CreateSpreadsheet())
        {
            spreadsheet
                .AddStyle("Red", CreateStyle(FillKind.Solid, Color.Red))
                .AddStyle("Orange", CreateStyle(FillKind.Solid, Color.Orange))
                .AddStyle("Yellow", CreateStyle(FillKind.Solid, Color.Yellow))
                .AddStyle("Green", CreateStyle(FillKind.Solid, Color.Green))
                .AddStyle("Blue", CreateStyle(FillKind.Solid, Color.Blue))
                .AddStyle("Purple", CreateStyle(FillKind.Solid, Color.Purple));

            spreadsheet.AddPage("Fill")
                .AddCell("Red", "Red").AdvanceRow()
                .AddCell("Orange", "Orange").AdvanceRow()
                .AddCell("Yellow", "Yellow").AdvanceRow()
                .AddCell("Green", "Green").AdvanceRow()
                .AddCell("Blue", "Blue").AdvanceRow()
                .AddCell("Purple", "Purple");
        }

        // Assert
        _excelFixture.EquivalentToSource();
    }

    [Fact]
    [ExcelSourceFile("StyleFont.xlsx")]
    public void StylingFonts()
    {
        // Arrange
        static Style CreateNameStyle(string name)
            => Style.Default with { Font = Font.Default with { Name = name, }, };
        static Style CreateSizeStyle(double size)
            => Style.Default with { Font = Font.Default with { Size = size, }, };
        static Style CreateColorStyle(Color color)
            => Style.Default with { Font = Font.Default with { Color = color, }, };
        static Style CreateStyle(string name, double size, Color color, bool isBold, bool isItalic)
            => Style.Default with { Font = new(name, size, color, isBold, isItalic), };

        // Act
        using (var spreadsheet = _excelFixture.CreateSpreadsheet())
        {
            // Excel performs some kind of sorting when creating font styles. Since the result is functionally identical, we will
            // add the styles in the expected order.
            spreadsheet
                .AddStyle("Red", CreateColorStyle(Color.Red))
                .AddStyle("Bold", CreateStyle("Calibri", 11D, Color.Black, true, false))
                .AddStyle("Arial", CreateNameStyle("Arial"))
                .AddStyle("Consolas", CreateNameStyle("Consolas"))
                .AddStyle("Times New Roman", CreateNameStyle("Times New Roman"))
                .AddStyle("Twelve", CreateSizeStyle(12D))
                .AddStyle("Fourteen", CreateSizeStyle(14D))
                .AddStyle("Twenty", CreateSizeStyle(20D))
                .AddStyle("Yellow", CreateColorStyle(Color.Yellow))
                .AddStyle("Green", CreateColorStyle(Color.Green))
                .AddStyle("Italic", CreateStyle("Calibri", 11D, Color.Black, false, true))
                .AddStyle("ArialTwelveRedBold", CreateStyle("Arial", 12D, Color.Red, true, false))
                .AddStyle("ConsolasFourteenGreenItalic", CreateStyle("Consolas", 14D, Color.Green, false, true));

            spreadsheet.AddPage("Font")
                .AddCell("Arial", "Arial")
                .AddCell("Consolas", "Consolas")
                .AddCell("Times New Roman", "Times New Roman")
                .AdvanceRow()
                .AddCell("Twelve", "Twelve")
                .AddCell("Fourteen", "Fourteen")
                .AddCell("Twenty", "Twenty")
                .AdvanceRows(1)
                .AddCell("Red", "Red")
                .AddCell("Yellow", "Yellow")
                .AddCell("Green", "Green")
                .AdvanceToRow(4)
                .AddCell("Bold", "Bold")
                .AddCell("Italic", "Italic")
                .AdvanceRow()
                .AddCell("ArialTwelveRedBold", "ArialTwelveRedBold")
                .AddCell("ConsolasFourteenGreenItalic", "ConsolasFourteenGreenItalic");
        }

        // Assert
        _excelFixture.EquivalentToSource();
    }

    [Fact]
    [ExcelSourceFile("StyleNumericFormat.xlsx")]
    public void StylingNumericFormats()
    {
        // Arrange
        static Style CreateStyle(NumericFormat numericFormat)
            => Style.Default with { NumericFormat = numericFormat, };
        static void AddCells(IWritingSpreadsheetPage spreadsheetPage, string styleName, bool advanceRow = true)
        {
            // Excel stores floating point numbers in an odd way, so this must be emulated for values to match.
            spreadsheetPage
                .AddCell(new WritingCell(new(12.345599999999999M), new(styleName)))
                .AddCell(new WritingCell(new(-78.900000000000006M), new(styleName)))
                .AddCell(new WritingCell(new(50D), new(styleName)))
                .AddCell(new WritingCell(new(-100.123D), new(styleName)));

            if (advanceRow)
            {
                spreadsheetPage.AdvanceRow();
            }
        }

        // Act
        using (var spreadsheet = _excelFixture.CreateSpreadsheet())
        {
            spreadsheet
                .AddStyle("AccountingTwoDollarSymbol", CreateStyle(new AccountingNumericFormat(2, "$").ToNumericFormat()))
                .AddStyle("AccountingFourNone", CreateStyle(new AccountingNumericFormat(4, string.Empty).ToNumericFormat()))
                .AddStyle("AccountingFiveUsd", CreateStyle(new AccountingNumericFormat(5, "USD").ToNumericFormat()))
                .AddStyle("AccountingSixEur", CreateStyle(new AccountingNumericFormat(6, "EUR").ToNumericFormat()))
                .AddStyle(
                    "CurrencyTwoDollarSymbolDefault",
                    CreateStyle(new CurrencyNumericFormat(2, "$", NegativeNumericFormatKind.Default).ToNumericFormat()))
                .AddStyle(
                    "CurrencyFourNoneParentheses",
                    CreateStyle(
                        new CurrencyNumericFormat(4, string.Empty, NegativeNumericFormatKind.Parentheses).ToNumericFormat()))
                .AddStyle(
                    "CurrencyFiveUsdRed",
                    CreateStyle(new CurrencyNumericFormat(5, "USD", NegativeNumericFormatKind.Red).ToNumericFormat()))
                .AddStyle(
                    "CurrencySixEurRedParentheses",
                    CreateStyle(new CurrencyNumericFormat(6, "EUR", NegativeNumericFormatKind.RedParentheses).ToNumericFormat()))
                .AddStyle(
                    "NumberTwoNoDefault",
                    CreateStyle(new NumberNumericFormat(2, false, NegativeNumericFormatKind.Default).ToNumericFormat()))
                .AddStyle(
                    "NumberFourYesParentheses",
                    CreateStyle(new NumberNumericFormat(4, true, NegativeNumericFormatKind.Parentheses).ToNumericFormat()))
                .AddStyle(
                    "NumberFiveNoRed",
                    CreateStyle(new NumberNumericFormat(5, false, NegativeNumericFormatKind.Red).ToNumericFormat()))
                .AddStyle(
                    "NumberSixYesRedParentheses",
                    CreateStyle(new NumberNumericFormat(6, true, NegativeNumericFormatKind.RedParentheses).ToNumericFormat()))
                .AddStyle("Text", CreateStyle(new TextNumericFormat().ToNumericFormat()));

            var spreadsheetPage = spreadsheet.AddPage("Numeric Format");
            AddCells(spreadsheetPage, "AccountingTwoDollarSymbol");
            AddCells(spreadsheetPage, "AccountingFourNone");
            AddCells(spreadsheetPage, "AccountingFiveUsd");
            AddCells(spreadsheetPage, "AccountingSixEur");
            AddCells(spreadsheetPage, "CurrencyTwoDollarSymbolDefault");
            AddCells(spreadsheetPage, "CurrencyFourNoneParentheses");
            AddCells(spreadsheetPage, "CurrencyFiveUsdRed");
            AddCells(spreadsheetPage, "CurrencySixEurRedParentheses");
            AddCells(spreadsheetPage, "NumberTwoNoDefault");
            AddCells(spreadsheetPage, "NumberFourYesParentheses");
            AddCells(spreadsheetPage, "NumberFiveNoRed");
            AddCells(spreadsheetPage, "NumberSixYesRedParentheses");
            AddCells(spreadsheetPage, "Text", false);
        }

        // Assert
        _excelFixture.EquivalentToSource();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _excelFixture.Dispose();
        }
    }
}
