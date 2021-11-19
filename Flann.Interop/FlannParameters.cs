
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
        /// How many leafs (features) to check in one search (-1 for unlimited).
        /// </summary>
        public int checks;

        /// <summary>
        /// eps parameter for eps-knn search (default: 0).
        /// </summary>
        public float eps;

        /// <summary>
        /// Indicates if results returned by radius search should be sorted or not (default: true).
        /// </summary>
        public int sorted;

        /// <summary>
        /// Limits the maximum number of neighbors should be returned by radius search (-1 for unlimited).
        /// </summary>
        public int maxNeighbors;

        /// <summary>
        /// Number of parallel cores to use for searching (0 for auto).
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

        /// <summary>
        /// What fraction of the dataset to use for autotuning.
        /// </summary>
        public float sampleFraction;

        #endregion

        #region LSH parameters

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

            // from util/params.h

            fp.checks = 32;
            fp.eps = 0f;
            fp.sorted = 1;
            fp.maxNeighbors = -1;

            // Setting cores to 1 instead of 0 (default) to avoid error if FLANN is compiled
            // with OMP: "argument to num_threads clause must be positive".
            fp.cores = 1;

            // from algorithms/kdtree_index.h

            fp.trees = 4;

            // from algorithms/kdtree_single_index.h

            fp.leafMaxSize = 10;

            // from algorithms/kmeans_index.h

            fp.branching = 32;
            fp.iterations = 11;
            fp.centersInit = FlannCentersInit.Random;
            fp.cbIndex = 0.2f;

            // from algorithms/autotuned_index.h

            fp.targetPrecision = 0.8f;
            fp.buildWeight = 0.01f;
            fp.memoryWeight = 0f;
            fp.sampleFraction = 0.1f;

            // from algorithms/lsh_index.h

            fp.tableNumber = 12;
            fp.keySize = 20;
            fp.multiProbeLevel = 2;

            // other

            fp.logLevel = FlannLogLevel.None;
            fp.randomSeed = -1;

            return fp;
        }
    }
}
