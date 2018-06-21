using HexCore.HexGraph;
using HexCore.DataStructures;

namespace HexCore.BattleCore.Unit
{
    public interface IUnitState
    {
        int MovementPoints { get; set; }
        MovementType MovementType { get; set; }
        Coordinate3D Position { get; set; }
    }
}