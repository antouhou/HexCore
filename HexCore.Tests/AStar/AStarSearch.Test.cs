using System.Collections.Generic;
using HexCore.AStar;
using HexCore.DataStructures;
using HexCore.HexGraph;
using HexCoreTests.Fixtures;
using NUnit.Framework;

namespace HexCoreTests.AStar
{
    [TestFixture]
    public class AStarSearchTest
    {
        [Test]
        public void FindShortestPath_ShouldFindShortestPathOnBiggerGraph()
        {
            // This test wouldn't be that different from previous ones, except size of the graph
            // Not so square anymore! 7 columns, 10 rows.
            var graph = GraphFactory.CreateRectangularGraph(7, 10, MovementTypesFixture.GetMovementTypes(), MovementTypesFixture.Ground,
                OffsetTypes.OddRowsRight);

            // First, let's do simple test - from 5,6 to 1,2 without obstacles
            var startOddR = new Coordinate2D(5, 6, OffsetTypes.OddRowsRight);
            var goalOddR = new Coordinate2D(1, 2, OffsetTypes.OddRowsRight);
            var start = startOddR.To3D();
            var goal = goalOddR.To3D();

            // We expect algo to go up by diagonal and then turn left
            var expectedOffsetPath = new List<Coordinate2D>
            {
                // Go up and left
                new Coordinate2D(4, 5, OffsetTypes.OddRowsRight),
                new Coordinate2D(4, 4, OffsetTypes.OddRowsRight),
                new Coordinate2D(3, 3, OffsetTypes.OddRowsRight),
                new Coordinate2D(3, 2, OffsetTypes.OddRowsRight),
                // And now just left until the goal is reached
                new Coordinate2D(2, 2, OffsetTypes.OddRowsRight),
                goalOddR
            };
            var expectedPath = Coordinate2D.To3D(expectedOffsetPath);

            var path = AStarSearch.FindShortestPath(graph, start, goal, MovementTypesFixture.Ground);

            Assert.That(path, Is.EqualTo(expectedPath));

            // Good! Now let's block some of them, and also let's add a lake in the middle.
            graph.SetManyCellsBlocked(Coordinate2D.To3D(new List<Coordinate2D>
            {
                new Coordinate2D(4, 6, OffsetTypes.OddRowsRight),
                new Coordinate2D(4, 5, OffsetTypes.OddRowsRight),
                new Coordinate2D(4, 7, OffsetTypes.OddRowsRight),
                new Coordinate2D(5, 5, OffsetTypes.OddRowsRight)
            }), true);
            graph.SetManyCellsMovementType(Coordinate2D.To3D(new List<Coordinate2D>
            {
                new Coordinate2D(4, 2, OffsetTypes.OddRowsRight),
                new Coordinate2D(3, 2, OffsetTypes.OddRowsRight),
                new Coordinate2D(2, 2, OffsetTypes.OddRowsRight),
                new Coordinate2D(1, 3, OffsetTypes.OddRowsRight),
                new Coordinate2D(2, 3, OffsetTypes.OddRowsRight),
                new Coordinate2D(3, 3, OffsetTypes.OddRowsRight)
            }), MovementTypesFixture.Water);

            //Let's see what's going to happen!
            expectedOffsetPath = new List<Coordinate2D>
            {
                // Avoiding obstacles
                new Coordinate2D(6, 6, OffsetTypes.OddRowsRight),
                new Coordinate2D(6, 5, OffsetTypes.OddRowsRight),
                new Coordinate2D(6, 4, OffsetTypes.OddRowsRight),
                // Going parallel to the bank
                new Coordinate2D(5, 4, OffsetTypes.OddRowsRight),
                new Coordinate2D(4, 4, OffsetTypes.OddRowsRight),
                new Coordinate2D(3, 4, OffsetTypes.OddRowsRight),
                new Coordinate2D(2, 4, OffsetTypes.OddRowsRight),
                // Now we are going to cross the water, since it's shortest available solution from this point 
                new Coordinate2D(1, 3, OffsetTypes.OddRowsRight),
                // And we are here.
                goalOddR
            };
            expectedPath = Coordinate2D.To3D(expectedOffsetPath);

            path = AStarSearch.FindShortestPath(graph, start, goal, MovementTypesFixture.Ground);

            Assert.That(path, Is.EqualTo(expectedPath));

            // Now let's check water movement type - it should prefer going through the water rather than the ground
            path = AStarSearch.FindShortestPath(graph, start, goal, MovementTypesFixture.Water);

            expectedOffsetPath = new List<Coordinate2D>
            {
                // Avoiding obstacles
                new Coordinate2D(6, 6, OffsetTypes.OddRowsRight),
                new Coordinate2D(6, 5, OffsetTypes.OddRowsRight),
                new Coordinate2D(6, 4, OffsetTypes.OddRowsRight),
                // Head right to the water
                new Coordinate2D(5, 3, OffsetTypes.OddRowsRight),
                new Coordinate2D(5, 2, OffsetTypes.OddRowsRight),
                // Swim
                new Coordinate2D(4, 2, OffsetTypes.OddRowsRight),
                new Coordinate2D(3, 2, OffsetTypes.OddRowsRight),
                new Coordinate2D(2, 2, OffsetTypes.OddRowsRight),
                // And we are here.
                goalOddR
            };
            expectedPath = Coordinate2D.To3D(expectedOffsetPath);

            Assert.That(path, Is.EqualTo(expectedPath));
        }

