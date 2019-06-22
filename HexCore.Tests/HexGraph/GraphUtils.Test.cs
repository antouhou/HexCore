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
        public void ResizeSquareGraph_AllCellsShouldHaveCorrectPositionsAfterResize()
        {
            var width = 6;
            var height = 7;
            var graph = GraphFactory.CreateRectangularGraph(width, height, OffsetTypes.OddRowsRight,
                MovementTypesMock.Ground);

            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));

            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(new Coordinate2D(x, y, OffsetTypes.OddRowsRight).To3D()), Is.True);

            width = 4;
            height = 5;
            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, width, height, MovementTypesMock.Ground);

            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(new Coordinate2D(x, y, OffsetTypes.OddRowsRight).To3D()), Is.True);

            width = 7;
            height = 8;
            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, width, height, MovementTypesMock.Ground);

            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(new Coordinate2D(x, y, OffsetTypes.OddRowsRight).To3D()), Is.True);
        }

        [Test]
        public void ResizeSquareGraph_ShouldIncreaseHeight()
        {
            var width = 3;
            var height = 3;
            var graph = GraphFactory.CreateRectangularGraph(width, height, OffsetTypes.OddRowsRight,
                MovementTypesMock.Ground);

            height = 4;

            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, width, height, MovementTypesMock.Ground);
            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(new Coordinate2D(x, y, OffsetTypes.OddRowsRight).To3D()), Is.True);
        }

        [Test]
        public void ResizeSquareGraph_ShouldIncreaseWidth()
        {
            var width = 4;
            var height = 3;
            var graph = GraphFactory.CreateRectangularGraph(width, height, OffsetTypes.OddRowsRight,
                MovementTypesMock.Ground);

            width = 5;

            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, width, height, MovementTypesMock.Ground);
            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(new Coordinate2D(x, y, OffsetTypes.OddRowsRight).To3D()), Is.True);
        }

        [Test]
        public void ResizeSquareGraph_ShouldIncreaseWidthAndHeight()
        {
            var width = 3;
            var height = 4;
            var graph = GraphFactory.CreateRectangularGraph(width, height, OffsetTypes.OddRowsRight,
                MovementTypesMock.Ground);

            width = 4;
            height = 5;

            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, width, height, MovementTypesMock.Ground);
            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(new Coordinate2D(x, y, OffsetTypes.OddRowsRight).To3D()), Is.True);
        }

        [Test]
        public void ResizeSquareGraph_ShouldMaintainCellStatesOnResize()
        {
            var graph = GraphFactory.CreateRectangularGraph(3, 3, OffsetTypes.OddRowsRight, MovementTypesMock.Ground);
            Assert.False(graph.IsCellBlocked(new Coordinate2D(0, 1, OffsetTypes.OddRowsRight).To3D()));
            graph.SetOneCellBlocked(new Coordinate2D(0, 1, OffsetTypes.OddRowsRight).To3D(), true);
            Assert.True(graph.IsCellBlocked(new Coordinate2D(0, 1, OffsetTypes.OddRowsRight).To3D()));
            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, 2, 2, MovementTypesMock.Ground);
            Assert.True(graph.IsCellBlocked(new Coordinate2D(0, 1, OffsetTypes.OddRowsRight).To3D()));
        }

        [Test]
        public void ResizeSquareGraph_ShouldNotResizeIfNothingChanged()
        {
            var width = 3;
            var height = 3;
            var graph = GraphFactory.CreateRectangularGraph(width, height, OffsetTypes.OddRowsRight,
                MovementTypesMock.Ground);

            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, width, height, MovementTypesMock.Ground);
            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(new Coordinate2D(x, y, OffsetTypes.OddRowsRight).To3D()), Is.True);
        }

        [Test]
        public void ResizeSquareGraph_ShouldReduceHeight()
        {
            var width = 3;
            var height = 3;
            var graph = GraphFactory.CreateRectangularGraph(width, height, OffsetTypes.OddRowsRight,
                MovementTypesMock.Ground);

            height = 2;

            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, width, height, MovementTypesMock.Ground);
            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(new Coordinate2D(x, y, OffsetTypes.OddRowsRight).To3D()), Is.True);
        }

        [Test]
        public void ResizeSquareGraph_ShouldReduceWidth()
        {
            var width = 3;
            var height = 3;
            var graph = GraphFactory.CreateRectangularGraph(width, height, OffsetTypes.OddRowsRight,
                MovementTypesMock.Ground);

            width = 2;

            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, width, height, MovementTypesMock.Ground);
            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(new Coordinate2D(x, y, OffsetTypes.OddRowsRight).To3D()), Is.True);
        }

        [Test]
        public void ResizeSquareGraph_ShouldReduceWidthAndHeight()
        {
            var width = 3;
            var height = 3;
            var graph = GraphFactory.CreateRectangularGraph(width, height, OffsetTypes.OddRowsRight,
                MovementTypesMock.Ground);

            width = 2;
            height = 2;

            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, width, height, MovementTypesMock.Ground);
            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(new Coordinate2D(x, y, OffsetTypes.OddRowsRight).To3D()), Is.True);
        }
    }
}