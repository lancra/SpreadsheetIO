using System;

namespace LanceC.SpreadsheetIO.Reading.Internal.Readers
{
    /// <summary>
    /// Defines the reader for shared string table elements.
    /// </summary>
    internal interface ISharedStringTableElementReader : IDisposable
    {
        /// <summary>
        /// Reads to the next shared string item.
        /// </summary>
        /// <returns><c>true</c> if the shared string item was read to; otherwise, <c>false</c>.</returns>
        bool ReadNextItem();

        /// <summary>
        /// Gets the value of the current shared string item.
        /// </summary>
        /// <returns>The current shared string item value.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the current element is not a valid shared string item start element.
        /// </exception>
        string GetItemValue();
    }
}
