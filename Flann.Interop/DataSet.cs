
namespace Flann
{
    using System;
    using System.Runtime.InteropServices;

    public class DataSet<T> where T : struct, IEquatable<T>, IFormattable
    {
        private readonly int rows;
        private readonly int columns;
        private readonly T[] data;
        private readonly int sizeT;

        /// <summary>
        /// Gets the number of rows in the data set.
        /// </summary>
        public int Rows => rows;

        /// <summary>
        /// Gets the number of columns in the data set (dimensionality).
        /// </summary>
        public int Columns => columns;

        /// <summary>
        /// Gets the raw data array.
        /// </summary>
        public T[] Data => data;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSet{T}"/> class.
        /// </summary>
        /// <param name="rows">The number of rows in the data set.</param>
        /// <param name="columns">The number of columns in the dataset (dimensionality).</param>
        public DataSet(int rows, int columns)
            : this(rows, columns, new T[rows * columns])
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSet{T}"/> class.
        /// </summary>
        /// <param name="item">The single row of the data set.</param>
        public DataSet(T[] item)
            : this(1, item.Length, item)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSet{T}"/> class.
        /// </summary>
        /// <param name="rows">The number of rows in the data set.</param>
        /// <param name="columns">The number of columns in the data set (dimensionality).</param>
        /// <param name="data">The already allocated memory (size must be at least rows * columns)</param>
        public DataSet(int rows, int columns, T[] data)
        {
            if (data.Length < rows * columns)
            {
                throw new ArgumentException("Invalid array size.", nameof(data));
            }

            this.rows = rows;
            this.columns = columns;
            this.data = data;

            sizeT = Marshal.SizeOf(typeof(T));
        }

        /// <summary>
        /// Sets the values for given row index.
        /// </summary>
        /// <param name="i">The row index.</param>
        /// <param name="values">The row values.</param>
        public void SetRow(int i, T[] values)
        {
            if (i < 0 || i >= rows)
            {
                throw new ArgumentException("Row index out of range.", nameof(i));
            }

            if (values.Length != columns)
            {
                throw new ArgumentException("Invalid vector dimension.", nameof(values));
            }

            int offset = sizeT * columns;

            Buffer.BlockCopy(values, 0, data, i * offset, offset);
        }
    }
}
