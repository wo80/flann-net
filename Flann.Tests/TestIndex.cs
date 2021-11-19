using NUnit.Framework;

namespace Flann.Tests
{
    public class TestIndex
    {
        // Data set containing 3 vectors (dimension 5).
        private static readonly DataSet<float> dataset = new DataSet<float>(3, 5, new[] {
                  1.0f,   1.0f,  1.0f,  2.0f, 3.0f,
                 10.0f,  10.0f, 10.0f,  3.0f, 2.0f,
                100.0f, 100.0f,  2.0f, 30.0f, 1.0f
            });

        // Search for 2 vectors.
        private static readonly DataSet<float> testset = new DataSet<float>(2, 5, new[] {
                  1.0f,  1.0f,  1.0f,  1.0f, 1.0f,
                 90.0f, 90.0f, 10.0f, 10.0f, 1.0f
            });

        [Test]
        public void TestFindNearestNeighbors()
        {
            using (var index = Index.Create(dataset))
            {
                // Find 2 nearest neighbours for each vector in the test set.
                var result = index.FindNearestNeighbors(testset, 2);

                var indices = result.Indices.GetRow(0);

                Assert.AreEqual(0, indices[0]);
                Assert.AreEqual(1, indices[1]);

                indices = result.Indices.GetRow(1);

                Assert.AreEqual(2, indices[0]);
                Assert.AreEqual(1, indices[1]);
            }
        }

        [Test]
        public void TestSerialization()
        {
            using (var index = Index.Create(dataset))
            {
                // Save data and index.
                dataset.Serialize("testdata.dat");
                index.Save("testdata.idx");
            }

            var dataset2 = DataSet<float>.Deserialize("testdata.dat");

            using (var index = Index.Load("testdata.idx", dataset2))
            {
                // Find 2 nearest neighbours for each vector in the test set.
                var result = index.FindNearestNeighbors(testset, 2);

                var indices = result.Indices.GetRow(0);

                Assert.AreEqual(0, indices[0]);
                Assert.AreEqual(1, indices[1]);

                indices = result.Indices.GetRow(1);

                Assert.AreEqual(2, indices[0]);
                Assert.AreEqual(1, indices[1]);
            }
        }

        [Test]
        public void TestSerializationMap()
        {
            var indexMap = new int[] { 3, 2, 1 };

            var dataset2 = new DataSet<float>(dataset.Rows, dataset.Columns, dataset.Data, indexMap);

            dataset2.Serialize("testdata2.dat");

            var dataset3 = DataSet<float>.Deserialize("testdata2.dat");

            CollectionAssert.AreEqual(dataset2.Data, dataset3.Data);
            CollectionAssert.AreEqual(dataset2.IndexMap, dataset3.IndexMap);
        }
    }
}