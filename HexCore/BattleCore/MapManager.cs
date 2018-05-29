using System.Collections.Generic;
using HexCore.DataStructures;
using HexCore.HexGraph;

namespace HexCore.BattleCore
{
    public class MapManager
    {
        public Graph Graph;
        public List<UnitState> Units = new List<UnitState>();

        public MapManager(Graph graph)
        {
            Graph = graph;
        }

        public List<Coordinate3D> GetMovableArea(UnitState unit)
        {
            return Graph.GetMovableArea(unit.Coordinate3D, unit.MovementPoints, unit.MovementType);
        }

        public bool IsUnitAbleToMoveTo(UnitState unit, Coordinate3D coordinate3D)
        {
            var possibleMovementArea = GetMovableArea(unit);
            return possibleMovementArea.Contains(coordinate3D);
        }

        public bool MoveUnitTo(UnitState unit, Coordinate3D coordinate3D)
        {
            if (!IsUnitAbleToMoveTo(unit, coordinate3D)) return false;
            Graph.SetOneCellBlocked(coordinate3D, true);
            Graph.SetOneCellBlocked(unit.Coordinate3D, false);
            unit.Coordinate3D = coordinate3D;
            return true;
        }

        public bool AddUnit(UnitState unit)
        {
            var randomEmptyCell = Graph.GetRandomEmptyCoordinate3D();
            if (randomEmptyCell == null) return false;
            unit.Coordinate3D = randomEmptyCell.Value;
            Graph.SetOneCellBlocked(unit.Coordinate3D, true);
            Units.Add(unit);
            return true;
        }

        public void RemoveUnit(UnitState unit)
        {
            Graph.SetOneCellBlocked(unit.Coordinate3D, false);
            Units.Remove(unit);
        }

        public bool IsCellBlocked(Coordinate3D cellCoordinate)
        {
            return Graph.GetCellStateByCoordinate3D(cellCoordinate).IsBlocked;
        }
    }
}