using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        [Test]
        public void ShouldCreateGraph()
        {
            var graph = new Graph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            Assert.That(graph.Columns.Count, Is.EqualTo(3));
            foreach (var row in graph.Columns)
            {
                Assert.That(row.Count, Is.EqualTo(3));
            }
        }

        [Test]
        public void ShouldResizeWholeGraphUp()
        {
            var graph = new Graph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            graph.Resize(4, 5);
            Assert.That(graph.Columns.Count, Is.EqualTo(4));
            foreach (var row in graph.Columns)
            {
                Assert.That(row.Count, Is.EqualTo(5));
            }
        }

        [Test]
        public void ShouldResizeGraphDown()
        {
            var graph = new Graph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            graph.Resize(2, 3);
            Assert.That(graph.Columns.Count, Is.EqualTo(2));
            foreach (var row in graph.Columns)
            {
                Assert.That(row.Count, Is.EqualTo(3));
            }
        }


        [Test]
        public void ShouldResizeOnlyCellsInEachRow()
        {
            var graph = new Graph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            graph.Resize(3, 4);
            Assert.That(graph.Columns.Count, Is.EqualTo(3));
            foreach (var row in graph.Columns)
            {
                Assert.That(row.Count, Is.EqualTo(4));
            }
        }

        [Test]
        public void ShouldResizeOnlyCellsInEachRowDown()
        {
            var graph = new Graph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            graph.Resize(3, 2);
            Assert.That(graph.Columns.Count, Is.EqualTo(3));
            foreach (var row in graph.Columns)
            {
                Assert.That(row.Count, Is.EqualTo(2));
            }
        }

        [Test]
        public void ShouldResizeOnlyHeightUp()
        {
            var graph = new Graph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            graph.Resize(4, 3);
            Assert.That(graph.Columns.Count, Is.EqualTo(4));
            foreach (var row in graph.Columns)
            {
                Assert.That(row.Count, Is.EqualTo(3));
            }
        }

        [Test]
        public void ShouldResizeOnlyColsDown()
        {
            var graph = new Graph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            graph.Resize(2, 3);
            Assert.That(graph.Columns.Count, Is.EqualTo(2));
            foreach (var row in graph.Columns)
            {
                Assert.That(row.Count, Is.EqualTo(3));
            }
        }

        [Test]
        public void ShouldNotResizeIfNothingChanged()
        {
            var graph = new Graph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            graph.Resize(3, 3);
            Assert.That(graph.Columns.Count, Is.EqualTo(3));
            foreach (var row in graph.Columns)
            {
                Assert.That(row.Count, Is.EqualTo(3));
            }
        }


        [Test]
        public void ShouldMaintainCellStatesOnResize()
        {
            var graph = new Graph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            Assert.False(graph.Columns[0][1].IsBlocked);
            graph.Columns[0][1].IsBlocked = true;
            Assert.True(graph.Columns[0][1].IsBlocked);
            graph.Resize(2, 2);
            Assert.True(graph.Columns[0][1].IsBlocked);
        }

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
                {
                    Assert.That(graph.Columns[x][y].Position, Is.EqualTo(new Coordinate2D(x, y)));
                }
            }

            graph.Resize(4, 5);
            Assert.That(graph.Columns.Count, Is.EqualTo(4));
            for (var x = 0;
                x < graph.Columns.Count;
                x++)
            {
                Assert.That(graph.Columns[x].Count, Is.EqualTo(5));
                for (var y = 0; y < graph.Columns[x].Count; y++)
                {
                    Assert.That(graph.Columns[x][y].Position, Is.EqualTo(new Coordinate2D(x, y)));
                }
            }

            graph.Resize(7, 8);
            Assert.That(graph.Columns.Count, Is.EqualTo(7));
            for (var x = 0;
                x < graph.Columns.Count;
                x++)
            {
                Assert.That(graph.Columns[x].Count, Is.EqualTo(8));
                for (var y = 0; y < graph.Columns[x].Count; y++)
                {
                    Assert.That(graph.Columns[x][y].Position, Is.EqualTo(new Coordinate2D(x, y)));
                }
            }
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
                {
                    Assert.That(graph.Columns[x][y].Position, Is.EqualTo(new Coordinate2D(x, y)));
                }
            }

            // Column 2, row 1
            var offsetTarget = new Coordinate2D(2, 1);
            var cubeTarget = CoordinateConverter.ConvertOneOffsetToCube(OffsetTypes.OddRowsRight, offsetTarget);
            // Correctness of convertion is target for CoordinateConverter test
            // var expectedCubeTarget = new CubeCoordinate(2, -3, 1);
            var neighbors = graph.GetNeighbors(cubeTarget, false).ToList();
            // Neighbors for cube coordinates should be:
            // Cube(+1, -1, 0), Cube(+1, 0, -1), Cube(0, +1, -1), Cube(-1, +1, 0), Cube(-1, 0, +1), Cube(0, -1, +1),
            var expectedNeighbors = new List<Coordinate3D>()
            {
                new Coordinate3D(3, -4, 1),
                new Coordinate3D(3, -3, 0),
                new Coordinate3D(2, -2, 0),
                new Coordinate3D(1, -2, 1),
                new Coordinate3D(1, -3, +2),
                new Coordinate3D(2, -4, 2)
            };
            Assert.That(neighbors.Count, Is.EqualTo(expectedNeighbors.Count));
            for (var index = 0; index < expectedNeighbors.Count; index++)
            {
                Assert.That(neighbors[index], Is.EqualTo(expectedNeighbors[index]));
            }
        }
        
        [Test]
        public void ShouldGetCorrectMovableArea()
        {
            var graph = new Graph(6, 7, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            var center = CoordinateConverter.ConvertOneOffsetToCube(OffsetTypes.OddRowsRight, new Coordinate2D(3,2));
            
            var expectedMovableArea2D = new List<Coordinate2D>
            {
                // Closest circle
                new Coordinate2D(4,2),
                new Coordinate2D(3,1),
                new Coordinate2D(2,1),
                new Coordinate2D(2,2),
                new Coordinate2D(2,3),
                new Coordinate2D(3,3),
                // Second circle
                new Coordinate2D(5,2),
                new Coordinate2D(4,1),
                new Coordinate2D(4,3),
                new Coordinate2D(4,0),
                new Coordinate2D(3,0),
                new Coordinate2D(2,0),
                new Coordinate2D(1,1),
                new Coordinate2D(1,2),
                new Coordinate2D(1,3),
                new Coordinate2D(2,4),
                new Coordinate2D(3,4),
                new Coordinate2D(4,4),
            };
            var expectedMovableArea =
                CoordinateConverter.ConvertManyOffsetToCube(OffsetTypes.OddRowsRight, expectedMovableArea2D);
            
            // 1 is center
            var movableArea = graph.GetMovableArea(center, 3);

            var movableArea2D = CoordinateConverter.ConvertManyCubeToOffset(OffsetTypes.OddRowsRight, movableArea);
            
            Assert.That(movableArea.Count, Is.EqualTo(expectedMovableArea.Count));
            Assert.That(movableArea, Is.EqualTo(expectedMovableArea));
        }
    }
}