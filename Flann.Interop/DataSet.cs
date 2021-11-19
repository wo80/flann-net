
namespace Flann
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    /// <summary>
    /// A data set (array stored as row major order).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataSet<T> where T : struct, IEquatable<T>, IFormattable
    {
        private readonly int rows;
        private readonly int columns;
        private readonly T[] data;
        private readonly int sizeT;

        private int[] map;

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
        /// Gets the index map (optional).
        /// </summary>
        /// <remarks>
        /// The <see cref="SearchResult{T}"/> of a nearest neighbours search on the FLANN index contains a list of
        /// indices linearly pointing to the vector position (row) in the dataset.  Since the data might come from
        /// a database, the index map can be used to map the linear dataset array index to a not necessarily linear
        /// database index (row id).
        /// </remarks>
        public int[] IndexMap => map;

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
        /// <param name="data">The already allocated memory (size must be at least rows * columns).</param>
        public DataSet(int rows, int columns, T[] data)
            : this(rows, columns, data, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSet{T}"/> class.
        /// </summary>
        /// <param name="rows">The number of rows in the data set.</param>
        /// <param name="columns">The number of columns in the data set (dimensionality).</param>
        /// <param name="data">The already allocated memory for the data (size must be at least <c>rows * columns</c>).</param>
        /// <param name="map">Optional memory for the index map (if not null, size must be at least <c>rows</c>).</param>
        public DataSet(int rows, int columns, T[] data, int[] map)
        {
            if (data.Length < rows * columns)
            {
                throw new ArgumentException("Invalid data array size.", nameof(data));
            }

            if (map != null && map.Length < rows)
            {
                throw new ArgumentException("Invalid map array size.", nameof(map));
            }

            this.rows = rows;
            this.columns = columns;
            this.data = data;
            this.map = map;

            sizeT = Marshal.SizeOf(typeof(T));
        }

        /// <summary>
        /// Gets the values for given row index.
        /// </summary>
        /// <param name="i">The row index.</param>
        /// <returns>The row values.</returns>
        public T[] GetRow(int i)
        {
            var values = new T[columns];

            GetRow(i, values);

            return values;
        }

        /// <summary>
        /// Gets the values for given row index.
        /// </summary>
        /// <param name="i">The row index.</param>
        /// <param name="values">The row values.</param>
        public void GetRow(int i, T[] values)
        {
            if (i < 0 || i >= rows)
            {
                throw new ArgumentException("Row index out of range.", nameof(i));
            }

            if (values.Length < columns)
            {
                throw new ArgumentException("Invalid vector dimension.", nameof(values));
            }

            int offset = sizeT * columns;

            Buffer.BlockCopy(data, i * offset, values, 0, offset);
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

            if (values.Length < columns)
            {
                throw new ArgumentException("Invalid vector dimension.", nameof(values));
            }

            int offset = sizeT * columns;

            Buffer.BlockCopy(values, 0, data, i * offset, offset);
        }

        #region Serialization

        // File signature.
        private const uint MB = 0xF1A02323;

        private const uint MAP_HINT = 0x11222211;

        /// <summary>
        /// Serialize the data set.
        /// </summary>
        /// <param name="file">The file name.</param>
        public void Serialize(string file)
        {
            using (var stream = File.OpenWrite(file))
            {
                Serialize(stream);
            }
        }

        /// <summary>
        /// Serialize the data set.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        public void Serialize(Stream stream)
        {
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(MB);
                writer.Write(rows);
                writer.Write(columns);

                WriteData(writer, data, TypeToInt(typeof(T)));

                WriteMap(writer, map);
            }
        }

        /// <summary>
        /// Deserialize a data set from given file.
        /// </summary>
        /// <param name="file">The file name.</param>
        /// <returns>The data set.</returns>
        public static DataSet<T> Deserialize(string file)
        {
            using (var stream = File.OpenRead(file))
            {
                return Deserialize(stream);
            }
        }

        /// <summary>
        /// Deserialize a data set from given stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The data set.</returns>
        public static DataSet<T> Deserialize(Stream stream)
        {
            using (var reader = new BinaryReader(stream))
            {
                if (reader.ReadUInt32() != MB)
                {
                    throw new NotSupportedException("Invalid file format.");
                }

                var rows = reader.ReadInt32();
                var columns = reader.ReadInt32();
                var type = reader.ReadInt32();

                var data = ReadData(reader, (FlannDataType)type, rows, columns);

                if (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    data.map = ReadMap(reader, rows);
                }

                return data;
            }
        }

        private static int[] ReadMap(BinaryReader reader, int rows)
        {
            if (reader.ReadUInt32() != MAP_HINT)
            {
                throw new NotSupportedException("Invalid file format (expected index map).");
            }

            var map = new int[rows];

            for (int i = 0; i < rows; i++)
            {
                map[i] = reader.ReadInt32();
            }

            return map;
        }

        private static DataSet<T> ReadData(BinaryReader reader, FlannDataType type, int rows, int columns)
        {
            if (type != TypeToInt(typeof(T)))
            {
                throw new NotSupportedException("Invalid data type.");
            }

            switch (type)
            {
                case FlannDataType.Float32:
                    return ReadFloatData(reader, rows, columns);
                default:
                    break;
            }

            throw new NotSupportedException();
        }

        private static DataSet<T> ReadFloatData(BinaryReader reader, int rows, int columns)
        {
            var data = new DataSet<float>(rows, columns);

            var a = data.Data;

            for (int i = 0; i < rows * columns; i++)
            {
                a[i] = reader.ReadSingle();
            }

            return (DataSet<T>)(object)data;
        }

        private void WriteData(BinaryWriter writer, T[] data, FlannDataType type)
        {
            switch (type)
            {
                case FlannDataType.Float32:
                    WriteData(writer, (float[])(object)data);
                    return;
                default:
                    break;
            }

            throw new NotSupportedException();
        }

        private void WriteData(BinaryWriter writer, float[] data)
        {
            writer.Write((int)FlannDataType.Float32);

            for (int i = 0; i < rows * columns; i++)
            {
                writer.Write(data[i]);
            }
        }

        private void WriteMap(BinaryWriter writer, int[] map)
        {
            if (map == null) return;

            writer.Write(MAP_HINT);

            for (int i = 0; i < rows; i++)
            {
                writer.Write(map[i]);
            }
        }

        private static FlannDataType TypeToInt(Type type)
        {
            if (type == typeof(float))
            {
                return FlannDataType.Float32;
            }
            else if (type == typeof(double))
            {
                return FlannDataType.Float64;
            }
            else if (type == typeof(byte))
            {
                return FlannDataType.Uint8;
            }
            else if (type == typeof(int))
            {
                return FlannDataType.Int32;
            }

            throw new NotSupportedException();
        }

        #endregion
    }
}
