using LanceC.SpreadsheetIO.Shared;
using OpenXml = DocumentFormat.OpenXml.Spreadsheet;

namespace LanceC.SpreadsheetIO.Writing
{
    /// <summary>
    /// Represents the value segment of a <see cref="WritingCell"/>.
    /// </summary>
    public class WritingCellValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellValue"/> class with no value.
        /// </summary>
        public WritingCellValue()
            : this(default(Action<OpenXml.Cell>?))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellValue"/> class with an integer value.
        /// </summary>
        /// <param name="value">The integer value.</param>
        public WritingCellValue(int value)
            : this(CreateCellModifier(value))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellValue"/> class with a nullable integer value.
        /// </summary>
        /// <param name="value">The nullable integer value.</param>
        public WritingCellValue(int? value)
            : this(value.HasValue ? CreateCellModifier(value.Value) : default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellValue"/> class with an unsigned integer value.
        /// </summary>
        /// <param name="value">The unsigned integer value.</param>
        public WritingCellValue(uint value)
            : this(CreateCellModifier((double)value))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellValue"/> class with a nullable unsigned integer value.
        /// </summary>
        /// <param name="value">The nullable unsigned integer value.</param>
        public WritingCellValue(uint? value)
            : this(value.HasValue ? CreateCellModifier((double)value.Value) : default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellValue"/> class with a long value.
        /// </summary>
        /// <param name="value">The long value.</param>
        public WritingCellValue(long value)
            : this(CreateCellModifier((double)value))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellValue"/> class with a nullable long value.
        /// </summary>
        /// <param name="value">The nullable long value.</param>
        public WritingCellValue(long? value)
            : this(value.HasValue ? CreateCellModifier((double)value.Value) : default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellValue"/> class with an unsigned long value.
        /// </summary>
        /// <param name="value">The unsigned long value.</param>
        public WritingCellValue(ulong value)
            : this(CreateCellModifier((double)value))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellValue"/> class with a nullable unsigned long value.
        /// </summary>
        /// <param name="value">The nullable unsigned long value.</param>
        public WritingCellValue(ulong? value)
            : this(value.HasValue ? CreateCellModifier((double)value.Value) : default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellValue"/> class with a double value.
        /// </summary>
        /// <param name="value">The double value.</param>
        public WritingCellValue(double value)
            : this(CreateCellModifier(value))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellValue"/> class with a nullable double value.
        /// </summary>
        /// <param name="value">The nullable double value.</param>
        public WritingCellValue(double? value)
            : this(value.HasValue ? CreateCellModifier(value.Value) : default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellValue"/> class with a boolean value.
        /// </summary>
        /// <param name="value">The boolean value.</param>
        public WritingCellValue(bool value)
            : this(CreateCellModifier(value))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellValue"/> class with a nullable boolean value.
        /// </summary>
        /// <param name="value">The nullable boolean value.</param>
        public WritingCellValue(bool? value)
            : this(value.HasValue ? CreateCellModifier(value.Value) : default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellValue"/> class with a decimal value.
        /// </summary>
        /// <param name="value">The decimal value.</param>
        public WritingCellValue(decimal value)
            : this(CreateCellModifier(value))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellValue"/> class with a nullable decimal value.
        /// </summary>
        /// <param name="value">The nullable decimal value.</param>
        public WritingCellValue(decimal? value)
            : this(value.HasValue ? CreateCellModifier(value.Value) : default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellValue"/> class with a datetime value.
        /// </summary>
        /// <param name="value">The datetime value.</param>
        public WritingCellValue(DateTime value)
            : this(value, CellDateKind.Number)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellValue"/> class with a datetime value.
        /// </summary>
        /// <param name="value">The datetime value.</param>
        /// <param name="dateKind">The kind of date to write.</param>
        public WritingCellValue(DateTime value, CellDateKind dateKind)
            : this(CreateCellModifier(value, dateKind))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellValue"/> class with a nullable datetime value.
        /// </summary>
        /// <param name="value">The nullable datetime value.</param>
        public WritingCellValue(DateTime? value)
            : this(value, CellDateKind.Number)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellValue"/> class with a nullable datetime value.
        /// </summary>
        /// <param name="value">The nullable datetime value.</param>
        /// <param name="dateKind">The kind of date to write.</param>
        public WritingCellValue(DateTime? value, CellDateKind dateKind)
            : this(value.HasValue ? CreateCellModifier(value.Value, dateKind) : default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellValue"/> class with a datetime offset value.
        /// </summary>
        /// <param name="value">The datetime offset value.</param>
        public WritingCellValue(DateTimeOffset value)
            : this(value, CellDateKind.Number)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellValue"/> class with a datetime offset value.
        /// </summary>
        /// <param name="value">The datetime offset value.</param>
        /// <param name="dateKind">The kind of date to write.</param>
        public WritingCellValue(DateTimeOffset value, CellDateKind dateKind)
            : this(CreateCellModifier(value, dateKind))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellValue"/> class with a nullable datetime offset value.
        /// </summary>
        /// <param name="value">The nullable datetime offset value.</param>
        public WritingCellValue(DateTimeOffset? value)
            : this(value, CellDateKind.Number)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellValue"/> class with a nullable datetime offset value.
        /// </summary>
        /// <param name="value">The nullable datetime offset value.</param>
        /// <param name="dateKind">The kind of date to write.</param>
        public WritingCellValue(DateTimeOffset? value, CellDateKind dateKind)
            : this(value.HasValue ? CreateCellModifier(value.Value, dateKind) : default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellValue"/> class with a character value.
        /// </summary>
        /// <param name="value">The character value.</param>
        public WritingCellValue(char value)
            : this(value, CellStringKind.SharedString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellValue"/> class with a character value.
        /// </summary>
        /// <param name="value">The character value.</param>
        /// <param name="stringKind">The kind of string to write.</param>
        public WritingCellValue(char value, CellStringKind stringKind)
            : this(CreateCellModifier(value.ToString(), stringKind))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellValue"/> class with a nullable character value.
        /// </summary>
        /// <param name="value">The nullable character value.</param>
        public WritingCellValue(char? value)
            : this(value, CellStringKind.SharedString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellValue"/> class with a nullable character value.
        /// </summary>
        /// <param name="value">The nullable character value.</param>
        /// <param name="stringKind">The kind of string to write.</param>
        public WritingCellValue(char? value, CellStringKind stringKind)
            : this(value.HasValue ? CreateCellModifier(value.Value.ToString(), stringKind) : default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellValue"/> class with a string value.
        /// </summary>
        /// <param name="value">The string value.</param>
        public WritingCellValue(string? value)
            : this(value, CellStringKind.SharedString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritingCellValue"/> class with a string value.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <param name="stringKind">The kind of string to write.</param>
        public WritingCellValue(string? value, CellStringKind stringKind)
            : this(!string.IsNullOrEmpty(value) ? CreateCellModifier(value, stringKind) : default)
        {
        }

        private WritingCellValue(Action<OpenXml.Cell>? cellModifier)
        {
            CellModifier = cellModifier;
        }

        internal Action<OpenXml.Cell>? CellModifier { get; }

        private static Action<OpenXml.Cell> CreateCellModifier(int value)
            => (cell) => cell.CellValue = new OpenXml.CellValue(value);

        private static Action<OpenXml.Cell> CreateCellModifier(double value)
            => (cell) => cell.CellValue = new OpenXml.CellValue(value);

        private static Action<OpenXml.Cell> CreateCellModifier(bool value)
            => (cell) =>
            {
                cell.CellValue = new OpenXml.CellValue(Convert.ToInt32(value));
                cell.DataType = OpenXml.CellValues.Boolean;
            };

        private static Action<OpenXml.Cell> CreateCellModifier(decimal value)
            => (cell) => cell.CellValue = new OpenXml.CellValue(value);

        private static Action<OpenXml.Cell> CreateCellModifier(DateTime value, CellDateKind dateKind)
            => (cell) =>
            {
                if (dateKind == CellDateKind.Number)
                {
                    cell.CellValue = new OpenXml.CellValue(value.ToOADate());
                }
                else if (dateKind == CellDateKind.Text)
                {
                    cell.CellValue = new OpenXml.CellValue(value);
                    cell.DataType = OpenXml.CellValues.SharedString;
                }
            };

        private static Action<OpenXml.Cell> CreateCellModifier(DateTimeOffset value, CellDateKind dateKind)
            => (cell) =>
            {
                if (dateKind == CellDateKind.Number)
                {
                    cell.CellValue = new OpenXml.CellValue(value.DateTime.ToOADate());
                }
                else if (dateKind == CellDateKind.Text)
                {
                    cell.CellValue = new OpenXml.CellValue(value);
                    cell.DataType = OpenXml.CellValues.SharedString;
                }
            };

        private static Action<OpenXml.Cell> CreateCellModifier(string value, CellStringKind stringKind)
            => (cell) =>
            {
                cell.DataType = stringKind.OpenXmlValue;

                if (stringKind == CellStringKind.SharedString)
                {
                    cell.CellValue = new OpenXml.CellValue(value);
                    cell.DataType = OpenXml.CellValues.SharedString;
                }
                else if (stringKind == CellStringKind.InlineString)
                {
                    cell.DataType = OpenXml.CellValues.InlineString;
                    cell.InlineString = new OpenXml.InlineString
                    {
                        Text = new OpenXml.Text(value),
                    };
                }
            };
    }
}
