using HexCore.DataStructures;
using HexCore.HexGraph;
using NUnit.Framework;
using Tests.Fixtures;

namespace Tests.HexGraph
{
    [TestFixture]
    public class GraphFactoryTest
    {
        [Test]
        public void ShouldCreateSquareGraphWithOptionalParameters()
        {
            const int width = 4;
            const int height = 3;
            var graph = GraphFactory.CreateSquareGraph(width, height, OffsetTypes.OddRowsRight, MovementTypes.Ground);
            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));

            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(new Coordinate2D(x, y, OffsetTypes.OddRowsRight).To3D()), Is.True);
        }

        [Test]
        public void ShouldCreateSquareGraphWithoutOptionalParameters()
        {
            const int width = 4;
            const int height = 3;
            var graph = GraphFactory.CreateSquareGraph(width, height);
            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));

            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(new Coordinate2D(x, y, OffsetTypes.OddRowsRight).To3D()), Is.True);
        }
    }
}