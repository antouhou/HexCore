using HexCore.BattleCore;
using HexCore.BattleCore.Unit;
using HexCore.HexGraph;

namespace Tests.Fixtures
{
    public class UnitFactory
    {
        private readonly Graph _graph;

        public UnitFactory(Graph graph)
        {
            _graph = graph;
        }

        public UnitController GetUnit(MovementType movementType, int movementPoint, int attackRange)
        {
            var unitState = new UnitState(movementType, movementPoint) {Attack = new Attack {Range = attackRange}};
            return new UnitController(unitState, _graph);
        }

        public UnitController GetBasicMeele()
        {
            var unitState = new UnitState(MovementTypes.Ground, 2) {Attack = new Attack {Range = 1}};
            return new UnitController(unitState, _graph);
        }
    }
}