        [Test]
        public void FindShortestPath_ShouldFindShortestPathWhenThereIsPenalties()
        {
            // Everything is like before, but now instead of blocking 1,1 let's make it water to apply some penalties 
            // to our ground moving type
            var graph = GraphFactory.CreateRectangularGraph(3, 3, MovementTypesFixture.GetMovementTypes(), MovementTypesFixture.Ground,
                OffsetTypes.OddRowsRight);

            graph.SetOneCellMovementType(new Coordinate2D(1, 1, OffsetTypes.OddRowsRight).To3D(),
                MovementTypesFixture.Water);
            // And we expect to achieve same result - even through 1,1 is not blocked

            var startOddR = new Coordinate2D(2, 2, OffsetTypes.OddRowsRight);
            var goalOddR = new Coordinate2D(0, 0, OffsetTypes.OddRowsRight);
            var start = startOddR.To3D();
            var goal = goalOddR.To3D();

            var expectedOffsetPath = new List<Coordinate2D>
            {
                // But this time we can't go to 1,1, since there is movement penalty. Instead, we are going to the left - 1,2 first
                new Coordinate2D(1, 2, OffsetTypes.OddRowsRight),
                // From there we can move up and right
                new Coordinate2D(0, 1, OffsetTypes.OddRowsRight),
                // And from there we can go to our final goal.
                goalOddR
            };
            var expectedPath = Coordinate2D.To3D(expectedOffsetPath);

            var path = AStarSearch.FindShortestPath(graph, start, goal, MovementTypesFixture.Ground);

            Assert.That(path, Is.EqualTo(expectedPath));

            // Let's make 0,1 water too and move our starting point to bottom left
            graph.SetOneCellMovementType(new Coordinate2D(0, 1, OffsetTypes.OddRowsRight).To3D(),
                MovementTypesFixture.Water);
            startOddR = new Coordinate2D(0, 2, OffsetTypes.OddRowsRight);
            start = startOddR.To3D();

            // What's different from previous test - even if 0,1 is water, if we go from the bottom left
            // to the top left - go through water still we preferable - path length will be only two cells, but because
            // of penalty it'll cost 3 movement point. Going through all corners will take 6 points - 6 cells, 1 point each.
            expectedOffsetPath = new List<Coordinate2D>
            {
                // Going up to the water
                new Coordinate2D(0, 1, OffsetTypes.OddRowsRight),
                goalOddR
            };
            expectedPath = Coordinate2D.To3D(expectedOffsetPath);

            path = AStarSearch.FindShortestPath(graph, start, goal, MovementTypesFixture.Ground);

            Assert.That(path, Is.EqualTo(expectedPath));
        }

        [Test]
        public void FindShortestPath_ShouldFindShortestPathWhenThereIsPenaltiesAndObstacles()
        {
            // Now let's make 1,1 water and block 1,2
            var graph = GraphFactory.CreateRectangularGraph(3, 3, MovementTypesFixture.GetMovementTypes(), MovementTypesFixture.Ground,
                OffsetTypes.OddRowsRight);

            graph.SetOneCellBlocked(new Coordinate2D(1, 2, OffsetTypes.OddRowsRight).To3D(), true);
            graph.SetOneCellMovementType(new Coordinate2D(1, 1, OffsetTypes.OddRowsRight).To3D(),
                MovementTypesFixture.Water);

            var startOddR = new Coordinate2D(2, 2, OffsetTypes.OddRowsRight);
            var goalOddR = new Coordinate2D(0, 0, OffsetTypes.OddRowsRight);
            var start = startOddR.To3D();
            var goal = goalOddR.To3D();

            // Now we have two shortest paths - 1,1, 1,0, 0,0 costs 4, since there is a penalty on 1,1
            // And 2,1, 2,0, 1,0 0,0, costs 4 too. It's 1 cell longer, but there is no penalties.
            // We are expecting to take path 1 because of the heuristics - it's leades to our goal a bit more stright.
            var expectedOffsetPath = new List<Coordinate2D>
            {
                new Coordinate2D(1, 1, OffsetTypes.OddRowsRight),
                new Coordinate2D(1, 0, OffsetTypes.OddRowsRight),
                goalOddR
            };
            var expectedPath = Coordinate2D.To3D(expectedOffsetPath);

            var path = AStarSearch.FindShortestPath(graph, start, goal, MovementTypesFixture.Ground);

            Assert.That(path, Is.EqualTo(expectedPath));
        }

