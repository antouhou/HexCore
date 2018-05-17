using System;
using System.Collections.Generic;
using HexCore.DataStructures;
using HexCore.HexGraph;

namespace HexCore.AStar
{
    /**
     * This is pathfinding algorythm called 'A*'. It's one of the most common pathfindg algorythms.
     * In order to work it needs a gird's graph that's implemented in HexGridGraph class.
     */
    public static class AStarSearch
    {
        // It's just returns a distance to the goal.
        private static double Heuristic(Coordinate3D a, Coordinate3D b)
        {
            return (Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z)) / 2;
        }

        /**
         * In order to find shortest path between two points, you need to:
         * var path = new AStartSearch(graph, start, goal).ReconstructPath();
         * TODO: Having two methods looks redundant. Needs to be rafctored.
         */
        public static List<Coordinate3D> FindPath(IWeightedGraph graph, Coordinate3D start, Coordinate3D goal,
            MovementType unitMovementType)
        {
            var costSoFar = new Dictionary<Coordinate3D, int>();
            var cameFrom = new Dictionary<Coordinate3D, Coordinate3D>();
            var frontier = new PriorityQueue<Coordinate3D>();

            frontier.Enqueue(start, 0);
            cameFrom[start] = start;
            costSoFar[start] = 0;

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                if (current.Equals(goal))
                {
                    break;
                }

                foreach (var next in graph.GetPassableNeighbors(current))
                {
                    var newCost = costSoFar[current] + graph.GetMovementCost(next, unitMovementType);
                    if (costSoFar.ContainsKey(next) && newCost >= costSoFar[next]) continue;
                    costSoFar[next] = newCost;
                    var priority = newCost + Heuristic(next, goal);
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                }
            }

            // Reconstructing path
            var curr = goal;
            var path = new List<Coordinate3D>();
            while (!curr.Equals(start))
            {
                path.Add(curr);
                curr = cameFrom[curr];
            }

            //path.Add(_start); // optional
            // Reverse it to start at actual start point
            path.Reverse(); // optional
            return path;
        }
    }
}