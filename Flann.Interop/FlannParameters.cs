
namespace Flann
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct FlannParameters
    {
        /// <summary>
        /// The FLANN algorithm to use.
        /// </summary>
        public FlannAlgorithm algorithm;

        #region search time parameters

        /// <summary>
        /// How many leafs (features) to check in one search.
        /// </summary>
        public int checks;

        /// <summary>
        /// eps parameter for eps-knn search.
        /// </summary>
        public float eps;

        /// <summary>
        /// Indicates if results returned by radius search should be sorted or not.
        /// </summary>
        public int sorted;

        /// <summary>
        /// Limits the maximum number of neighbors should be returned by radius search.
        /// </summary>
        public int maxNeighbors;

        /// <summary>
        /// Number of parallel cores to use for searching.
        /// </summary>
        public int cores;

        #endregion

        #region kdtree index parameters

        /// <summary>
        /// Number of randomized trees to use (for kdtree).
        /// </summary>
        public int trees;

        /// <summary>
        /// 
        /// </summary>
        public int leafMaxSize;

        #endregion

        #region kmeans index parameters

        /// <summary>
        /// Branching factor (for kmeans tree).
        /// </summary>
        public int branching;

        /// <summary>
        /// Max iterations to perform in one kmeans cluetering (kmeans tree).
        /// </summary>
        public int iterations;

        /// <summary>
        /// Algorithm used for picking the initial cluster centers for kmeans tree.
        /// </summary>
        public FlannCentersInit centersInit;

        /// <summary>
        /// Cluster boundary index. Used when searching the kmeans tree.
        /// </summary>
        public float cbIndex;

        #endregion

        #region autotuned index parameters

        /// <summary>
        /// Precision desired (used for autotuning, -1 otherwise).
        /// </summary>
        public float targetPrecision;

        /// <summary>
        /// Build tree time weighting factor.
        /// </summary>
        public float buildWeight;

        /// <summary>
        /// Index memory weigthing factor.
        /// </summary>
        public float memoryWeight;

        #endregion

        #region LSH parameters

        /// <summary>
        /// What fraction of the dataset to use for autotuning.
        /// </summary>
        public float sampleFraction;

        /// <summary>
        /// The number of hash tables to use.
        /// </summary>
        public int tableNumber;

        /// <summary>
        /// The length of the key in the hash tables.
        /// </summary>
        public int keySize;

        /// <summary>
        /// Number of levels to use in multi-probe LSH, 0 for standard LSH.
        /// </summary>
        public int multiProbeLevel;

        #endregion

        #region other parameters

        /// <summary>
        /// Determines the verbosity of each flann function.
        /// </summary>
        public FlannLogLevel logLevel;

        /// <summary>
        /// Random seed to use.
        /// </summary>
        public int randomSeed;

        #endregion

        public static FlannParameters Default()
        {
            var fp = new FlannParameters();

            fp.algorithm = FlannAlgorithm.KDTree;
            fp.checks = 32;
            fp.eps = 0.0f;
            fp.sorted = 1;
            fp.maxNeighbors = -1;
            fp.cores = 0;
            fp.trees = 1;
            fp.leafMaxSize = 4;
            fp.branching = 32;
            fp.iterations = 5;
            fp.centersInit = FlannCentersInit.Random;
            fp.cbIndex = 0.5f;
            fp.targetPrecision = 0.9f;
            fp.buildWeight = 0.01f;
            fp.memoryWeight = 0.0f;
            fp.sampleFraction = 0.1f;
            fp.tableNumber = 12;
            fp.keySize = 20;
            fp.multiProbeLevel = 2;
            fp.logLevel = FlannLogLevel.None;
            fp.randomSeed = -1;

            return fp;
        }
    }
}
