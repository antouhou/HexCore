using System;
using System.Collections.Generic;
using System.Linq;
using HexCore;
using HexCoreTests.Fixtures;
using NUnit.Framework;

namespace HexCoreTests
{
    [TestFixture]
    [TestOf(typeof(Graph))]
    public class GraphTest
    {
        [Test]
        public void AddCells_ShouldThrow_WhenAddingCellWithAnUnknownType()
        {
            var graph = GraphFactory.CreateRectangularGraph(3, 3, MovementTypesFixture.GetMovementTypes(),
                MovementTypesFixture.Ground);
            var unexpectedTerrainType = new TerrainType(10, "Some Unexpected Terrain");
            Assert.That(() =>
                {
                    graph.AddCells(new List<CellState>
                    {
                        new CellState(
                            false,
                            new Coordinate2D(),
                            unexpectedTerrainType
                        )
                    });
                },
                Throws.ArgumentException.With.Message.EqualTo(
                    "One of the cells in graph has an unknown terrain type: 'Some Unexpected Terrain'"));
        }

        [Test]
        public void GetAllCells_ShouldReturnAllCellStates()
        {
            var movementTypes = MovementTypesFixture.GetMovementTypes();
            var cellStates = new List<CellState>
            {
                new CellState(false, new Coordinate2D(0, 0, OffsetTypes.OddRowsRight), MovementTypesFixture.Ground),
                new CellState(false, new Coordinate2D(0, 1, OffsetTypes.OddRowsRight), MovementTypesFixture.Ground),
                new CellState(true, new Coordinate2D(1, 0, OffsetTypes.OddRowsRight), MovementTypesFixture.Water),
                new CellState(false, new Coordinate2D(1, 1, OffsetTypes.OddRowsRight), MovementTypesFixture.Water),
                new CellState(false, new Coordinate2D(1, 2, OffsetTypes.OddRowsRight), MovementTypesFixture.Ground)
            };
            var graph = new Graph(cellStates, movementTypes);

            Assert.That(graph.GetAllCells(), Is.EqualTo(cellStates));
        }

        [Test]
        public void GetAllCellsCoordinate_ShouldReturn2DCoordinates_WhenOffsetTypeIsPassed()
        {
            var states = new List<CellState>
            {
                new CellState(false, new Coordinate2D(0, 0, OffsetTypes.OddRowsRight), MovementTypesFixture.Ground),
                new CellState(false, new Coordinate2D(0, 1, OffsetTypes.OddRowsRight), MovementTypesFixture.Ground),
                new CellState(false, new Coordinate2D(1, 0, OffsetTypes.OddRowsRight), MovementTypesFixture.Ground),
                new CellState(false, new Coordinate2D(1, 1, OffsetTypes.OddRowsRight), MovementTypesFixture.Ground)
            };
            var graph = new Graph(states, MovementTypesFixture.GetMovementTypes());

            var expectedCoordinates = new List<Coordinate2D>
            {
                new Coordinate2D(0, 0, OffsetTypes.OddRowsRight),
                new Coordinate2D(0, 1, OffsetTypes.OddRowsRight),
                new Coordinate2D(1, 0, OffsetTypes.OddRowsRight),
                new Coordinate2D(1, 1, OffsetTypes.OddRowsRight)
            };

            Assert.That(
                graph.GetAllCellsCoordinates(OffsetTypes.OddRowsRight),
                Is.EqualTo(expectedCoordinates)
            );
        }

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
            var center2d = new Coordinate2D(3, 2, OffsetTypes.OddRowsRight);
            var center = center2d.To3D();

            var expectedMovementRange2D = new List<Coordinate2D>
            {
                // Closest circle
                new Coordinate2D(2, 1, OffsetTypes.OddRowsRight),
                new Coordinate2D(3, 1, OffsetTypes.OddRowsRight),
                new Coordinate2D(4, 2, OffsetTypes.OddRowsRight),
                new Coordinate2D(3, 3, OffsetTypes.OddRowsRight),
                new Coordinate2D(2, 3, OffsetTypes.OddRowsRight),
                new Coordinate2D(2, 2, OffsetTypes.OddRowsRight),
                // Second circle
                new Coordinate2D(2, 0, OffsetTypes.OddRowsRight),
                new Coordinate2D(3, 0, OffsetTypes.OddRowsRight),
                new Coordinate2D(1, 1, OffsetTypes.OddRowsRight),
                new Coordinate2D(4, 0, OffsetTypes.OddRowsRight),
                new Coordinate2D(4, 1, OffsetTypes.OddRowsRight),
                new Coordinate2D(5, 2, OffsetTypes.OddRowsRight),
                new Coordinate2D(4, 3, OffsetTypes.OddRowsRight),
                new Coordinate2D(4, 4, OffsetTypes.OddRowsRight),
                new Coordinate2D(3, 4, OffsetTypes.OddRowsRight),
                new Coordinate2D(2, 4, OffsetTypes.OddRowsRight),
                new Coordinate2D(1, 3, OffsetTypes.OddRowsRight),
                new Coordinate2D(1, 2, OffsetTypes.OddRowsRight)
            };
            var expectedMovementRange =
                Coordinate2D.To3D(expectedMovementRange2D);

