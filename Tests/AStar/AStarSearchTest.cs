using NUnit.Framework;
using HexCore.AStar;
using HexCore.DataStructures;
using HexCore.HexGraph;
using Tests.Fixtures;
using System.Collections.Generic;
using HexCore.Helpers;

namespace Tests.AStar
{
    [TestFixture]
    public class AStarSearchTest
    {
        [Test]
        public void ShouldFindShortestPathWithoutObstacles()
        {
            var graph = new Graph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            Assert.That(graph.Columns.Count, Is.EqualTo(3));
            foreach (var row in graph.Columns)
            {
                Assert.That(row.Count, Is.EqualTo(3));
            }

            // Cube coordinates are not so intuitive when it comes to visualizing them in your head, so let's use 
            // offset ones and convert them to cube. Cube coordinate are used by the algorythm because it's
            // much easier to operate them when in comes to actual algorythms

            // From bottom right 
            var startOddR = new Coordinate2D(2, 2);
            // To top left
            var goalOddR = new Coordinate2D(0, 0);
            var start = CoordinateConverter.ConvertOneOffsetToCube(OffsetTypes.OddRowsRight, startOddR);
            var goal = CoordinateConverter.ConvertOneOffsetToCube(OffsetTypes.OddRowsRight, goalOddR);

            // Start point is excluded from the path
            var expectedOffsetPath = new List<Coordinate2D>
            {
                // From 2, 2 we move to 1,1, which is central
                new Coordinate2D(1, 1),
                // From 1,1 we move to 1,0, since there is no direct connection between 1,1 and 0,0
                new Coordinate2D(1, 0),
                // And then moving to our goal.
                goalOddR
            };
            var expectedPath =
                CoordinateConverter.ConvertManyOffsetToCube(OffsetTypes.OddRowsRight, expectedOffsetPath);

            // For the simplest test we assume that all cells have type ground, as well as a unit
            List<Coordinate3D> path = AStarSearch.FindPath(graph, start, goal, MovementTypes.Ground);


            // For easier debugging
            var restoredExpected = CoordinateConverter.ConvertManyCubeToOffset(OffsetTypes.OddRowsRight, expectedPath);
            var restoredActual = CoordinateConverter.ConvertManyCubeToOffset(OffsetTypes.OddRowsRight, path);

            Assert.That(path.Count, Is.EqualTo(expectedPath.Count));
            for (int i = 0; i < expectedPath.Count; i++)
            {
                Assert.That(path[i], Is.EqualTo(expectedPath[i]));
            }
        }

