using HexCore.DataStructures;
using HexCore.HexGraph;

namespace HexCore.BattleCore
{
    public class UnitState
    {
        public readonly int MovementPoints;
        public readonly MovementType MovementType;
        public Coordinate3D Coordinate3D;

        public UnitState(MovementType movementType, int movementPoints)
        {
            MovementPoints = movementPoints;
            MovementType = movementType;
        }
    }
}