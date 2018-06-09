using System.Collections.Generic;
using HexCore.DataStructures;

namespace HexCore.BattleCore.Unit
{
    public interface IUnitBehavior<TUnitState>
    {
        List<Coordinate3D> GetAttackRange();
        List<Coordinate3D> GetMovementRange();
        bool CanMoveTo(Coordinate3D position);
        bool CanAttack(IUnitBehavior<TUnitState> unit);
        bool MoveTo(Coordinate3D coordinate3D);
        void Attack(IUnitBehavior<TUnitState> unit);
        TUnitState State { get; }
    }
}