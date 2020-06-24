using System;
using System.Collections.Generic;
using System.Linq;
using HexCore.DataStructures;
using HexCore.HexGraph;
using HexCoreTests.Fixtures;
using NUnit.Framework;

namespace HexCoreTests.HexGraph
{
    [TestFixture]
    [TestOf(typeof(Graph))]
    public class GraphTest
    {
        [Test]
        public void GetLine_ShouldGetCorrectDirectionFromOneCoordinateToAnother()
        {
            var graph = GraphFactory.CreateRectangularGraph(10, 10, MovementTypesFixture.GetMovementTypes(),
                MovementTypesFixture.Ground);
            var start = new Coordinate3D(1, -3, 2);

            var direction = graph.GetLine(start, new Coordinate3D(0, 1, -1), 2);
            var expectedDirection = new List<Coordinate3D>
            {
                new Coordinate3D(1, -2, 1),
                new Coordinate3D(1, -1, 0)
            };
            Assert.That(direction.ToList(), Is.EqualTo(expectedDirection));
        }

        [Test]
        public void GetLine_ShouldNotIncludeCoordinatesOutsideOfGraph()
        {
            var graph = GraphFactory.CreateRectangularGraph(10, 10, MovementTypesFixture.GetMovementTypes(),
                MovementTypesFixture.Ground);
            var start = new Coordinate3D(1, -3, 2);

            var direction = graph.GetLine(start, new Coordinate3D(0, 1, -1), 5);
            var expectedDirection = new List<Coordinate3D>
            {
                new Coordinate3D(1, -2, 1),
                new Coordinate3D(1, -1, 0)
            };
            Assert.That(direction.ToList(), Is.EqualTo(expectedDirection));
        }

