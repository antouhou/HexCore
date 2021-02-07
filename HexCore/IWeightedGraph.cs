using System.Collections.Generic;

namespace HexCore
{
    public interface IWeightedGraph
    {
        int GetMovementCostForTheType(Coordinate3D a, MovementType unitMovementType);
        List<Coordinate3D> GetPassableNeighbors(Coordinate3D id);
        void ReturnListToPool(List<Coordinate3D> list);
        List<Coordinate3D> GetListFromPool();
    }
}