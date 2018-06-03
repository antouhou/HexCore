using System.Collections.Generic;
using HexCore.BattleCore.Unit;
using HexCore.DataStructures;
using HexCore.HexGraph;

namespace HexCore.BattleCore
{
    public class MapManager
    {
        public readonly Graph Graph;
        public readonly List<AbstractUnitController> Units = new List<AbstractUnitController>();

        public MapManager(Graph graph)
        {
            Graph = graph;
        }

        public bool MoveUnitTo(UnitController unit, Coordinate3D coordinate3D)
        {
            if (!unit.CanMoveTo(coordinate3D)) return false;
            Graph.SetOneCellBlocked(coordinate3D, true);
            Graph.SetOneCellBlocked(unit.State.Position, false);
            unit.State.Position = coordinate3D;
            return true;
        }

        public bool AddUnit(AbstractUnitController unit)
        {
            var randomEmptyCell = Graph.GetCoordinateOfRandomEmptyCell();
            if (randomEmptyCell == null) return false;
            unit.State.Position = randomEmptyCell.Value;
            Graph.SetOneCellBlocked(unit.State.Position, true);
            Units.Add(unit);
            return true;
        }

        public void RemoveUnit(AbstractUnitController unit)
        {
            Graph.SetOneCellBlocked(unit.State.Position, false);
            Units.Remove(unit);
        }
    }
}