        [Test]
        public void ShouldFindShortestPathWithObstacles()
        {
            // Now let's block center, 1,1
            var graph = new Graph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            Assert.That(graph.Columns.Count, Is.EqualTo(3));
            foreach (var row in graph.Columns)
            {
                Assert.That(row.Count, Is.EqualTo(3));
            }

            graph.SetCellBlocked(new Coordinate2D(1, 1));
            Assert.That(graph.Columns[1][1].IsBlocked, Is.True);

            // Same as in prevoius test
            var startOddR = new Coordinate2D(2, 2);
            var goalOddR = new Coordinate2D(0, 0);
            var start = CoordinateConverter.ConvertOneOffsetToCube(OffsetTypes.OddRowsRight, startOddR);
            var goal = CoordinateConverter.ConvertOneOffsetToCube(OffsetTypes.OddRowsRight, goalOddR);

            var expectedOffsetPath = new List<Coordinate2D>
            {
                // But this time we can't go to 1,1, since it's blocked. Instead, we are going to the left - 1,2 first
                new Coordinate2D(1, 2),
                // From there we can move up and right
                new Coordinate2D(0, 1),
                // And from there we can go to our final goal.
                goalOddR
            };
            var expectedPath =
                CoordinateConverter.ConvertManyOffsetToCube(OffsetTypes.OddRowsRight, expectedOffsetPath);

            List<Coordinate3D> path = AStarSearch.FindPath(graph, start, goal, MovementTypes.Ground);

            var restoredExpected = CoordinateConverter.ConvertManyCubeToOffset(OffsetTypes.OddRowsRight, expectedPath);
            var restoredActual = CoordinateConverter.ConvertManyCubeToOffset(OffsetTypes.OddRowsRight, path);

            Assert.That(path.Count, Is.EqualTo(expectedPath.Count));
            for (int i = 0; i < expectedPath.Count; i++)
            {
                Assert.That(path[i], Is.EqualTo(expectedPath[i]));
            }

            // Let's block 0,1 and move our starting point to bottom left
            graph.SetCellBlocked(new Coordinate2D(0, 1));
            startOddR = new Coordinate2D(0, 2);
            start = CoordinateConverter.ConvertOneOffsetToCube(OffsetTypes.OddRowsRight, startOddR);

            expectedOffsetPath = new List<Coordinate2D>
            {
                // Now we need to go through all corners - first let's go to the bottom right
                new Coordinate2D(1, 2),
                new Coordinate2D(2, 2),
                // Up to the top right
                new Coordinate2D(2, 1),
                new Coordinate2D(2, 0),
                // And from there we can go left until we reach our goal
                new Coordinate2D(1, 0),
                goalOddR
            };
            expectedPath =
                CoordinateConverter.ConvertManyOffsetToCube(OffsetTypes.OddRowsRight, expectedOffsetPath);

            path = AStarSearch.FindPath(graph, start, goal, MovementTypes.Ground);

            restoredExpected = CoordinateConverter.ConvertManyCubeToOffset(OffsetTypes.OddRowsRight, expectedPath);
            restoredActual = CoordinateConverter.ConvertManyCubeToOffset(OffsetTypes.OddRowsRight, path);

            Assert.That(path.Count, Is.EqualTo(expectedPath.Count));
            for (int i = 0; i < expectedPath.Count; i++)
            {
                Assert.That(path[i], Is.EqualTo(expectedPath[i]));
            }
        }

        [Test]
        public void ShouldFindShortestPathWhenThereIsPenalties()
        {
            // Everything is like before, but now instead of blocking 1,1 let's make it water to apply some penalties 
            // to our ground moving type
            var graph = new Graph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            Assert.That(graph.Columns.Count, Is.EqualTo(3));
            foreach (var row in graph.Columns)
            {
                Assert.That(row.Count, Is.EqualTo(3));
            }

            graph.SetCellMovementType(new Coordinate2D(1, 1), MovementTypes.Water);
            Assert.That(graph.Columns[1][1].MovementType.Name, Is.EqualTo(MovementTypes.Water.Name));

            // And we expect to achieve same result - even through 1,1 is not blocked
            Assert.That(graph.Columns[1][1].IsBlocked, Is.False);

            var startOddR = new Coordinate2D(2, 2);
            var goalOddR = new Coordinate2D(0, 0);
            var start = CoordinateConverter.ConvertOneOffsetToCube(OffsetTypes.OddRowsRight, startOddR);
            var goal = CoordinateConverter.ConvertOneOffsetToCube(OffsetTypes.OddRowsRight, goalOddR);

            var expectedOffsetPath = new List<Coordinate2D>
            {
                // But this time we can't go to 1,1, since there is movement penalty. Instead, we are going to the left - 1,2 first
                new Coordinate2D(1, 2),
                // From there we can move up and right
                new Coordinate2D(0, 1),
                // And from there we can go to our final goal.
                goalOddR
            };
            var expectedPath =
                CoordinateConverter.ConvertManyOffsetToCube(OffsetTypes.OddRowsRight, expectedOffsetPath);

            List<Coordinate3D> path = AStarSearch.FindPath(graph, start, goal, MovementTypes.Ground);

            var restoredExpected = CoordinateConverter.ConvertManyCubeToOffset(OffsetTypes.OddRowsRight, expectedPath);
            var restoredActual = CoordinateConverter.ConvertManyCubeToOffset(OffsetTypes.OddRowsRight, path);

            Assert.That(path.Count, Is.EqualTo(expectedPath.Count));
            for (int i = 0; i < expectedPath.Count; i++)
            {
                Assert.That(path[i], Is.EqualTo(expectedPath[i]));
            }

            // Let's make 0,1 water too and move our starting point to bottom left
            graph.SetCellMovementType(new Coordinate2D(0, 1), MovementTypes.Water);
            Assert.That(graph.Columns[0][1].MovementType.Name, Is.EqualTo(MovementTypes.Water.Name));
            startOddR = new Coordinate2D(0, 2);
            start = CoordinateConverter.ConvertOneOffsetToCube(OffsetTypes.OddRowsRight, startOddR);

            // What's different from previous test - even if 0,1 is water, if we go from the bottom left
            // to the top left - go through water still we preferable - path length will be only two cells, but because
            // of penalty it'll cost 3 movement point. Going through all corners will take 6 points - 6 cells, 1 point each.
            expectedOffsetPath = new List<Coordinate2D>
            {
                // Going up to the water
                new Coordinate2D(0, 1),
                goalOddR
            };
            expectedPath =
                CoordinateConverter.ConvertManyOffsetToCube(OffsetTypes.OddRowsRight, expectedOffsetPath);

            path = AStarSearch.FindPath(graph, start, goal, MovementTypes.Ground);

            restoredExpected = CoordinateConverter.ConvertManyCubeToOffset(OffsetTypes.OddRowsRight, expectedPath);
            restoredActual = CoordinateConverter.ConvertManyCubeToOffset(OffsetTypes.OddRowsRight, path);

            Assert.That(path.Count, Is.EqualTo(expectedPath.Count));
            for (int i = 0; i < expectedPath.Count; i++)
            {
                Assert.That(path[i], Is.EqualTo(expectedPath[i]));
            }
        }

