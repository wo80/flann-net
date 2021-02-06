
namespace Flann
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class Index : IDisposable
    {
        private IntPtr index;

        private readonly int rows;
        private readonly int columns;

        private FlannParameters fp;

        private Index(FlannParameters fp, int rows, int columns)
        {
            this.fp = fp;
            this.rows = rows;
            this.columns = columns;
        }

        #region Static methods

        /// <summary>
        /// Creates an <see cref="Index"/> for the given data set with default FLANN parameters.
        /// </summary>
        /// <param name="data">The data set.</param>
        public static Index Create(DataSet<float> data)
        {
            return Create(data, FlannParameters.Default());
        }

        /// <summary>
        /// Loads an <see cref="Index"/> from file associated to given data set.
        /// </summary>
        /// <param name="file">The file path.</param>
        /// <param name="data">The data set.</param>
        public static Index Load(string file, DataSet<float> data)
        {
            int rows = data.Rows;
            int cols = data.Columns;

            var flann = new Index(FlannParameters.Default(), rows, cols);

            var h = GCHandle.Alloc(data.Data, GCHandleType.Pinned);

            flann.index = NativeMethods.flann_load_index(file, h.AddrOfPinnedObject(), rows, cols);

            h.Free();

            return flann;
        }

        #endregion

        /// <summary>
        /// Creates an <see cref="Index"/> for the given data set.
        /// </summary>
        /// <param name="data">The data set.</param>
        /// <param name="fp">The FLANN parameters.</param>
        public static Index Create(DataSet<float> data, FlannParameters fp)
        {
            int rows = data.Rows;
            int cols = data.Columns;

            var flann = new Index(fp, rows, cols);

            var h = GCHandle.Alloc(data.Data, GCHandleType.Pinned);

            flann.index = NativeMethods.flann_build_index(h.AddrOfPinnedObject(), rows, cols, out _, ref fp);

            h.Free();

            return flann;
        }

        ~Index() => Dispose(false);

        /// <summary>
        /// Find <paramref name="n"/> nearest neighbours of the given vector.
        /// </summary>
        /// <param name="item">The vector to search for.</param>
        /// <param name="n">The number of nearest neighbours to return.</param>
        /// <returns></returns>
        public SearchResult<float> FindNearestNeighbors(float[] item, int n)
        {
            if (item.Length != columns)
            {
                throw new ArgumentException("Invalid vector dimension.", nameof(item));
            }

            var data = new DataSet<float>(item);

            return FindNearestNeighbors(data, n);
        }

        /// <summary>
        /// Find <paramref name="n"/> nearest neighbours for each vector in the given data set.
        /// </summary>
        /// <param name="items">The search data set (each row corresponds to a vector to search for).</param>
        /// <param name="n">The number of nearest neighbours to return for each vector.</param>
        /// <returns></returns>
        public SearchResult<float> FindNearestNeighbors(DataSet<float> items, int n)
        {
            var result = new SearchResult<float>(items.Rows, n);

            var list = new List<GCHandle>();

            var data = InteropHelper.Pin(items.Data, list);
            var indices = InteropHelper.Pin(result.Indices, list);
            var distances = InteropHelper.Pin(result.Distances, list);

            NativeMethods.flann_find_nearest_neighbors_index(index, data, rows, indices, distances, n, ref fp);

            InteropHelper.Free(list);

            return result;
        }

        /// <summary>
        /// Saves an <see cref="Index"/> to given file.
        /// </summary>
        /// <param name="file">The file path.</param>
        public void Save(string file)
        {
            NativeMethods.flann_save_index(index, file);
        }

        #region IDisposable

        // See https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose

        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
            }

            if (index != IntPtr.Zero)
            {
                NativeMethods.flann_free_index(index, ref fp);
            }

            _disposed = true;
        }

        #endregion
    }
}
