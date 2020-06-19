using System.Collections.Generic;
using HexCore.DataStructures;
using HexCore.HexGraph;

namespace HexCore.AStar
{
    public interface IWeightedGraph
    {
        int GetMovementCost(Coordinate3D a, IMovementType unitMovementType);
        IEnumerable<Coordinate3D> GetPassableNeighbors(Coordinate3D id);
    }
}