using System;
using HexCore.DataStructures;

namespace HexCore.HexGraph
{
    [Serializable]
    public class CellState
    {
        public Coordinate3D Coordinate3;

        // Never set this field directly, only through graph.SetCellBlocked, since changing cell state requires
        // rebuild of some graph coordinates.
        public bool IsBlocked;
        public IMovementType MovementType;

        public CellState(bool isBlocked, Coordinate3D coordinate3, IMovementType movementType)
        {
            IsBlocked = isBlocked;
            Coordinate3 = coordinate3;
            MovementType = movementType;
        }
    }
}