using System;
using System.Collections.Generic;
using HexCore.DataStructures;
using HexCore.HexGraph;

namespace HexCore.BattleCore.Unit
{
    [Serializable]
    public class BasicUnitBehavior : IUnitBehavior<BasicUnitState>
    {
        private readonly Graph _graph;

        public BasicUnitState State { get; }
        public BasicUnitAttack Attack { get; }
        private BasicUnitDefense Defense { get; }

        public BasicUnitBehavior(BasicUnitState unitState, Graph graph)
        {
            if (graph.IsCellBlocked(unitState.Position))
            {
                throw new InvalidOperationException("Can't initialize unit; Provided position is not empty");
            }

            State = unitState;
            // TODO: move to constructor
            Defense = new BasicUnitDefense();
            // TODO: same as above
            Attack = new BasicUnitAttack();
            _graph = graph;
            _graph.SetOneCellBlocked(State.Position, true);
        }

        public List<Coordinate3D> GetAttackRange()
        {
            return _graph.GetRange(State.Position, State.Attack.Range);
        }

        public List<Coordinate3D> GetMovementRange()
        {
            return _graph.GetMovementRange(State.Position, State.MovementPoints, State.MovementType);
        }

        public bool CanMoveTo(Coordinate3D position)
        {
            return GetMovementRange().Contains(position);
        }

        public bool CanAttack(IUnitBehavior<IUnitState> unit)
        {
            return GetAttackRange().Contains(unit.State.Position);
        }

        public double GetAttackPower()
        {
            return 2.0;
        }

        public AttackResult RecieveAttack(BasicUnitAttack attack, double attackPower)
        {
            var damageTaken = attackPower - Defense.GetBlockedDamageAmount(attack, attackPower);
            return new AttackResult {totalDamage = damageTaken};
        }

        public bool MoveTo(Coordinate3D coordinate3D)
        {
            if (!CanMoveTo(coordinate3D)) return false;
            _graph.SetOneCellBlocked(coordinate3D, true);
            _graph.SetOneCellBlocked(State.Position, false);
            State.Position = coordinate3D;
            return true;
        }
    }
}