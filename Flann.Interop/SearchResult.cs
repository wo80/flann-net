
namespace Flann
{
    using System;

    public class SearchResult<T> where T : struct, IEquatable<T>, IFormattable
    {
        /// <summary>
        /// Gets the number of items in the search data set.
        /// </summary>
        public int Rows { get; internal set; }

        /// <summary>
        /// Gets the number of requested nearest neighbours.
        /// </summary>
        public int Count { get; internal set; }

        /// <summary>
        /// Gets the raw array representation of the indices of returned nearest neighbours (row major storage).
        /// </summary>
        /// <remarks>
        /// If there are K vectors in the search data set, and for each vector N nearest neighbours
        /// were requested, then the indices of the found nearest neighbour for the i-th vector are
        /// stored at positions <c>i * N</c> to <c>(i + 1) * N</c>. 
        /// </remarks>
        public DataSet<int> Indices { get; internal set; }

        /// <summary>
        /// Gets the raw array representation of the distances to the returned nearest neighbours (row major storage).
        /// </summary>
        /// <remarks>
        /// If there are K vectors in the search data set, and for each vector N nearest neighbours
        /// were requested, then the indices of the found nearest neighbour for the i-th vector are
        /// stored at positions <c>i * N</c> to <c>(i + 1) * N</c>. 
        /// </remarks>
        public DataSet<T> Distances { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rows">The number of items in the search data set.</param>
        /// <param name="count">The number of requested nearest neighbours.</param>
        public SearchResult(int rows, int count)
        {
            this.Rows = rows;
            this.Count = count;

            Indices = new DataSet<int>(rows, count);
            Distances = new DataSet<T>(rows, count);
        }
    }
}