        [Test]
        public void FindShortestPath_ShouldFindShortestPathWithObstacles()
        {
            // Now let's block center, 1,1
            var graph = GraphFactory.CreateRectangularGraph(3, 3, MovementTypesFixture.GetMovementTypes(), MovementTypesFixture.Ground,
                OffsetTypes.OddRowsRight);

            graph.SetOneCellBlocked(new Coordinate2D(1, 1, OffsetTypes.OddRowsRight).To3D(), true);

            // Same as in prevoius test
            var startOddR = new Coordinate2D(2, 2, OffsetTypes.OddRowsRight);
            var goalOddR = new Coordinate2D(0, 0, OffsetTypes.OddRowsRight);
            var start = startOddR.To3D();
            var goal = goalOddR.To3D();

            var expectedOffsetPath = new List<Coordinate2D>
            {
                // But this time we can't go to 1,1, since it's blocked. Instead, we are going to the left - 1,2 first
                new Coordinate2D(1, 2, OffsetTypes.OddRowsRight),
                // From there we can move up and right
                new Coordinate2D(0, 1, OffsetTypes.OddRowsRight),
                // And from there we can go to our final goal.
                goalOddR
            };
            var expectedPath = Coordinate2D.To3D(expectedOffsetPath);

            var path = AStarSearch.FindShortestPath(graph, start, goal, MovementTypesFixture.Ground);

            Assert.That(path, Is.EqualTo(expectedPath));

            // Let's block 0,1 and move our starting point to bottom left
            graph.SetOneCellBlocked(new Coordinate2D(0, 1, OffsetTypes.OddRowsRight).To3D(), true);
            startOddR = new Coordinate2D(0, 2, OffsetTypes.OddRowsRight);
            start = startOddR.To3D();

            expectedOffsetPath = new List<Coordinate2D>
            {
                // Now we need to go through all corners - first let's go to the bottom right
                new Coordinate2D(1, 2, OffsetTypes.OddRowsRight),
                new Coordinate2D(2, 2, OffsetTypes.OddRowsRight),
                // Up to the top right
                new Coordinate2D(2, 1, OffsetTypes.OddRowsRight),
                new Coordinate2D(2, 0, OffsetTypes.OddRowsRight),
                // And from there we can go left until we reach our goal
                new Coordinate2D(1, 0, OffsetTypes.OddRowsRight),
                goalOddR
            };
            expectedPath = Coordinate2D.To3D(expectedOffsetPath);

            path = AStarSearch.FindShortestPath(graph, start, goal, MovementTypesFixture.Ground);

            Assert.That(path, Is.EqualTo(expectedPath));
        }

        [Test]
        public void FindShortestPath_ShouldFindShortestPathWithoutObstacles()
        {
            var graph = GraphFactory.CreateRectangularGraph(3, 3, MovementTypesFixture.GetMovementTypes(), MovementTypesFixture.Ground,
                OffsetTypes.OddRowsRight);

            // Cube coordinates are not so intuitive when it comes to visualizing them in your head, so let's use 
            // offset ones and convert them to cube. Cube coordinate are used by the algorythm because it's
            // much easier to operate them when in comes to actual algorythms

            // From bottom right 
            var startOddR = new Coordinate2D(2, 2, OffsetTypes.OddRowsRight);
            // To top left
            var goalOddR = new Coordinate2D(0, 0, OffsetTypes.OddRowsRight);
            var start = startOddR.To3D();
            var goal = goalOddR.To3D();

            // Start point is excluded from the path
            var expectedOffsetPath = new List<Coordinate2D>
            {
                // From 2, 2 we move to 1,1, which is central
                new Coordinate2D(1, 1, OffsetTypes.OddRowsRight),
                // From 1,1 we move to 1,0, since there is no direct connection between 1,1 and 0,0
                new Coordinate2D(1, 0, OffsetTypes.OddRowsRight),
                // And then moving to our goal.
                goalOddR
            };
            var expectedPath = Coordinate2D.To3D(expectedOffsetPath);

            // For the simplest test we assume that all cells have type ground, as well as a unit
            var path = AStarSearch.FindShortestPath(graph, start, goal, MovementTypesFixture.Ground);
            Assert.That(path, Is.EqualTo(expectedPath));
        }
    }
}