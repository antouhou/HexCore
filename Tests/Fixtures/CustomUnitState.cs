using HexCore.BattleCore;
using HexCore.BattleCore.Unit;
using HexCore.DataStructures;
using HexCore.HexGraph;

namespace Tests.Fixtures
{
    public class CustomUnitState : IUnitState
    {
        public CustomUnitState(MovementType movementType, int movementPoints)
        {
            MovementType = movementType;
            MovementPoints = movementPoints;
        }

        public Attack Attack { get; set; } = new Attack {Range = 1};

        public int MovementPoints { get; set; }
        public MovementType MovementType { get; set; }
        public Coordinate3D Position { get; set; }
    }
}