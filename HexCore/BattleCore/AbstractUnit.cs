using System.Collections.Generic;
using HexCore.DataStructures;
using HexCore.HexGraph;

namespace HexCore.BattleCore
{
    public abstract class AbstractUnit
    {
        public abstract int MovementPoints { get; set; }
        public abstract MovementType MovementType { get; set; }
        public abstract Coordinate3D Position { get; set; }
        public abstract Attack Attack { get; set; }

        public abstract List<Coordinate3D> GetAttackRange(Graph graph);
        public abstract List<Coordinate3D> GetMovementRange(Graph graph);
        public abstract bool CanMoveTo(Coordinate3D position, Graph graph);
        public abstract bool CanAttack(AbstractUnit unit, Graph graph);
    }
}