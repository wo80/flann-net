
namespace Flann
{
    using System;
    using System.Runtime.InteropServices;

    #region Enums (from defines.h)

    /* Nearest neighbour index algorithms */
    public enum FlannAlgorithm
    {
        Linear = 0,
        KDTree = 1,
        KMeans = 2,
        Composite = 3,
        KDTreeSingle = 4,
        Hierarchical = 5,
        LSH = 6,
        Saved = 254,
        AutoTuned = 255
    }

    public enum FlannCentersInit
    {
        Random = 0,
        Gonzales = 1,
        KMeansPP = 2,
        Groupwise = 3
    }

    public enum FlannLogLevel
    {
        None = 0,
        Fatal = 1,
        Error = 2,
        Warn = 3,
        Info = 4,
        Debug = 5
    }

    public enum FlannDistance
    {
        Euclidean = 1,
        L2 = 1,
        Manhattan = 2,
        L1 = 2,
        Minkowski = 3,
        MAX = 4,
        HistIntersect = 5,
        Hellinger = 6,
        ChiSquare = 7,
        KullbackLeibler = 8,
        Hamming = 9,
        HammingLut = 10,
        HammingPopcnt = 11,
        L2Simple = 12
    }

    public enum FlannDataType
    {
        None = -1,
        Int8 = 0,
        Int16 = 1,
        Int32 = 2,
        Int64 = 3,
        Uint8 = 4,
        Uint16 = 5,
        Uint32 = 6,
        Uint64 = 7,
        Float32 = 8,
        Float64 = 9
    }

    public enum FlannChecks
    {
        Unlimited = -1,
        AutoTuned = -2
    }

    #endregion

    static class NativeMethods
    {
        private const string FLANN_DLL = "flann";

        /// <summary>
        /// Sets the log level used for all flann functions (unless specified in FLANN parameters for each call).
        /// </summary>
        /// <param name="level">level = verbosity level</param>
        [DllImport(FLANN_DLL, EntryPoint = "flann_log_verbosity", CallingConvention = CallingConvention.Cdecl)]
        public static extern void flann_log_verbosity(int level);

        /// <summary>
        /// Sets the distance type to use throughout FLANN.
        /// </summary>
        /// <param name="distance_type">The <see cref="FlannDistance"/> type.</param>
        /// <param name="order">If distance type specified is <see cref="FlannDistance.Minkowski"/>, the second argument specifies which order the minkowski distance should have.</param>
        [DllImport(FLANN_DLL, EntryPoint = "flann_set_distance_type", CallingConvention = CallingConvention.Cdecl)]
        public static extern void flann_set_distance_type(FlannDistance distance_type, int order);

        /// <summary>
        /// Gets the distance type in use throughout FLANN.
        /// </summary>
        [DllImport(FLANN_DLL, EntryPoint = "flann_get_distance_type", CallingConvention = CallingConvention.Cdecl)]
        public static extern FlannDistance flann_get_distance_type();

