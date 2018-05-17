using System;
using HexCore.DataStructures;

namespace HexCore.HexGraph
{
    [Serializable]
    public class CellState
    {
        public Coordinate2D Position;

        public Coordinate3D Coordinate;

        // Never set this field directly, only through graph.SetCellBlocked, since changing cell state requires
        // rebuild of some graph coordinates.
        public bool IsBlocked;
        public MovementType MovementType;

        public CellState(Coordinate2D position, bool isBlocked, Coordinate3D coordinate, MovementType movementType)
        {
            Position = position;
            IsBlocked = isBlocked;
            Coordinate = coordinate;
            MovementType = movementType;
        }
    }
}