        [Test]
        public void ShouldFindShortestPathWhenThereIsPenaltiesAndObstacles()
        {
            // Now let's make 1,1 water and block 1,2
            var graph = new Graph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            Assert.That(graph.Columns.Count, Is.EqualTo(3));
            foreach (var row in graph.Columns)
            {
                Assert.That(row.Count, Is.EqualTo(3));
            }

            graph.SetCellBlocked(new Coordinate2D(1, 2));
            graph.SetCellMovementType(new Coordinate2D(1, 1), MovementTypes.Water);
            Assert.That(graph.Columns[1][1].MovementType.Name, Is.EqualTo(MovementTypes.Water.Name));
            Assert.That(graph.Columns[1][2].IsBlocked, Is.True);

            var startOddR = new Coordinate2D(2, 2);
            var goalOddR = new Coordinate2D(0, 0);
            var start = CoordinateConverter.ConvertOneOffsetToCube(OffsetTypes.OddRowsRight, startOddR);
            var goal = CoordinateConverter.ConvertOneOffsetToCube(OffsetTypes.OddRowsRight, goalOddR);

            // Now we have two shortest paths - 1,1, 1,0, 0,0 costs 4, since there is a penalty on 1,1
            // And 2,1, 2,0, 1,0 0,0, costs 4 too. It's 1 cell longer, but there is no penalties.
            // We are expecting to take path 1 because of the heuristics - it's leades to our goal a bit more stright.
            var expectedOffsetPath = new List<Coordinate2D>
            {
                new Coordinate2D(1, 1),
                new Coordinate2D(1, 0),
                goalOddR
            };
            var expectedPath =
                CoordinateConverter.ConvertManyOffsetToCube(OffsetTypes.OddRowsRight, expectedOffsetPath);

            List<Coordinate3D> path = AStarSearch.FindPath(graph, start, goal, MovementTypes.Ground);

            var restoredExpected = CoordinateConverter.ConvertManyCubeToOffset(OffsetTypes.OddRowsRight, expectedPath);
            var restoredActual = CoordinateConverter.ConvertManyCubeToOffset(OffsetTypes.OddRowsRight, path);

            Assert.That(path.Count, Is.EqualTo(expectedPath.Count));
            for (int i = 0; i < expectedPath.Count; i++)
            {
                Assert.That(path[i], Is.EqualTo(expectedPath[i]));
            }
        }
    }
}