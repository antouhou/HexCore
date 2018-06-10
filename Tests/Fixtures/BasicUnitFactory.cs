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

        public BasicUnitBehavior GetUnit(MovementType movementType, int movementPoint, int attackRange)
        {
            var position = _graph.GetRandomEmptyCellCoordinate();
            if (position == null)
            {
                throw new InvalidOperationException("Can't add more units to graph; Graph is full");
            }
            var unitState = new BasicUnitState(movementType, movementPoint)
            {
                Attack = new Attack {Range = attackRange}, Position = position.Value
            };
            return new BasicUnitBehavior(unitState, _graph);
            
        }

        public BasicUnitBehavior GetBasicMeele()
        {
            var position = _graph.GetRandomEmptyCellCoordinate();
            if (position == null)
            {
                throw new InvalidOperationException("Can't add more units to graph; Graph is full");
            }
            var unitState = new BasicUnitState(MovementTypes.Ground, 2)
            {
                Attack = new Attack {Range = 1}, Position = position.Value
            };
            return new BasicUnitBehavior(unitState, _graph);
        }
    }
}