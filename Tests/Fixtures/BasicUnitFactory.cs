using System;
using HexCore.BattleCore;
using HexCore.BattleCore.Unit;
using HexCore.HexGraph;

namespace Tests.Fixtures
{
    public class BasicUnitFactory
    {
        private readonly Graph _graph;

        public BasicUnitFactory(Graph graph)
        {
            _graph = graph;
        }

        public BasicUnitBehavior<BaseUnitState> GetUnit(MovementType movementType, int movementPoint, int attackRange)
        {
            var position = _graph.GetRandomEmptyCellCoordinate();
            if (position == null)
            {
                throw new InvalidOperationException("Can't add more units to graph; Graph is full");
            }
            var unitState = new BaseUnitState(movementType, movementPoint)
            {
                Attack = new Attack {Range = attackRange}, Position = position.Value
            };
            return new BasicUnitBehavior<BaseUnitState>(unitState, _graph);
            
        }

        public BasicUnitBehavior<BaseUnitState> GetBasicMeele()
        {
            var position = _graph.GetRandomEmptyCellCoordinate();
            if (position == null)
            {
                throw new InvalidOperationException("Can't add more units to graph; Graph is full");
            }
            var unitState = new BaseUnitState(MovementTypes.Ground, 2)
            {
                Attack = new Attack {Range = 1}, Position = position.Value
            };
            return new BasicUnitBehavior<BaseUnitState>(unitState, _graph);
        }
    }
}