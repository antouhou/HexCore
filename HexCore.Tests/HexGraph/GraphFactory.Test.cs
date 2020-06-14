using HexCore.DataStructures;
using HexCore.HexGraph;
using HexCoreTests.Fixtures;
using NUnit.Framework;

namespace HexCoreTests.HexGraph
{
    [TestFixture]
    public class GraphFactoryTest
    {
        [Test]
        public void CreateRectangularGraph_ShouldCreateSquareGraphWithOptionalParameters()
        {
            const int width = 4;
            const int height = 3;
            var graph = GraphFactory.CreateRectangularGraph(width, height, MovementTypesFixture.GetMovementTypes(),
                MovementTypesFixture.Ground, OffsetTypes.OddRowsRight);
            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));

            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(new Coordinate2D(x, y, OffsetTypes.OddRowsRight).To3D()), Is.True);
        }

        [Test]
        public void CreateRectangularGraph_ShouldCreateSquareGraphWithoutOptionalParameters()
        {
            const int width = 4;
            const int height = 3;
            var graph = GraphFactory.CreateRectangularGraph(width, height, MovementTypesFixture.GetMovementTypes(), MovementTypesFixture.Ground);
            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));

            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(new Coordinate2D(x, y, OffsetTypes.OddRowsRight).To3D()), Is.True);
        }
    }
}