using HexCore.DataStructures;
using HexCore.Helpers;
using HexCore.HexGraph;
using NUnit.Framework;
using Tests.Fixtures;

namespace Tests.HexGraph
{
    [TestFixture]
    public class GraphUtilsTest

    {
        private readonly CoordinateConverter
            _coordinateConverterOrr = new CoordinateConverter(OffsetTypes.OddRowsRight);

        [Test]
        public void AllCellsShouldHaveCorrectPositions()
        {
            var width = 6;
            var height = 7;
            var graph = GraphFactory.CreateSquareGraph(width, height, OffsetTypes.OddRowsRight, MovementTypes.Ground);

            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));

            var kek = _coordinateConverterOrr.ConvertManyCubeToOffset(graph.GetAllCellsCoordinates());
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(_coordinateConverterOrr.ConvertOneOffsetToCube(new Coordinate2D(x, y))), Is.True);

            width = 4;
            height = 5;
            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, width, height, MovementTypes.Ground);

            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(_coordinateConverterOrr.ConvertOneOffsetToCube(new Coordinate2D(x, y))), Is.True);

            width = 7;
            height = 8;
            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, width, height, MovementTypes.Ground);

            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(_coordinateConverterOrr.ConvertOneOffsetToCube(new Coordinate2D(x, y))), Is.True);
        }

        [Test]
        public void ShouldIncreaseHeight()
        {
            var width = 3;
            var height = 3;
            var graph = GraphFactory.CreateSquareGraph(width, height, OffsetTypes.OddRowsRight, MovementTypes.Ground);

            height = 4;

            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, width, height, MovementTypes.Ground);
            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(_coordinateConverterOrr.ConvertOneOffsetToCube(new Coordinate2D(x, y))), Is.True);
        }

        [Test]
        public void ShouldIncreaseWidth()
        {
            var width = 4;
            var height = 3;
            var graph = GraphFactory.CreateSquareGraph(width, height, OffsetTypes.OddRowsRight, MovementTypes.Ground);

            width = 5;

            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, width, height, MovementTypes.Ground);
            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(_coordinateConverterOrr.ConvertOneOffsetToCube(new Coordinate2D(x, y))), Is.True);
        }

        [Test]
        public void ShouldIncreaseWidthAndHeight()
        {
            var width = 3;
            var height = 4;
            var graph = GraphFactory.CreateSquareGraph(width, height, OffsetTypes.OddRowsRight, MovementTypes.Ground);

            width = 4;
            height = 5;

            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, width, height, MovementTypes.Ground);
            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(_coordinateConverterOrr.ConvertOneOffsetToCube(new Coordinate2D(x, y))), Is.True);
        }

        [Test]
        public void ShouldMaintainCellStatesOnResize()
        {
            var graph = GraphFactory.CreateSquareGraph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.Ground);
            Assert.False(graph.IsCellBlocked(_coordinateConverterOrr.ConvertOneOffsetToCube(new Coordinate2D(0, 1))));
            graph.SetOneCellBlocked(_coordinateConverterOrr.ConvertOneOffsetToCube(new Coordinate2D(0, 1)), true);
            Assert.True(graph.IsCellBlocked(_coordinateConverterOrr.ConvertOneOffsetToCube(new Coordinate2D(0, 1))));
            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, 2, 2, MovementTypes.Ground);
            Assert.True(graph.IsCellBlocked(_coordinateConverterOrr.ConvertOneOffsetToCube(new Coordinate2D(0, 1))));
        }

        [Test]
        public void ShouldNotResizeIfNothingChanged()
        {
            var width = 3;
            var height = 3;
            var graph = GraphFactory.CreateSquareGraph(width, height, OffsetTypes.OddRowsRight, MovementTypes.Ground);

            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, width, height, MovementTypes.Ground);
            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(_coordinateConverterOrr.ConvertOneOffsetToCube(new Coordinate2D(x, y))), Is.True);
        }

        [Test]
        public void ShouldReduceHeight()
        {
            var width = 3;
            var height = 3;
            var graph = GraphFactory.CreateSquareGraph(width, height, OffsetTypes.OddRowsRight, MovementTypes.Ground);

            height = 2;

            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, width, height, MovementTypes.Ground);
            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(_coordinateConverterOrr.ConvertOneOffsetToCube(new Coordinate2D(x, y))), Is.True);
        }

        [Test]
        public void ShouldReduceWidth()
        {
            var width = 3;
            var height = 3;
            var graph = GraphFactory.CreateSquareGraph(width, height, OffsetTypes.OddRowsRight, MovementTypes.Ground);

            width = 2;

            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, width, height, MovementTypes.Ground);
            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(_coordinateConverterOrr.ConvertOneOffsetToCube(new Coordinate2D(x, y))), Is.True);
        }

        [Test]
        public void ShouldReduceWidthAndHeight()
        {
            var width = 3;
            var height = 3;
            var graph = GraphFactory.CreateSquareGraph(width, height, OffsetTypes.OddRowsRight, MovementTypes.Ground);

            width = 2;
            height = 2;

            GraphUtils.ResizeSquareGraph(graph, OffsetTypes.OddRowsRight, width, height, MovementTypes.Ground);
            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(_coordinateConverterOrr.ConvertOneOffsetToCube(new Coordinate2D(x, y))), Is.True);
        }
    }
}