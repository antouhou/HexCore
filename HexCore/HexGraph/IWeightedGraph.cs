using System.Collections.Generic;
using HexCore.DataStructures;

namespace HexCore.HexGraph
{
    public interface IWeightedGraph
    {
        int GetMovementCostForTheType(Coordinate3D a, IMovementType unitMovementType);
        IEnumerable<Coordinate3D> GetPassableNeighbors(Coordinate3D id);
    }
}