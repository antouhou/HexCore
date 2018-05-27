using HexCore.DataStructures;
using HexCore.HexGraph;

namespace HexCore.BattleCore
{
    public class UnitState
    {
        public Coordinate3D Coordinate3D;
        public int MovementPoints;
        public MovementType MovementType;

        public UnitState(MovementType movementType, int movementPoints)
        {
            //Coordinate3D = position;
            MovementPoints = movementPoints;
            MovementType = movementType;
        }
    }
}