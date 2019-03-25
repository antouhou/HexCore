using System.Collections.Generic;
using System.Linq;
using HexCore.DataStructures;
using HexCore.HexGraph;
using NUnit.Framework;
using Tests.Fixtures;

namespace Tests.HexGraph
{
    [TestFixture]
    public class GraphTest
    {
        [Test]
        public void ShouldBeAbleToFindingShortestPath()
        {
            // Note: this method uses AStarSearch class inside.
            // AStarSerach has its own comprehensive tests, so this test is only to ensure that this method exists and
            // returns something meaningful.
            var graph = GraphFactory.CreateSquareGraph(height: 3, width: 3);
            var start = new Coordinate2D(0, 0, OffsetTypes.OddRowsRight).To3D();
            var goal = new Coordinate2D(2, 2, OffsetTypes.OddRowsRight).To3D();
            var shortesPath = graph.GetShortestPath(start, goal, MovementTypes.Ground);
            var expectedShortestPath = Coordinate2D.To3D(new List<Coordinate2D>()
            {
                new Coordinate2D(1, 0, OffsetTypes.OddRowsRight),
                new Coordinate2D(1, 1, OffsetTypes.OddRowsRight),
                new Coordinate2D(2, 2, OffsetTypes.OddRowsRight)
            });

            Assert.That(shortesPath, Is.EqualTo(expectedShortestPath));
        }

        [Test]
        public void ShouldBlockCell()
        {
            var graph = GraphFactory.CreateSquareGraph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.Ground);
            Assert.That(graph.IsCellBlocked(new Coordinate3D(0, 0, 0)), Is.False);
            graph.SetOneCellBlocked(new Coordinate3D(0, 0, 0), true);
            Assert.That(graph.IsCellBlocked(new Coordinate3D(0, 0, 0)), Is.True);
            graph.SetOneCellBlocked(new Coordinate3D(0, 0, 0), false);
            Assert.That(graph.IsCellBlocked(new Coordinate3D(0, 0, 0)), Is.False);
        }

        [Test]
        public void ShouldGetCorrectMovementRange()
        {
            var graph = GraphFactory.CreateSquareGraph(6, 7, OffsetTypes.OddRowsRight, MovementTypes.Ground);
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

            var movementRange = graph.GetMovementRange(center, 2, MovementTypes.Ground);

            Assert.That(movementRange.Count, Is.EqualTo(expectedMovementRange.Count));
            Assert.That(movementRange, Is.EqualTo(expectedMovementRange));

            // If 2,3 is water, we shouldn't be able to access 2,4. If we make 1,3 water - we just shouldn't be able to 
            // access it, since going to 1,3 will cost more than movement points we have.
            graph.SetManyCellsMovementType(Coordinate2D.To3D(new List<Coordinate2D>
            {
                new Coordinate2D(2, 3, OffsetTypes.OddRowsRight),
                new Coordinate2D(1, 3, OffsetTypes.OddRowsRight)
            }), MovementTypes.Water);

            // Blocking 2,1 will prevent us from going to 2,1 and 2,0 at the same time
            graph.SetOneCellBlocked(new Coordinate2D(2, 1, OffsetTypes.OddRowsRight).To3D(), true);

            // 2,4 isn't accessible because the only path to it thorough the water
            expectedMovementRange2D.Remove(new Coordinate2D(2, 4, OffsetTypes.OddRowsRight));
            // 1,3 isn't accessible because it is water
            expectedMovementRange2D.Remove(new Coordinate2D(1, 3, OffsetTypes.OddRowsRight));
            // 2,1 and 2,0 isn't accessible because 2,1 is blocked
            expectedMovementRange2D.Remove(new Coordinate2D(2, 1, OffsetTypes.OddRowsRight));
            expectedMovementRange2D.Remove(new Coordinate2D(2, 0, OffsetTypes.OddRowsRight));

            expectedMovementRange = Coordinate2D.To3D(expectedMovementRange2D);

            movementRange = graph.GetMovementRange(center, 2, MovementTypes.Ground);

            Assert.That(movementRange, Is.EqualTo(expectedMovementRange));
        }

        [Test]
        public void ShouldGetCorrectNeighbors()
        {
            var graph = GraphFactory.CreateSquareGraph(6, 7, OffsetTypes.OddRowsRight, MovementTypes.Ground);

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
        public void ShouldGetCorrectRange()
        {
            var graph = GraphFactory.CreateSquareGraph(6, 7, OffsetTypes.OddRowsRight, MovementTypes.Ground);
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
        public void ShouldSetMovementTypesToCells()
        {
            var graph = GraphFactory.CreateSquareGraph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.Ground);

            var coordinateToSet = new Coordinate2D(2, 1, OffsetTypes.OddRowsRight).To3D();

            graph.SetOneCellMovementType(coordinateToSet, MovementTypes.Water);
            Assert.That(graph.GetCellState(coordinateToSet).MovementType, Is.EqualTo(MovementTypes.Water));

            var coordinatesToSet = Coordinate2D.To3D(new List<Coordinate2D>
            {
                new Coordinate2D(0, 1, OffsetTypes.OddRowsRight),
                new Coordinate2D(0, 2, OffsetTypes.OddRowsRight)
            });

            graph.SetManyCellsMovementType(coordinatesToSet, MovementTypes.Water);
            foreach (var coordinate in coordinatesToSet)
                Assert.That(graph.GetCellState(coordinate).MovementType, Is.EqualTo(MovementTypes.Water));
        }
    }
}