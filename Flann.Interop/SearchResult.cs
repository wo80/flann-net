
namespace Flann
{
    using System;

    /// <summary>
    /// Search result of a nearest-neighbours search containing the indices and distances.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SearchResult<T> where T : struct, IEquatable<T>, IFormattable
    {
        /// <summary>
        /// Gets the indices of returned nearest neighbours.
        /// </summary>
        public DataSet<int> Indices { get; }

        /// <summary>
        /// Gets the distances to the returned nearest neighbours.
        /// </summary>
        public DataSet<T> Distances { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResult{T}"/> class.
        /// </summary>
        /// <param name="rows">The number of items in the search data set.</param>
        /// <param name="count">The number of requested nearest neighbours.</param>
        public SearchResult(int rows, int count)
        {
            Indices = new DataSet<int>(rows, count);
            Distances = new DataSet<T>(rows, count);
        }
    }
}
