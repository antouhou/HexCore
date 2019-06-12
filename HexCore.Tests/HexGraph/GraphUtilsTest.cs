using HexCore.DataStructures;
using HexCore.HexGraph;
using HexCoreTests.Mocks;
using NUnit.Framework;

namespace HexCoreTests.HexGraph
{
    [TestFixture]
    public class GraphUtilsTest

    {
        [Test]
        public void AllCellsShouldHaveCorrectPositions()
        {
            var width = 6;
            var height = 7;
            var graph = GraphFactory.CreateRectangularGraph(width, height, OffsetTypes.OddRowsRight,
                MovementTypes.Ground);

            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));

            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(new Coordinate2D(x, y, OffsetTypes.OddRowsRight).To3D()), Is.True);

            width = 4;
            height = 5;
            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, width, height, MovementTypes.Ground);

            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(new Coordinate2D(x, y, OffsetTypes.OddRowsRight).To3D()), Is.True);

            width = 7;
            height = 8;
            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, width, height, MovementTypes.Ground);

            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(new Coordinate2D(x, y, OffsetTypes.OddRowsRight).To3D()), Is.True);
        }

        [Test]
        public void ShouldIncreaseHeight()
        {
            var width = 3;
            var height = 3;
            var graph = GraphFactory.CreateRectangularGraph(width, height, OffsetTypes.OddRowsRight,
                MovementTypes.Ground);

            height = 4;

            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, width, height, MovementTypes.Ground);
            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(new Coordinate2D(x, y, OffsetTypes.OddRowsRight).To3D()), Is.True);
        }

        [Test]
        public void ShouldIncreaseWidth()
        {
            var width = 4;
            var height = 3;
            var graph = GraphFactory.CreateRectangularGraph(width, height, OffsetTypes.OddRowsRight,
                MovementTypes.Ground);

            width = 5;

            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, width, height, MovementTypes.Ground);
            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(new Coordinate2D(x, y, OffsetTypes.OddRowsRight).To3D()), Is.True);
        }

        [Test]
        public void ShouldIncreaseWidthAndHeight()
        {
            var width = 3;
            var height = 4;
            var graph = GraphFactory.CreateRectangularGraph(width, height, OffsetTypes.OddRowsRight,
                MovementTypes.Ground);

            width = 4;
            height = 5;

            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, width, height, MovementTypes.Ground);
            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(new Coordinate2D(x, y, OffsetTypes.OddRowsRight).To3D()), Is.True);
        }

        [Test]
        public void ShouldMaintainCellStatesOnResize()
        {
            var graph = GraphFactory.CreateRectangularGraph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.Ground);
            Assert.False(graph.IsCellBlocked(new Coordinate2D(0, 1, OffsetTypes.OddRowsRight).To3D()));
            graph.SetOneCellBlocked(new Coordinate2D(0, 1, OffsetTypes.OddRowsRight).To3D(), true);
            Assert.True(graph.IsCellBlocked(new Coordinate2D(0, 1, OffsetTypes.OddRowsRight).To3D()));
            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, 2, 2, MovementTypes.Ground);
            Assert.True(graph.IsCellBlocked(new Coordinate2D(0, 1, OffsetTypes.OddRowsRight).To3D()));
        }

        [Test]
        public void ShouldNotResizeIfNothingChanged()
        {
            var width = 3;
            var height = 3;
            var graph = GraphFactory.CreateRectangularGraph(width, height, OffsetTypes.OddRowsRight,
                MovementTypes.Ground);

            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, width, height, MovementTypes.Ground);
            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(new Coordinate2D(x, y, OffsetTypes.OddRowsRight).To3D()), Is.True);
        }

        [Test]
        public void ShouldReduceHeight()
        {
            var width = 3;
            var height = 3;
            var graph = GraphFactory.CreateRectangularGraph(width, height, OffsetTypes.OddRowsRight,
                MovementTypes.Ground);

            height = 2;

            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, width, height, MovementTypes.Ground);
            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(new Coordinate2D(x, y, OffsetTypes.OddRowsRight).To3D()), Is.True);
        }

        [Test]
        public void ShouldReduceWidth()
        {
            var width = 3;
            var height = 3;
            var graph = GraphFactory.CreateRectangularGraph(width, height, OffsetTypes.OddRowsRight,
                MovementTypes.Ground);

            width = 2;

            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, width, height, MovementTypes.Ground);
            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(new Coordinate2D(x, y, OffsetTypes.OddRowsRight).To3D()), Is.True);
        }

        [Test]
        public void ShouldReduceWidthAndHeight()
        {
            var width = 3;
            var height = 3;
            var graph = GraphFactory.CreateRectangularGraph(width, height, OffsetTypes.OddRowsRight,
                MovementTypes.Ground);

            width = 2;
            height = 2;

            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, width, height, MovementTypes.Ground);
            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(new Coordinate2D(x, y, OffsetTypes.OddRowsRight).To3D()), Is.True);
        }
    }
}