using System;
using HexCore.DataStructures;

namespace HexCore.HexGraph
{
    [Serializable]
    public class CellState
    {
        public Coordinate2D Coordinate2;
        public Coordinate3D Coordinate3;

        // Never set this field directly, only through graph.SetCellBlocked, since changing cell state requires
        // rebuild of some graph coordinates.
        public bool IsBlocked;
        public MovementType MovementType;

        public CellState(Coordinate2D coordinate2, bool isBlocked, Coordinate3D coordinate3, MovementType movementType)
        {
            Coordinate2 = coordinate2;
            IsBlocked = isBlocked;
            Coordinate3 = coordinate3;
            MovementType = movementType;
        }
    }
}