using System.Collections.Generic;
using System.Linq;
using HexCore.DataStructures;
using HexCore.Helpers;
using HexCore.HexGraph;
using NUnit.Framework;
using Tests.Fixtures;

namespace Tests.HexGraph
{
    [TestFixture]
    public class GraphTest
    {
        private readonly CoordinateConverter
            _coordinateConverterOrr = new CoordinateConverter(OffsetTypes.OddRowsRight);

        [Test]
        public void AllCellsShouldHaveCorrectPositions()
        {
            var graph = new Graph(6, 7, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            Assert.That(graph.Columns.Count, Is.EqualTo(6));
            for (var x = 0;
                x < graph.Columns.Count;
                x++)
            {
                Assert.That(graph.Columns[x].Count, Is.EqualTo(7));
                for (var y = 0; y < graph.Columns[x].Count; y++)
                    Assert.That(graph.Columns[x][y].Coordinate2, Is.EqualTo(new Coordinate2D(x, y)));
            }

            graph.Resize(4, 5);
            Assert.That(graph.Columns.Count, Is.EqualTo(4));
            for (var x = 0;
                x < graph.Columns.Count;
                x++)
            {
                Assert.That(graph.Columns[x].Count, Is.EqualTo(5));
                for (var y = 0; y < graph.Columns[x].Count; y++)
                    Assert.That(graph.Columns[x][y].Coordinate2, Is.EqualTo(new Coordinate2D(x, y)));
            }

            graph.Resize(7, 8);
            Assert.That(graph.Columns.Count, Is.EqualTo(7));
            for (var x = 0;
                x < graph.Columns.Count;
                x++)
            {
                Assert.That(graph.Columns[x].Count, Is.EqualTo(8));
                for (var y = 0; y < graph.Columns[x].Count; y++)
                    Assert.That(graph.Columns[x][y].Coordinate2, Is.EqualTo(new Coordinate2D(x, y)));
            }
        }

        [Test]
        public void ShouldCreateGraph()
        {
            var graph = new Graph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            Assert.That(graph.Columns.Count, Is.EqualTo(3));
            foreach (var row in graph.Columns) Assert.That(row.Count, Is.EqualTo(3));
        }

        [Test]
        public void ShouldGetCorrectMovableArea()
        {
            var graph = new Graph(6, 7, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            var center = _coordinateConverterOrr.ConvertOneOffsetToCube(new Coordinate2D(3, 2));

            var expectedMovableArea2D = new List<Coordinate2D>
            {
                // Closest circle
                new Coordinate2D(4, 2),
                new Coordinate2D(3, 1),
                new Coordinate2D(2, 1),
                new Coordinate2D(2, 2),
                new Coordinate2D(2, 3),
                new Coordinate2D(3, 3),
                // Second circle
                new Coordinate2D(5, 2),
                new Coordinate2D(4, 1),
                new Coordinate2D(4, 3),
                new Coordinate2D(4, 0),
                new Coordinate2D(3, 0),
                new Coordinate2D(2, 0),
                new Coordinate2D(1, 1),
                new Coordinate2D(1, 2),
                new Coordinate2D(1, 3),
                new Coordinate2D(2, 4),
                new Coordinate2D(3, 4),
                new Coordinate2D(4, 4)
            };
            var expectedMovableArea =
                _coordinateConverterOrr.ConvertManyOffsetToCube(expectedMovableArea2D);

            var movableArea = graph.GetMovableArea(center, 2, MovementTypes.Ground);

            var movableArea2D = _coordinateConverterOrr.ConvertManyCubeToOffset(movableArea);

            Assert.That(movableArea.Count, Is.EqualTo(expectedMovableArea.Count));
            Assert.That(movableArea, Is.EqualTo(expectedMovableArea));

            // If 2,3 is water, we shouldn't be able to access 2,4. If we make 1,3 water - we just shouldn't be able to 
            // access it, since going to 1,3 will cost more than movement points we have.
            graph.SetManyCellsMovementType(new List<Coordinate2D>
            {
                new Coordinate2D(2, 3),
                new Coordinate2D(1, 3),
            }, MovementTypes.Water);

            // Blocking 2,1 will prevent us from going to 2,1 and 2,0 at the same time
            graph.SetOneCellBlocked(_coordinateConverterOrr.ConvertOneOffsetToCube(new Coordinate2D(2, 1)), true);

            // 2,4 isn't accessible because the only path to it thorough the water
            expectedMovableArea2D.Remove(new Coordinate2D(2, 4));
            // 1,3 isn't accessible because it is water
            expectedMovableArea2D.Remove(new Coordinate2D(1, 3));
            // 2,1 and 2,0 isn't accessible because 2,1 is blocked
            expectedMovableArea2D.Remove(new Coordinate2D(2, 1));
            expectedMovableArea2D.Remove(new Coordinate2D(2, 0));

            expectedMovableArea =
                _coordinateConverterOrr.ConvertManyOffsetToCube(expectedMovableArea2D);

            movableArea = graph.GetMovableArea(center, 2, MovementTypes.Ground);

            movableArea2D = _coordinateConverterOrr.ConvertManyCubeToOffset(movableArea);

            Assert.That(movableArea, Is.EqualTo(expectedMovableArea));
        }

        [Test]
        public void ShouldGetCorrectNeighbors()
        {
            var graph = new Graph(6, 7, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            Assert.That(graph.Columns.Count, Is.EqualTo(6));
            for (var x = 0; x < graph.Columns.Count; x++)
            {
                Assert.That(graph.Columns[x].Count, Is.EqualTo(7));
                for (var y = 0; y < graph.Columns[x].Count; y++)
                    Assert.That(graph.Columns[x][y].Coordinate2, Is.EqualTo(new Coordinate2D(x, y)));
            }

            // Column 2, row 1
            var offsetTarget = new Coordinate2D(2, 1);
            var cubeTarget = _coordinateConverterOrr.ConvertOneOffsetToCube(offsetTarget);
            var neighbors = graph.GetNeighbors(cubeTarget, false).ToList();
            // Neighbors for cube coordinates should be:
            // Cube(+1, -1, 0), Cube(+1, 0, -1), Cube(0, +1, -1), Cube(-1, +1, 0), Cube(-1, 0, +1), Cube(0, -1, +1),
            var expectedNeighbors = new List<Coordinate3D>
            {
                new Coordinate3D(3, -4, 1),
                new Coordinate3D(3, -3, 0),
                new Coordinate3D(2, -2, 0),
                new Coordinate3D(1, -2, 1),
                new Coordinate3D(1, -3, +2),
                new Coordinate3D(2, -4, 2)
            };
            Assert.That(neighbors, Is.EqualTo(expectedNeighbors));
        }

        [Test]
        public void ShouldMaintainCellStatesOnResize()
        {
            var graph = new Graph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            Assert.False(graph.Columns[0][1].IsBlocked);
            graph.SetOneCellBlocked(_coordinateConverterOrr.ConvertOneOffsetToCube(new Coordinate2D(0, 1)), true);
            Assert.True(graph.Columns[0][1].IsBlocked);
            graph.Resize(2, 2);
            Assert.True(graph.Columns[0][1].IsBlocked);
        }

        [Test]
        public void ShouldNotResizeIfNothingChanged()
        {
            var graph = new Graph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            graph.Resize(3, 3);
            Assert.That(graph.Columns.Count, Is.EqualTo(3));
            foreach (var row in graph.Columns) Assert.That(row.Count, Is.EqualTo(3));
        }

        [Test]
        public void ShouldResizeGraphDown()
        {
            var graph = new Graph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            graph.Resize(2, 3);
            Assert.That(graph.Columns.Count, Is.EqualTo(2));
            foreach (var row in graph.Columns) Assert.That(row.Count, Is.EqualTo(3));
        }


        [Test]
        public void ShouldResizeOnlyCellsInEachRow()
        {
            var graph = new Graph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            graph.Resize(3, 4);
            Assert.That(graph.Columns.Count, Is.EqualTo(3));
            foreach (var row in graph.Columns) Assert.That(row.Count, Is.EqualTo(4));
        }

        [Test]
        public void ShouldResizeOnlyCellsInEachRowDown()
        {
            var graph = new Graph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            graph.Resize(3, 2);
            Assert.That(graph.Columns.Count, Is.EqualTo(3));
            foreach (var row in graph.Columns) Assert.That(row.Count, Is.EqualTo(2));
        }

        [Test]
        public void ShouldResizeOnlyColsDown()
        {
            var graph = new Graph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            graph.Resize(2, 3);
            Assert.That(graph.Columns.Count, Is.EqualTo(2));
            foreach (var row in graph.Columns) Assert.That(row.Count, Is.EqualTo(3));
        }

        [Test]
        public void ShouldResizeOnlyHeightUp()
        {
            var graph = new Graph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            graph.Resize(4, 3);
            Assert.That(graph.Columns.Count, Is.EqualTo(4));
            foreach (var row in graph.Columns) Assert.That(row.Count, Is.EqualTo(3));
        }

        [Test]
        public void ShouldResizeWholeGraphUp()
        {
            var graph = new Graph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            graph.Resize(4, 5);
            Assert.That(graph.Columns.Count, Is.EqualTo(4));
            foreach (var row in graph.Columns) Assert.That(row.Count, Is.EqualTo(5));
        }
    }
}