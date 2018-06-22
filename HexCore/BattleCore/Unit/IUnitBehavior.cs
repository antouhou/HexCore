using System.Collections.Generic;
using HexCore.DataStructures;

namespace HexCore.BattleCore.Unit
{
    public interface IUnitBehavior<out TUnitState>
    {
        TUnitState State { get; }
        List<Coordinate3D> GetAttackRange();
        List<Coordinate3D> GetMovementRange();
        bool CanMoveTo(Coordinate3D position);
        bool CanAttack(IUnitBehavior<IUnitState> unit);
        bool MoveTo(Coordinate3D coordinate3D);
    }
}