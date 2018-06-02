using System;
using System.Collections.Generic;
using HexCore.DataStructures;
using HexCore.HexGraph;

namespace HexCore.BattleCore
{
    [Serializable]
    public class BasicUnit : AbstractUnit
    {
        private Attack _attack = new Attack {Range = 1};
        private int _movementPoints;
        private MovementType _movementType;
        private Coordinate3D _position;

        public BasicUnit(MovementType movementType, int movementPoints)
        {
            _movementPoints = movementPoints;
            _movementType = movementType;
        }

        public override int MovementPoints
        {
            get { return _movementPoints; }
            set { _movementPoints = value; }
        }

        public override MovementType MovementType
        {
            get { return _movementType; }
            set { _movementType = value; }
        }

        public override Coordinate3D Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public override Attack Attack
        {
            get { return _attack; }
            set { _attack = value; }
        }

        public override List<Coordinate3D> GetAttackRange(Graph graph)
        {
            return graph.GetRange(Position, Attack.Range);
        }

        public override List<Coordinate3D> GetMovementRange(Graph graph)
        {
            return graph.GetMovementRange(Position, MovementPoints, MovementType);
        }

        public override bool CanMoveTo(Coordinate3D position, Graph graph)
        {
            return GetMovementRange(graph).Contains(position);
        }

        public override bool CanAttack(AbstractUnit unit, Graph graph)
        {
            return GetAttackRange(graph).Contains(unit.Position);
        }
    }
}