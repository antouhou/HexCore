using System;
using System.Collections.Generic;

namespace HexCore
{
    /**
     * This is path finding algorithm called 'A*'. It's one of the most common pathfindg algorithms.
     * In order to work it needs a grid's graph that's implemented in HexGridGraph class.
     */
    public static class AStarSearch
    {
        // Returns a distance to the goal.
        private static double Heuristic(Coordinate3D a, Coordinate3D b)
        {
            return (Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z)) / 2.0;
        }

        public static List<Coordinate3D> FindShortestPath(IWeightedGraph graph, Coordinate3D start, Coordinate3D goal,
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

                if (current.Equals(goal)) break;

                foreach (var next in graph.GetPassableNeighbors(current))
                {
                    var newCost = costSoFar[current] + graph.GetMovementCostForTheType(next, unitMovementType);
                    if (costSoFar.ContainsKey(next) && newCost >= costSoFar[next]) continue;
                    costSoFar[next] = newCost;
                    var priority = newCost + Heuristic(next, goal);
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                }
            }

            var path = new List<Coordinate3D>();
            var pathWasNotFound = !cameFrom.ContainsKey(goal);

            // Returning an empty list if the path wasn't found
            if (pathWasNotFound) return path;

            // Reconstructing path
            var curr = goal;
            while (!curr.Equals(start))
            {
                path.Add(curr);
                curr = cameFrom[curr];
            }

            // Reverse it to start at actual start point
            path.Reverse();
            return path;
        }
    }
}