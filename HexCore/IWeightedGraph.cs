using System.Collections.Generic;

namespace HexCore
{
    public interface IWeightedGraph
    {
        int GetMovementCostForTheType(Coordinate3D a, IMovementType unitMovementType);
        IEnumerable<Coordinate3D> GetPassableNeighbors(Coordinate3D id);
    }
}