            var movementRange2d = graph.GetMovementRange(center2d, 2, MovementTypesFixture.Walking);
            var movementRange = graph.GetMovementRange(center, 2, MovementTypesFixture.Walking);

            Assert.That(movementRange2d, Is.EqualTo(expectedMovementRange2D));
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

            movementRange2d = graph.GetMovementRange(center2d, 2, MovementTypesFixture.Walking);
            movementRange = graph.GetMovementRange(center, 2, MovementTypesFixture.Walking);

            Assert.That(movementRange2d, Is.EquivalentTo(expectedMovementRange2D));
            Assert.That(movementRange, Is.EquivalentTo(expectedMovementRange));
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
                new Coordinate3D(2, -2, 0),
                new Coordinate3D(3, -3, 0),
                new Coordinate3D(3, -4, 1),
                new Coordinate3D(2, -4, 2),
                new Coordinate3D(1, -3, 2),
                new Coordinate3D(1, -2, 1)
            };
            Assert.That(neighbors, Is.EqualTo(expectedNeighbors));

            Assert.That(
                graph.GetNeighbors(offsetTarget, false).ToList(),
                Is.EquivalentTo(Coordinate3D.To2D(expectedNeighbors, OffsetTypes.OddRowsRight))
            );
        }

        [Test]
        public void GetPassableNeighbors_ShouldExcludeBlockedNeighbors()
        {
            var graph = GraphFactory.CreateRectangularGraph(3, 3, MovementTypesFixture.GetMovementTypes(),
                MovementTypesFixture.Ground);
            var center2d = new Coordinate2D(1, 1, OffsetTypes.OddRowsRight);
            var center = center2d.To3D();

            graph.BlockCells(new List<Coordinate2D>
            {
                new Coordinate2D(2, 0, OffsetTypes.OddRowsRight),
                new Coordinate2D(1, 2, OffsetTypes.OddRowsRight)
            });

            var expectedPassableNeighbors2d = new List<Coordinate2D>
            {
                new Coordinate2D(1, 0, OffsetTypes.OddRowsRight),
                new Coordinate2D(2, 1, OffsetTypes.OddRowsRight),
                new Coordinate2D(2, 2, OffsetTypes.OddRowsRight),
                new Coordinate2D(0, 1, OffsetTypes.OddRowsRight)
            };
            var expectedPassableNeighbors = Coordinate2D.To3D(expectedPassableNeighbors2d);

            Assert.That(graph.GetPassableNeighbors(center2d), Is.EqualTo(expectedPassableNeighbors2d));
            Assert.That(graph.GetPassableNeighbors(center), Is.EqualTo(expectedPassableNeighbors));
        }

        [Test]
        public void GetPassableNeighbors_ShouldExcludeNeighborsOutsideOfTheGraph()
        {
            var graph = GraphFactory.CreateRectangularGraph(3, 3, MovementTypesFixture.GetMovementTypes(),
                MovementTypesFixture.Ground);
            var center2d = new Coordinate2D(1, 0, OffsetTypes.OddRowsRight);
            var center = center2d.To3D();

            var expectedPassableNeighbors2d = new List<Coordinate2D>
            {
                new Coordinate2D(2, 0, OffsetTypes.OddRowsRight),
                new Coordinate2D(1, 1, OffsetTypes.OddRowsRight),
                new Coordinate2D(0, 1, OffsetTypes.OddRowsRight),
                new Coordinate2D(0, 0, OffsetTypes.OddRowsRight)
            };
            var expectedPassableNeighbors = Coordinate2D.To3D(expectedPassableNeighbors2d);

            Assert.That(graph.GetPassableNeighbors(center2d), Is.EqualTo(expectedPassableNeighbors2d));
            Assert.That(graph.GetPassableNeighbors(center), Is.EqualTo(expectedPassableNeighbors));
        }

        [Test]
        public void GetPassableNeighbors_ShouldReturnAllNeighbors_WhenAllNeighborsArePassable()
        {
            var graph = GraphFactory.CreateRectangularGraph(3, 3, MovementTypesFixture.GetMovementTypes(),
                MovementTypesFixture.Ground);
            var center2d = new Coordinate2D(1, 1, OffsetTypes.OddRowsRight);
            var center = center2d.To3D();

            var expectedPassableNeighbors2d = new List<Coordinate2D>
            {
                new Coordinate2D(1, 0, OffsetTypes.OddRowsRight),
                new Coordinate2D(2, 0, OffsetTypes.OddRowsRight),
                new Coordinate2D(2, 1, OffsetTypes.OddRowsRight),
                new Coordinate2D(2, 2, OffsetTypes.OddRowsRight),
                new Coordinate2D(1, 2, OffsetTypes.OddRowsRight),
                new Coordinate2D(0, 1, OffsetTypes.OddRowsRight)
            };
            var expectedPassableNeighbors = Coordinate2D.To3D(expectedPassableNeighbors2d);

            Assert.That(graph.GetPassableNeighbors(center2d), Is.EqualTo(expectedPassableNeighbors2d));
            Assert.That(graph.GetPassableNeighbors(center), Is.EqualTo(expectedPassableNeighbors));
        }

        [Test]
        public void GetRange_ShouldGetCorrectRange()
        {
            var graph = GraphFactory.CreateRectangularGraph(6, 7, MovementTypesFixture.GetMovementTypes(),
                MovementTypesFixture.Ground);
            var center2d = new Coordinate2D(3, 2, OffsetTypes.OddRowsRight);
            var center = center2d.To3D();

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

            Assert.That(range, Is.EquivalentTo(expectedMovementRange));

            Assert.That(graph.GetRange(center2d, 2), Is.EquivalentTo(expectedRange2D));
        }

        [Test]
        public void GetShortestPath_ShouldBeAbleToFindingShortestPath()
        {
            // Note: this method uses AStarSearch class inside.
            // AStarSerach has its own comprehensive tests, so this test is only to ensure that this method exists and
            // returns something meaningful.
            var graph = GraphFactory.CreateRectangularGraph(3,
                3, MovementTypesFixture.GetMovementTypes(), MovementTypesFixture.Ground);
            var start2d = new Coordinate2D(0, 0, OffsetTypes.OddRowsRight);
            var start = start2d.To3D();
            var goal2d = new Coordinate2D(2, 2, OffsetTypes.OddRowsRight);
            var goal = goal2d.To3D();
            var expectedPath2d = new List<Coordinate2D>
            {
                new Coordinate2D(1, 0, OffsetTypes.OddRowsRight),
                new Coordinate2D(1, 1, OffsetTypes.OddRowsRight),
                goal2d
            };
            var expectedShortestPath = Coordinate2D.To3D(expectedPath2d);

            var shortestPath2d = graph.GetShortestPath(start2d, goal2d, MovementTypesFixture.Walking);
            var shortestPath = graph.GetShortestPath(start, goal, MovementTypesFixture.Walking);

            Assert.That(shortestPath2d, Is.EqualTo(expectedPath2d));
            Assert.That(shortestPath, Is.EqualTo(expectedShortestPath));
        }

        [Test]
        public void IsInBounds_ShouldReturnTrueIfThePositionIsWithinTheGraphBounds()
        {
            var graph = GraphFactory.CreateRectangularGraph(3, 3, MovementTypesFixture.GetMovementTypes(),
                MovementTypesFixture.Ground);
            var position = new Coordinate3D(0, -1, 1);

            Assert.That(graph.Contains(position), Is.True);

            position = new Coordinate3D(-1, 2, -1);
            Assert.That(graph.Contains(position), Is.False);

            Assert.That(graph.Contains(new Coordinate2D(1, 1, OffsetTypes.OddRowsRight)), Is.True);
            Assert.That(graph.Contains(new Coordinate2D(10, 10, OffsetTypes.OddRowsRight)), Is.False);
        }

        [Test]
        public void RemoveCells_ShouldRemoveAListOfCells()
        {
            var graph = GraphFactory.CreateRectangularGraph(3, 3, MovementTypesFixture.GetMovementTypes(),
                MovementTypesFixture.Ground);

            var cellsToRemove = new List<Coordinate2D>
            {
                new Coordinate2D(1, 0, OffsetTypes.OddRowsRight),
                new Coordinate2D(2, 0, OffsetTypes.OddRowsRight)
            }.ToList();
            var cellsToRemove3d = Coordinate2D.To3D(cellsToRemove);

            Assert.That(graph.GetAllCellsCoordinates(), Does.Contain(cellsToRemove3d[0]));
            Assert.That(graph.GetAllCellsCoordinates(), Does.Contain(cellsToRemove3d[1]));

            graph.RemoveCells(cellsToRemove);

            Assert.That(graph.GetAllCellsCoordinates(), Does.Not.Contain(cellsToRemove3d[0]));
            Assert.That(graph.GetAllCellsCoordinates(), Does.Not.Contain(cellsToRemove3d[1]));
        }

        [Test]
        public void SetManyCellsTerrainType_ShouldSetTerrainTypesToCells()
        {
            var graph = GraphFactory.CreateRectangularGraph(3, 3, MovementTypesFixture.GetMovementTypes(),
                MovementTypesFixture.Ground);

            var coordinateToSet = new Coordinate2D(2, 1, OffsetTypes.OddRowsRight);

            graph.SetCellsTerrainType(coordinateToSet, MovementTypesFixture.Water);
            Assert.That(graph.GetCellState(coordinateToSet).TerrainType, Is.EqualTo(MovementTypesFixture.Water));

            var coordinatesToSet = Coordinate2D.To3D(new List<Coordinate2D>
            {
                new Coordinate2D(0, 1, OffsetTypes.OddRowsRight),
                new Coordinate2D(0, 2, OffsetTypes.OddRowsRight)
            });

            graph.SetCellsTerrainType(coordinatesToSet, MovementTypesFixture.Water);
            foreach (var coordinate in coordinatesToSet)
                Assert.That(graph.GetCellState(coordinate).TerrainType, Is.EqualTo(MovementTypesFixture.Water));

            graph.SetCellsTerrainType(new List<Coordinate2D>
            {
                new Coordinate2D(0, 1, OffsetTypes.OddRowsRight),
                new Coordinate2D(0, 2, OffsetTypes.OddRowsRight)
            }, MovementTypesFixture.Air);
            foreach (var coordinate in coordinatesToSet)
                Assert.That(graph.GetCellState(coordinate).TerrainType, Is.EqualTo(MovementTypesFixture.Air));
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

            Assert.That(graph.IsCellBlocked(new Coordinate2D(1, 1, OffsetTypes.OddRowsRight)), Is.False);
            graph.BlockCells(new Coordinate2D(1, 1, OffsetTypes.OddRowsRight));
            Assert.That(graph.IsCellBlocked(new Coordinate2D(1, 1, OffsetTypes.OddRowsRight)), Is.True);
            graph.UnblockCells(new Coordinate2D(1, 1, OffsetTypes.OddRowsRight));
            Assert.That(graph.IsCellBlocked(new Coordinate2D(1, 1, OffsetTypes.OddRowsRight)), Is.False);
        }

        [Test]
        public void UnblockCells_ShouldUnblockCells_WhenCellListIsPassed()
        {
            var graph = GraphFactory.CreateRectangularGraph(3, 3, MovementTypesFixture.GetMovementTypes(),
                MovementTypesFixture.Ground);

            var cellsToBlock = new List<Coordinate2D>
            {
                new Coordinate2D(2, 0, OffsetTypes.OddRowsRight),
                new Coordinate2D(1, 2, OffsetTypes.OddRowsRight)
            };

            Assert.That(graph.IsCellBlocked(cellsToBlock[0]), Is.False);
            Assert.That(graph.IsCellBlocked(cellsToBlock[1]), Is.False);

            graph.BlockCells(cellsToBlock);

            Assert.That(graph.IsCellBlocked(cellsToBlock[0]), Is.True);
            Assert.That(graph.IsCellBlocked(cellsToBlock[1]), Is.True);

            graph.UnblockCells(cellsToBlock);

            Assert.That(graph.IsCellBlocked(cellsToBlock[0]), Is.False);
            Assert.That(graph.IsCellBlocked(cellsToBlock[1]), Is.False);

            graph.BlockCells(cellsToBlock);

            Assert.That(graph.IsCellBlocked(cellsToBlock[0]), Is.True);
            Assert.That(graph.IsCellBlocked(cellsToBlock[1]), Is.True);

            graph.UnblockCells(Coordinate2D.To3D(cellsToBlock));

            Assert.That(graph.IsCellBlocked(cellsToBlock[0]), Is.False);
            Assert.That(graph.IsCellBlocked(cellsToBlock[1]), Is.False);
        }
    }
}