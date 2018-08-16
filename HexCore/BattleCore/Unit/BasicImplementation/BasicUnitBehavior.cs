using System;
using System.Collections.Generic;
using HexCore.DataStructures;
using HexCore.HexGraph;

namespace HexCore.BattleCore.Unit.BasicImplementation
{
    [Serializable]
    public class BasicUnitBehavior : IUnitBehavior<BasicUnitState>
    {
        private readonly Graph _graph;

        public BasicUnitBehavior(BasicUnitState unitState, Graph graph)
        {
            if (graph.IsCellBlocked(unitState.Position))
                throw new InvalidOperationException("Can't initialize unit; Provided position is not empty");

            State = unitState;
            // TODO: move to constructor
            Defense = new BasicUnitDefense();
            // TODO: same as above
            Attack = new BasicUnitAttack();
            // TODO
            HealthPoints = 3;
            _graph = graph;
            _graph.SetOneCellBlocked(State.Position, true);
        }

        public BasicUnitAttack Attack { get; }
        public BasicUnitDefense Defense { get; }
        public double HealthPoints { get; set; }

        public BasicUnitState State { get; }

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

        public bool MoveTo(Coordinate3D coordinate3D)
        {
            if (!CanMoveTo(coordinate3D)) return false;
            _graph.SetOneCellBlocked(coordinate3D, true);
            _graph.SetOneCellBlocked(State.Position, false);
            State.Position = coordinate3D;
            return true;
        }

        public double GetAttackPower()
        {
            return 2.0;
        }

        public AttackResult PerformAttack(BasicUnitBehavior attackedUnitBehavior)
        {
            var attackPower = GetAttackPower();
            var damageDealt = attackPower - attackedUnitBehavior.Defense.GetBlockedDamageAmount(Attack, attackPower);
            attackedUnitBehavior.HealthPoints -= damageDealt;
            return new AttackResult
            {
                TotalDamageAmount = damageDealt,
                HpLeft = attackedUnitBehavior.HealthPoints
            };
        }
    }
}