using System.Collections.Generic;
using HexCore.DataStructures;
using HexCore.HexGraph;

namespace HexCore.BattleCore
{
    public class MapManager
    {
        public List<int> Units;
        public Graph Graph;

        public List<Coordinate3D> GetMovableArea(Unit unit)
        {
            return Graph.GetMovableArea(unit.Coordinate3D, unit.MovementPoints, unit.MovementType);
        }

        public bool IsUnitAbleToMoveTo(Unit unit, Coordinate3D coordinate3D)
        {
            var possibleMovementArea = GetMovableArea(unit);
            return possibleMovementArea.Contains(coordinate3D);
        }

        public bool MoveUnitTo(Coordinate3D coordinate3D, Unit unit)
        {
            if (!IsUnitAbleToMoveTo(unit, coordinate3D)) return false;
            Graph.SetOneCellBlocked(coordinate3D, true);
            Graph.SetOneCellBlocked(unit.Coordinate3D, false);
            unit.Coordinate3D = coordinate3D;
            return true;
        }
    }
}