        /// <summary>
        /// Gets the distance order in use throughout FLANN (only applicable if <see cref="FlannDistance.Minkowski"/> distance is in use).
        /// </summary>
        [DllImport(FLANN_DLL, EntryPoint = "flann_get_distance_order", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_get_distance_order();

        /// <summary>
        /// Builds and returns an index. It uses autotuning if the target_precision field of index_params 
        /// is between 0 and 1, or the parameters specified if it's -1.
        /// </summary>
        /// <param name="dataset">pointer to a data set stored in row major order</param>
        /// <param name="rows">number of rows (features) in the dataset</param>
        /// <param name="cols">number of columns in the dataset (feature dimensionality)</param>
        /// <param name="speedup">speedup over linear search, estimated if using autotuning, output parameter</param>
        /// <param name="flann_params">generic flann parameters</param>
        /// <returns>the newly created index or a number <0 for error</returns>
        [DllImport(FLANN_DLL, EntryPoint = "flann_build_index", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr flann_build_index(IntPtr dataset, int rows, int cols, out float speedup,
            ref FlannParameters flann_params);

#if FLANN_TYPES
        [DllImport(FLANN_DLL, EntryPoint = "flann_build_index_float", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr flann_build_index_float(IntPtr dataset, int rows, int cols, out float speedup,
            ref FlannParameters flann_params);

        [DllImport(FLANN_DLL, EntryPoint = "flann_build_index_double", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr flann_build_index_double(IntPtr dataset, int rows, int cols, out float speedup,
            ref FlannParameters flann_params);

        [DllImport(FLANN_DLL, EntryPoint = "flann_build_index_byte", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr flann_build_index_byte(IntPtr dataset, int rows, int cols, out float speedup,
            ref FlannParameters flann_params);

        [DllImport(FLANN_DLL, EntryPoint = "flann_build_index_int", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr flann_build_index_int(IntPtr dataset, int rows, int cols, out float speedup,
            ref FlannParameters flann_params);
#endif

        /// <summary>
        /// Adds points to pre-built index.
        /// </summary>
        /// <param name="index">pointer to index, must already be built</param>
        /// <param name="points">pointer to array of points</param>
        /// <param name="rows">number of points to add</param>
        /// <param name="columns">feature dimensionality</param>
        /// <param name="rebuild_threshold">reallocs index when it grows by factor of `rebuild_threshold`.
        /// A smaller value results is more space efficient but less computationally efficient. Must be greater than 1.</param>
        /// <returns>0 if success otherwise -1</returns>
        [DllImport(FLANN_DLL, EntryPoint = "flann_add_points", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_add_points(IntPtr index, IntPtr points, int rows, int columns,
            float rebuild_threshold);

#if FLANN_TYPES
        [DllImport(FLANN_DLL, EntryPoint = "flann_add_points_float", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_add_points_float(IntPtr index, IntPtr points, int rows, int columns,
            float rebuild_threshold);

        [DllImport(FLANN_DLL, EntryPoint = "flann_add_points_double", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_add_points_double(IntPtr index, IntPtr points, int rows, int columns,
            float rebuild_threshold);

        [DllImport(FLANN_DLL, EntryPoint = "flann_add_points_byte", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_add_points_byte(IntPtr index, IntPtr points, int rows, int columns,
            float rebuild_threshold);

        [DllImport(FLANN_DLL, EntryPoint = "flann_add_points_int", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_add_points_int(IntPtr index, IntPtr points, int rows, int columns,
            float rebuild_threshold);
#endif

        /// <summary>
        /// Removes a point from a pre-built index.
        /// </summary>
        /// <param name="index">pointer to pre-built index.</param>
        /// <param name="point_id">index of datapoint to remove.</param>
        /// <returns></returns>
        [DllImport(FLANN_DLL, EntryPoint = "flann_remove_point", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_remove_point(IntPtr index, uint point_id);

#if FLANN_TYPES
        [DllImport(FLANN_DLL, EntryPoint = "flann_remove_point_float", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_remove_point_float(IntPtr index, uint point_id);

        [DllImport(FLANN_DLL, EntryPoint = "flann_remove_point_double", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_remove_point_double(IntPtr index, uint point_id);

        [DllImport(FLANN_DLL, EntryPoint = "flann_remove_point_byte", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_remove_point_byte(IntPtr index, uint point_id);

        [DllImport(FLANN_DLL, EntryPoint = "flann_remove_point_int", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_remove_point_int(IntPtr index, uint point_id);
#endif

        /// <summary>
        /// Gets a point from a given index position.
        /// </summary>
        /// <param name="index">pointer to pre-built index.</param>
        /// <param name="point_id">index of datapoint to get.</param>
        /// <returns>pointer to datapoint or NULL on miss</returns>
        [DllImport(FLANN_DLL, EntryPoint = "flann_get_point", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr flann_get_point(IntPtr index, uint point_id);

#if FLANN_TYPES
        [DllImport(FLANN_DLL, EntryPoint = "flann_get_point_float", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr flann_get_point_float(IntPtr index, uint point_id);

        [DllImport(FLANN_DLL, EntryPoint = "flann_get_point_double", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr flann_get_point_double(IntPtr index, uint point_id);

        [DllImport(FLANN_DLL, EntryPoint = "flann_get_point_byte", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr flann_get_point_byte(IntPtr index, uint point_id);

        [DllImport(FLANN_DLL, EntryPoint = "flann_get_point_int", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr flann_get_point_int(IntPtr index, uint point_id);
#endif

        /// <summary>
        /// Returns the number of datapoints stored in index.
        /// </summary>
        /// <param name="index">pointer to pre-built index.</param>
        /// <returns></returns>
        [DllImport(FLANN_DLL, EntryPoint = "flann_veclen", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint flann_veclen(IntPtr index);

#if FLANN_TYPES
        [DllImport(FLANN_DLL, EntryPoint = "flann_veclen_float", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint flann_veclen_float(IntPtr index);

        [DllImport(FLANN_DLL, EntryPoint = "flann_veclen_double", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint flann_veclen_double(IntPtr index);

        [DllImport(FLANN_DLL, EntryPoint = "flann_veclen_byte", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint flann_veclen_byte(IntPtr index);

        [DllImport(FLANN_DLL, EntryPoint = "flann_veclen_int", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint flann_veclen_int(IntPtr index);
#endif

        /// <summary>
        /// Returns the dimensionality of datapoints stored in index.
        /// </summary>
        /// <param name="index">pointer to pre-built index.</param>
        /// <returns></returns>
        [DllImport(FLANN_DLL, EntryPoint = "flann_size", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint flann_size(IntPtr index);

#if FLANN_TYPES
        [DllImport(FLANN_DLL, EntryPoint = "flann_size_float", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint flann_size_float(IntPtr index);

        [DllImport(FLANN_DLL, EntryPoint = "flann_size_double", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint flann_size_double(IntPtr index);

        [DllImport(FLANN_DLL, EntryPoint = "flann_size_byte", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint flann_size_byte(IntPtr index);

        [DllImport(FLANN_DLL, EntryPoint = "flann_size_int", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint flann_size_int(IntPtr index);
#endif

        /// <summary>
        /// Returns the number of bytes consumed by the index.
        /// </summary>
        /// <param name="index">pointer to pre-built index.</param>
        /// <returns></returns>
        [DllImport(FLANN_DLL, EntryPoint = "flann_used_memory", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_used_memory(IntPtr index);

#if FLANN_TYPES
        [DllImport(FLANN_DLL, EntryPoint = "flann_used_memory_float", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_used_memory_float(IntPtr index);

        [DllImport(FLANN_DLL, EntryPoint = "flann_used_memory_double", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_used_memory_double(IntPtr index);

        [DllImport(FLANN_DLL, EntryPoint = "flann_used_memory_byte", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_used_memory_byte(IntPtr index);

        [DllImport(FLANN_DLL, EntryPoint = "flann_used_memory_int", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_used_memory_int(IntPtr index);
#endif

        /// <summary>
        /// Saves the index to a file. Only the index is saved into the file, the dataset corresponding to the index is not saved.
        /// </summary>
        /// <param name="index_id">The index that should be saved</param>
        /// <param name="filename">The filename the index should be saved to</param>
        /// <returns>Returns 0 on success, negative value on error.</returns>
        [DllImport(FLANN_DLL, EntryPoint = "flann_save_index", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_save_index(IntPtr index_id, [MarshalAs(UnmanagedType.LPStr)] string filename);

#if FLANN_TYPES
        [DllImport(FLANN_DLL, EntryPoint = "flann_save_index_float", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_save_index_float(IntPtr index_id, [MarshalAs(UnmanagedType.LPStr)] string filename);

        [DllImport(FLANN_DLL, EntryPoint = "flann_save_index_double", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_save_index_double(IntPtr index_id, [MarshalAs(UnmanagedType.LPStr)] string filename);

        [DllImport(FLANN_DLL, EntryPoint = "flann_save_index_byte", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_save_index_byte(IntPtr index_id, [MarshalAs(UnmanagedType.LPStr)] string filename);

        [DllImport(FLANN_DLL, EntryPoint = "flann_save_index_int", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_save_index_int(IntPtr index_id, [MarshalAs(UnmanagedType.LPStr)] string filename);
#endif

        /// <summary>
        /// Loads an index from a file.
        /// </summary>
        /// <param name="filename">File to load the index from.</param>
        /// <param name="dataset">The dataset corresponding to the index.</param>
        /// <param name="rows">Dataset tors</param>
        /// <param name="cols">Dataset columns</param>
        /// <returns></returns>
        [DllImport(FLANN_DLL, EntryPoint = "flann_load_index", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr flann_load_index([MarshalAs(UnmanagedType.LPStr)] string filename, IntPtr dataset, int rows, int cols);

#if FLANN_TYPES
        [DllImport(FLANN_DLL, EntryPoint = "flann_load_index_float", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr flann_load_index_float([MarshalAs(UnmanagedType.LPStr)] string filename, IntPtr dataset, int rows, int cols);

        [DllImport(FLANN_DLL, EntryPoint = "flann_load_index_double", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr flann_load_index_double([MarshalAs(UnmanagedType.LPStr)] string filename, IntPtr dataset, int rows, int cols);

        [DllImport(FLANN_DLL, EntryPoint = "flann_load_index_byte", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr flann_load_index_byte([MarshalAs(UnmanagedType.LPStr)] string filename, IntPtr dataset, int rows, int cols);

        [DllImport(FLANN_DLL, EntryPoint = "flann_load_index_int", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr flann_load_index_int([MarshalAs(UnmanagedType.LPStr)] string filename, IntPtr dataset, int rows, int cols);
#endif

        /// <summary>
        /// Builds an index and uses it to find nearest neighbors.
        /// </summary>
        /// <param name="dataset">pointer to a data set stored in row major order</param>
        /// <param name="rows">number of rows (features) in the dataset</param>
        /// <param name="cols">number of columns in the dataset (feature dimensionality)</param>
        /// <param name="testset">pointer to a query set stored in row major order</param>
        /// <param name="trows">number of rows (features) in the query dataset (same dimensionality as features in the dataset)</param>
        /// <param name="indices">pointer to matrix for the indices of the nearest neighbors of the testset features in the dataset
        /// (must have trows number of rows and nn number of columns)</param>
        /// <param name="dists">pointer to matrix for the distances of the nearest neighbors of the testset features in the dataset
        /// (must have trows number of rows and 1 column)</param>
        /// <param name="nn">how many nearest neighbors to return</param>
        /// <param name="flann_params">generic flann parameters</param>
        /// <returns>zero or -1 for error</returns>
        [DllImport(FLANN_DLL, EntryPoint = "flann_find_nearest_neighbors", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_find_nearest_neighbors(IntPtr dataset, int rows, int cols, IntPtr testset, int trows,
            IntPtr indices, IntPtr dists, int nn, ref FlannParameters flann_params);

#if FLANN_TYPES
        [DllImport(FLANN_DLL, EntryPoint = "flann_find_nearest_neighbors_float", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_find_nearest_neighbors_float(IntPtr dataset, int rows, int cols, IntPtr testset, int trows,
            IntPtr indices, IntPtr dists, int nn, ref FlannParameters flann_params);

        [DllImport(FLANN_DLL, EntryPoint = "flann_find_nearest_neighbors_double", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_find_nearest_neighbors_double(IntPtr dataset, int rows, int cols, IntPtr testset, int trows,
            IntPtr indices, IntPtr dists, int nn, ref FlannParameters flann_params);

        [DllImport(FLANN_DLL, EntryPoint = "flann_find_nearest_neighbors_byte", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_find_nearest_neighbors_byte(IntPtr dataset, int rows, int cols, IntPtr testset, int trows,
            IntPtr indices, IntPtr dists, int nn, ref FlannParameters flann_params);

        [DllImport(FLANN_DLL, EntryPoint = "flann_find_nearest_neighbors_int", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_find_nearest_neighbors_int(IntPtr dataset, int rows, int cols, IntPtr testset, int trows,
            IntPtr indices, IntPtr dists, int nn, ref FlannParameters flann_params);
#endif

        /// <summary>
        /// Searches for nearest neighbors using the index provided
        /// </summary>
        /// <param name="index_id">the index (constructed previously using flann_build_index).</param>
        /// <param name="testset">pointer to a query set stored in row major order</param>
        /// <param name="trows">number of rows (features) in the query dataset (same dimensionality as features in the dataset)</param>
        /// <param name="indices">pointer to matrix for the indices of the nearest neighbors of the testset features in the dataset
        /// (must have trows number of rows and nn number of columns)</param>
        /// <param name="dists">pointer to matrix for the distances of the nearest neighbors of the testset features in the dataset
        /// (must have trows number of rows and 1 column)</param>
        /// <param name="nn">how many nearest neighbors to return</param>
        /// <param name="flann_params">generic flann parameters</param>
        /// <returns>zero or a number <0 for error</returns>
        [DllImport(FLANN_DLL, EntryPoint = "flann_find_nearest_neighbors_index", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_find_nearest_neighbors_index(IntPtr index_id, IntPtr testset, int trows, IntPtr indices,
            IntPtr dists, int nn, ref FlannParameters flann_params);

#if FLANN_TYPES
        [DllImport(FLANN_DLL, EntryPoint = "flann_find_nearest_neighbors_index_float", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_find_nearest_neighbors_index_float(IntPtr index_id, IntPtr testset, int trows, IntPtr indices,
            IntPtr dists, int nn, ref FlannParameters flann_params);

        [DllImport(FLANN_DLL, EntryPoint = "flann_find_nearest_neighbors_index_double", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_find_nearest_neighbors_index_double(IntPtr index_id, IntPtr testset, int trows, IntPtr indices,
            IntPtr dists, int nn, ref FlannParameters flann_params);

        [DllImport(FLANN_DLL, EntryPoint = "flann_find_nearest_neighbors_index_byte", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_find_nearest_neighbors_index_byte(IntPtr index_id, IntPtr testset, int trows, IntPtr indices,
            IntPtr dists, int nn, ref FlannParameters flann_params);

        [DllImport(FLANN_DLL, EntryPoint = "flann_find_nearest_neighbors_index_int", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_find_nearest_neighbors_index_int(IntPtr index_id, IntPtr testset, int trows, IntPtr indices,
            IntPtr dists, int nn, ref FlannParameters flann_params);
#endif

        /// <summary>
        /// Performs an radius search using an already constructed index.
        /// </summary>
        /// <param name="index">the index</param>
        /// <param name="query">query point</param>
        /// <param name="indices">array for storing the indices found (will be modified)</param>
        /// <param name="dists">similar, but for storing distances</param>
        /// <param name="max_nn">size of arrays indices and dists</param>
        /// <param name="radius">search radius (squared radius for euclidian metric)</param>
        /// <param name="flann_params"></param>
        /// <returns></returns>
        /// <remarks>
        /// In case of radius search, instead of always returning a predetermined
        /// number of nearest neighbours (for example the 10 nearest neighbours), the
        /// search will return all the neighbours found within a search radius
        /// of the query point.
        ///
        /// The check parameter in the FLANNParameters below sets the level of approximation
        /// for the search by only visiting "checks" number of features in the index
        /// (the same way as for the KNN search). A lower value for checks will give
        /// a higher search speedup at the cost of potentially not returning all the
        /// neighbours in the specified radius.
        /// </remarks>
        [DllImport(FLANN_DLL, EntryPoint = "flann_radius_search", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_radius_search(IntPtr index, IntPtr query, IntPtr indices, IntPtr dists,
            int max_nn, float radius, ref FlannParameters flann_params);

#if FLANN_TYPES
        [DllImport(FLANN_DLL, EntryPoint = "flann_radius_search_float", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_radius_search_float(IntPtr index, IntPtr query, IntPtr indices, IntPtr dists,
            int max_nn, float radius, ref FlannParameters flann_params);

        [DllImport(FLANN_DLL, EntryPoint = "flann_radius_search_double", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_radius_search_double(IntPtr index, IntPtr query, IntPtr indices, IntPtr dists,
            int max_nn, float radius, ref FlannParameters flann_params);

        [DllImport(FLANN_DLL, EntryPoint = "flann_radius_search_byte", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_radius_search_byte(IntPtr index, IntPtr query, IntPtr indices, IntPtr dists,
            int max_nn, float radius, ref FlannParameters flann_params);

        [DllImport(FLANN_DLL, EntryPoint = "flann_radius_search_int", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_radius_search_int(IntPtr index, IntPtr query, IntPtr indices, IntPtr dists,
            int max_nn, float radius, ref FlannParameters flann_params);
#endif

        /// <summary>
        /// Deletes an index and releases the memory used by it.
        /// </summary>
        /// <param name="index_id">the index (constructed previously using flann_build_index).</param>
        /// <param name="flann_params">generic flann parameters</param>
        /// <returns>zero or a number <0 for error</returns>
        [DllImport(FLANN_DLL, EntryPoint = "flann_free_index", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_free_index(IntPtr index_id, ref FlannParameters flann_params);

#if FLANN_TYPES
        [DllImport(FLANN_DLL, EntryPoint = "flann_free_index_float", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_free_index_float(IntPtr index_id, ref FlannParameters flann_params);

        [DllImport(FLANN_DLL, EntryPoint = "flann_free_index_double", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_free_index_double(IntPtr index_id, ref FlannParameters flann_params);

        [DllImport(FLANN_DLL, EntryPoint = "flann_free_index_byte", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_free_index_byte(IntPtr index_id, ref FlannParameters flann_params);

        [DllImport(FLANN_DLL, EntryPoint = "flann_free_index_int", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_free_index_int(IntPtr index_id, ref FlannParameters flann_params);
#endif

        /// <summary>
        ///  Clusters the features in the dataset using a hierarchical kmeans clustering approach.
        ///  This is significantly faster than using a flat kmeans clustering for a large number of clusters.
        /// </summary>
        /// <param name="dataset">pointer to a data set stored in row major order</param>
        /// <param name="rows">number of rows (features) in the dataset</param>
        /// <param name="cols">number of columns in the dataset (feature dimensionality)</param>
        /// <param name="clusters">number of cluster to compute</param>
        /// <param name="result">memory buffer where the output cluster centers are storred</param>
        /// <param name="flann_params">generic flann parameters</param>
        /// <returns>number of clusters computed or a number <0 for error.</returns>
        /// <remarks>
        /// The returned number of clusters can be different than the number of clusters requested, due to the
        /// way hierarchical clusters are computed. The number of clusters returned will be the highest number
        /// of the form (branch_size-1)*K+1 smaller than the number of clusters requested.
        /// </remarks>
        [DllImport(FLANN_DLL, EntryPoint = "flann_compute_cluster_centers", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_compute_cluster_centers(IntPtr dataset, int rows, int cols, int clusters,
            IntPtr result, ref FlannParameters flann_params);

#if FLANN_TYPES
        [DllImport(FLANN_DLL, EntryPoint = "flann_compute_cluster_centers_float", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_compute_cluster_centers_float(IntPtr dataset, int rows, int cols, int clusters,
            IntPtr result, ref FlannParameters flann_params);

        [DllImport(FLANN_DLL, EntryPoint = "flann_compute_cluster_centers_double", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_compute_cluster_centers_double(IntPtr dataset, int rows, int cols, int clusters,
            IntPtr result, ref FlannParameters flann_params);

        [DllImport(FLANN_DLL, EntryPoint = "flann_compute_cluster_centers_byte", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_compute_cluster_centers_byte(IntPtr dataset, int rows, int cols, int clusters,
            IntPtr result, ref FlannParameters flann_params);

        [DllImport(FLANN_DLL, EntryPoint = "flann_compute_cluster_centers_int", CallingConvention = CallingConvention.Cdecl)]
        public static extern int flann_compute_cluster_centers_int(IntPtr dataset, int rows, int cols, int clusters,
            IntPtr result, ref FlannParameters flann_params);
#endif
    }
}
