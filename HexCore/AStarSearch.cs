using System;
using System.Collections.Generic;

namespace HexCore
{
    public struct Path
    {
        public int cost;
        public List<Coordinate3D> cells;
    }

    /**
     * This is path finding algorithm called 'A*'. It's one of the most common pathfindg algorithms.
     * In order to work it needs a grid's graph that's implemented in HexGridGraph class.
     */
    public static class AStarSearch
    {
        private static ObjectPoolProvider PoolProvider => Graph.PoolProvider;

        // Returns a distance to the goal.
        private static double Heuristic(Coordinate3D a, Coordinate3D b)
        {
            return (Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z)) / 2.0;
        }

        public static List<Coordinate3D> FindShortestPath(IWeightedGraph graph, Coordinate3D start, Coordinate3D goal,
            MovementType unitMovementType)
        {
            var costSoFar = PoolProvider.CoordinateIntDictionaryPool.Get();
            var cameFrom = PoolProvider.CoordinateDictionaryPool.Get();
            var frontier = PoolProvider.CoordinateQueuePool.Get();
            var path = graph.GetListFromPool();

            frontier.Enqueue(start, 0);
            cameFrom[start] = start;
            costSoFar[start] = 0;

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                if (current.Equals(goal)) break;

                var neighbors = graph.GetPassableNeighbors(current);

                foreach (var next in neighbors)
                {
                    var newCost = costSoFar[current] + graph.GetMovementCostForTheType(next, unitMovementType);
                    if (costSoFar.ContainsKey(next) && newCost >= costSoFar[next]) continue;
                    costSoFar[next] = newCost;
                    var priority = newCost + Heuristic(next, goal);
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                }

                graph.ReturnListToPool(neighbors);
            }

            var pathWasFound = cameFrom.ContainsKey(goal);
            if (!pathWasFound) return path;

            // Reconstructing path
            var curr = goal;
            while (!curr.Equals(start))
            {
                path.Add(curr);
                curr = cameFrom[curr];
            }

            // Reverse it to start at actual start point
            path.Reverse();

            PoolProvider.CoordinateIntDictionaryPool.Return(costSoFar);
            PoolProvider.CoordinateDictionaryPool.Return(cameFrom);
            PoolProvider.CoordinateQueuePool.Return(frontier);

            return path;
        }
    }
}