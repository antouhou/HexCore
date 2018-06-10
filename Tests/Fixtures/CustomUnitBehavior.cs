using System;
using System.Collections.Generic;
using HexCore.BattleCore.Unit;
using HexCore.DataStructures;
using HexCore.HexGraph;

namespace Tests.Fixtures
{
    public class CustomUnitBehavior: IUnitBehavior<CustomUnitState>
    {
        private readonly Graph _graph;

        public CustomUnitBehavior(CustomUnitState unitState, Graph graph)
        {
            if (graph.IsCellBlocked(unitState.Position))
            {
                throw new InvalidOperationException("Can't initialize unit; Provided position is not empty");
            }
            State = unitState;
            _graph = graph;
            _graph.SetOneCellBlocked(State.Position, true);
        }

        public CustomUnitState State { get; }

        public List<Coordinate3D> GetAttackRange()
        {
            return _graph.GetAllCellsCoordinates();
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
        
        public int GetAttackPower()
        {
            return 1;
        }

        public bool MoveTo(Coordinate3D coordinate3D)
        {
            if (!CanMoveTo(coordinate3D)) return false;
            _graph.SetOneCellBlocked(coordinate3D, true);
            _graph.SetOneCellBlocked(State.Position, false);
            State.Position = coordinate3D;
            return true;

        }

        public void Attack(IUnitBehavior<IUnitState> unit)
        {
            throw new NotImplementedException();
        }
    }
}