using System.Collections.Generic;
using HexCore.DataStructures;

namespace HexCore.BattleCore.Unit
{
    public abstract class AbstractUnitBehavior
    {
        public abstract UnitState State { get; }

        public abstract List<Coordinate3D> GetAttackRange();
        public abstract List<Coordinate3D> GetMovementRange();
        public abstract bool CanMoveTo(Coordinate3D position);
        public abstract bool CanAttack(AbstractUnitBehavior unit);
        public abstract void Attack(AbstractUnitBehavior unit);
    }
}