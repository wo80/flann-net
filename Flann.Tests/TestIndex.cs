using NUnit.Framework;

namespace Flann.Tests
{
    public class TestIndex
    {
        [Test]
        public void Test1()
        {
            // Data set containing 3 vectors (dimension 5).
            var dataset = new DataSet<float>(3, 5, new[] {
                  1.0f,   1.0f,  1.0f,  2.0f, 3.0f,
                 10.0f,  10.0f, 10.0f,  3.0f, 2.0f,
                100.0f, 100.0f,  2.0f, 30.0f, 1.0f
            });

            // Search for 2 vectors.
            var testset = new DataSet<float>(2, 5, new[] {
                  1.0f,  1.0f,  1.0f,  1.0f, 1.0f,
                 90.0f, 90.0f, 10.0f, 10.0f, 1.0f
            });

            using (var index = Index.Create(dataset))
            {
                // Find 2 nearest neighbours for each vector in the test set.
                var result = index.FindNearestNeighbors(testset, 2);

                index.Save("testdata.flann");
            }

            using (var index = Index.Load("testdata.flann", dataset))
            {
                // Find 2 nearest neighbours for each vector in the test set.
                var result = index.FindNearestNeighbors(testset, 2);
            }
        }
    }
}