        [Test]
        public void GetLine_ShouldThrowIfCoordinateIsNotDirection()
        {
            var graph = GraphFactory.CreateRectangularGraph(10, 10, MovementTypesFixture.GetMovementTypes(),
                MovementTypesFixture.Ground);
            var start = new Coordinate3D(1, -3, 2);

            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                var dir = graph.GetLine(start, new Coordinate3D(-2, 3, -1), 2);
                dir.ToList();
            });
            Assert.That(exception.Message, Is.EqualTo("Invalid direction"));
        }

        [Test]
        public void GetMovementRange_ShouldGetCorrectMovementRange()
        {
            var graph = GraphFactory.CreateRectangularGraph(6, 7, MovementTypesFixture.GetMovementTypes(),
                MovementTypesFixture.Ground);
            var center = new Coordinate2D(3, 2, OffsetTypes.OddRowsRight).To3D();

            var expectedMovementRange2D = new List<Coordinate2D>
            {
                // Closest circle
                new Coordinate2D(4, 2, OffsetTypes.OddRowsRight),
                new Coordinate2D(3, 1, OffsetTypes.OddRowsRight),
                new Coordinate2D(2, 1, OffsetTypes.OddRowsRight),
                new Coordinate2D(2, 2, OffsetTypes.OddRowsRight),
                new Coordinate2D(2, 3, OffsetTypes.OddRowsRight),
                new Coordinate2D(3, 3, OffsetTypes.OddRowsRight),
                // Second circle
                new Coordinate2D(5, 2, OffsetTypes.OddRowsRight),
                new Coordinate2D(4, 1, OffsetTypes.OddRowsRight),
                new Coordinate2D(4, 3, OffsetTypes.OddRowsRight),
                new Coordinate2D(4, 0, OffsetTypes.OddRowsRight),
                new Coordinate2D(3, 0, OffsetTypes.OddRowsRight),
                new Coordinate2D(2, 0, OffsetTypes.OddRowsRight),
                new Coordinate2D(1, 1, OffsetTypes.OddRowsRight),
                new Coordinate2D(1, 2, OffsetTypes.OddRowsRight),
                new Coordinate2D(1, 3, OffsetTypes.OddRowsRight),
                new Coordinate2D(2, 4, OffsetTypes.OddRowsRight),
                new Coordinate2D(3, 4, OffsetTypes.OddRowsRight),
                new Coordinate2D(4, 4, OffsetTypes.OddRowsRight)
            };
            var expectedMovementRange =
                Coordinate2D.To3D(expectedMovementRange2D);

            var movementRange = graph.GetMovementRange(center, 2, MovementTypesFixture.Walking);

            Assert.That(movementRange.Count, Is.EqualTo(expectedMovementRange.Count));
            Assert.That(movementRange, Is.EqualTo(expectedMovementRange));

            // If 2,3 is water, we shouldn't be able to access 2,4. If we make 1,3 water - we just shouldn't be able to 
            // access it, since going to 1,3 will cost more than movement points we have.
            graph.SetCellsTerrainType(Coordinate2D.To3D(new List<Coordinate2D>
            {
                new Coordinate2D(2, 3, OffsetTypes.OddRowsRight),
                new Coordinate2D(1, 3, OffsetTypes.OddRowsRight)
            }), MovementTypesFixture.Water);

            // Blocking 2,1 will prevent us from going to 2,1 and 2,0 at the same time
            graph.BlockCells(new Coordinate2D(2, 1, OffsetTypes.OddRowsRight).To3D());

            // 2,4 isn't accessible because the only path to it thorough the water
            expectedMovementRange2D.Remove(new Coordinate2D(2, 4, OffsetTypes.OddRowsRight));
            // 1,3 isn't accessible because it is water
            expectedMovementRange2D.Remove(new Coordinate2D(1, 3, OffsetTypes.OddRowsRight));
            // 2,1 and 2,0 isn't accessible because 2,1 is blocked
            expectedMovementRange2D.Remove(new Coordinate2D(2, 1, OffsetTypes.OddRowsRight));
            expectedMovementRange2D.Remove(new Coordinate2D(2, 0, OffsetTypes.OddRowsRight));

            expectedMovementRange = Coordinate2D.To3D(expectedMovementRange2D);

            movementRange = graph.GetMovementRange(center, 2, MovementTypesFixture.Walking);

            Assert.That(movementRange, Is.EqualTo(expectedMovementRange));
        }

        [Test]
        public void GetNeighbours_ShouldGetCorrectNeighbors()
        {
            var graph = GraphFactory.CreateRectangularGraph(6, 7, MovementTypesFixture.GetMovementTypes(),
                MovementTypesFixture.Ground);

            // Column 2, row 1
            var offsetTarget = new Coordinate2D(2, 1, OffsetTypes.OddRowsRight);
            var cubeTarget = offsetTarget.To3D();
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
        public void GetRange_ShouldGetCorrectRange()
        {
            var graph = GraphFactory.CreateRectangularGraph(6, 7, MovementTypesFixture.GetMovementTypes(),
                MovementTypesFixture.Ground);
            var center = new Coordinate2D(3, 2, OffsetTypes.OddRowsRight).To3D();

            var expectedRange2D = new List<Coordinate2D>
            {
                // Closest circle
                new Coordinate2D(4, 2, OffsetTypes.OddRowsRight),
                new Coordinate2D(3, 1, OffsetTypes.OddRowsRight),
                new Coordinate2D(2, 1, OffsetTypes.OddRowsRight),
                new Coordinate2D(2, 2, OffsetTypes.OddRowsRight),
                new Coordinate2D(2, 3, OffsetTypes.OddRowsRight),
                new Coordinate2D(3, 3, OffsetTypes.OddRowsRight),
                // Second circle
                new Coordinate2D(5, 2, OffsetTypes.OddRowsRight),
                new Coordinate2D(4, 1, OffsetTypes.OddRowsRight),
                new Coordinate2D(4, 3, OffsetTypes.OddRowsRight),
                new Coordinate2D(4, 0, OffsetTypes.OddRowsRight),
                new Coordinate2D(3, 0, OffsetTypes.OddRowsRight),
                new Coordinate2D(2, 0, OffsetTypes.OddRowsRight),
                new Coordinate2D(1, 1, OffsetTypes.OddRowsRight),
                new Coordinate2D(1, 2, OffsetTypes.OddRowsRight),
                new Coordinate2D(1, 3, OffsetTypes.OddRowsRight),
                new Coordinate2D(2, 4, OffsetTypes.OddRowsRight),
                new Coordinate2D(3, 4, OffsetTypes.OddRowsRight),
                new Coordinate2D(4, 4, OffsetTypes.OddRowsRight)
            };
            var expectedMovementRange =
                Coordinate2D.To3D(expectedRange2D);

            var range = graph.GetRange(center, 2);

            Assert.That(range, Is.EqualTo(expectedMovementRange));
        }

        [Test]
        public void GetShortestPath_ShouldBeAbleToFindingShortesetPath()
        {
            // Note: this method uses AStarSearch class inside.
            // AStarSerach has its own comprehensive tests, so this test is only to ensure that this method exists and
            // returns something meaningful.
            var graph = GraphFactory.CreateRectangularGraph(3,
                3, MovementTypesFixture.GetMovementTypes(), MovementTypesFixture.Ground);
            var start = new Coordinate2D(0, 0, OffsetTypes.OddRowsRight).To3D();
            var goal = new Coordinate2D(2, 2, OffsetTypes.OddRowsRight).To3D();
            var shortestPath = graph.GetShortestPath(start, goal, MovementTypesFixture.Walking);
            var expectedShortestPath = Coordinate2D.To3D(new List<Coordinate2D>
            {
                new Coordinate2D(1, 0, OffsetTypes.OddRowsRight),
                new Coordinate2D(1, 1, OffsetTypes.OddRowsRight),
                new Coordinate2D(2, 2, OffsetTypes.OddRowsRight)
            });

            Assert.That(shortestPath, Is.EqualTo(expectedShortestPath));
        }

        [Test]
        public void IsInBounds_ShouldReturnTrueIfThePositionIsWithinTheGraphBounds()
        {
            var graph = GraphFactory.CreateRectangularGraph(3, 3, MovementTypesFixture.GetMovementTypes(),
                MovementTypesFixture.Ground);
            var position = new Coordinate3D(0, -1, 1);

            Assert.That(graph.IsInBounds(position), Is.True);

            position = new Coordinate3D(-1, 2, -1);
            Assert.That(graph.IsInBounds(position), Is.False);
        }

        [Test]
        public void SetManyCellsMovementType_ShouldSetMovementTypesToCells()
        {
            var graph = GraphFactory.CreateRectangularGraph(3, 3, MovementTypesFixture.GetMovementTypes(),
                MovementTypesFixture.Ground);

            var coordinateToSet = new Coordinate2D(2, 1, OffsetTypes.OddRowsRight).To3D();

            graph.SetCellTerrainType(coordinateToSet, MovementTypesFixture.Water);
            Assert.That(graph.GetCellState(coordinateToSet).TerrainType, Is.EqualTo(MovementTypesFixture.Water));

            var coordinatesToSet = Coordinate2D.To3D(new List<Coordinate2D>
            {
                new Coordinate2D(0, 1, OffsetTypes.OddRowsRight),
                new Coordinate2D(0, 2, OffsetTypes.OddRowsRight)
            });

            graph.SetCellsTerrainType(coordinatesToSet, MovementTypesFixture.Water);
            foreach (var coordinate in coordinatesToSet)
                Assert.That(graph.GetCellState(coordinate).TerrainType, Is.EqualTo(MovementTypesFixture.Water));
        }

        [Test]
        public void UnblockCell_ShouldUnblockCell()
        {
            var graph = GraphFactory.CreateRectangularGraph(3, 3, MovementTypesFixture.GetMovementTypes(),
                MovementTypesFixture.Ground);
            Assert.That(graph.IsCellBlocked(new Coordinate3D(0, 0, 0)), Is.False);
            graph.BlockCells(new Coordinate3D(0, 0, 0));
            Assert.That(graph.IsCellBlocked(new Coordinate3D(0, 0, 0)), Is.True);
            graph.UnblockCells(new Coordinate3D(0, 0, 0));
            Assert.That(graph.IsCellBlocked(new Coordinate3D(0, 0, 0)), Is.False);
        }
    }
}