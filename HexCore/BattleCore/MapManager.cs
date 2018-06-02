using System.Collections.Generic;
using HexCore.DataStructures;
using HexCore.HexGraph;

namespace HexCore.BattleCore
{
    public class MapManager
    {
        public readonly Graph Graph;
        public readonly List<AbstractUnit> Units = new List<AbstractUnit>();

        public MapManager(Graph graph)
        {
            Graph = graph;
        }

        private bool CanUnitMoveTo(AbstractUnit unit, Coordinate3D coordinate3D)
        {
            return unit.CanMoveTo(coordinate3D, Graph);
        }

        public bool MoveUnitTo(BasicUnit unit, Coordinate3D coordinate3D)
        {
            if (!CanUnitMoveTo(unit, coordinate3D)) return false;
            Graph.SetOneCellBlocked(coordinate3D, true);
            Graph.SetOneCellBlocked(unit.Position, false);
            unit.Position = coordinate3D;
            return true;
        }

        public bool AddUnit(AbstractUnit unit)
        {
            var randomEmptyCell = Graph.GetRandomEmptyCoordinate3D();
            if (randomEmptyCell == null) return false;
            unit.Position = randomEmptyCell.Value;
            Graph.SetOneCellBlocked(unit.Position, true);
            Units.Add(unit);
            return true;
        }

        public void RemoveUnit(AbstractUnit unit)
        {
            Graph.SetOneCellBlocked(unit.Position, false);
            Units.Remove(unit);
        }

        public bool CanAttack(AbstractUnit attackingUnit, BasicUnit attackedUnit)
        {
            return attackingUnit.CanAttack(attackedUnit, Graph);
        }

        public void Attack(AbstractUnit attackingUnit, BasicUnit attackedUnit)
        {
        }
    }
}