using System;
using System.Net.Mime;
using HexCore.BattleCore;
using HexCore.BattleCore.Unit;
using HexCore.DataStructures;
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

        public BasicUnitBehavior GetBasicUnit(MovementType movementType = null, int movementRange = 3, int attackRange = 1)
        {
            if (movementType == null) movementType = BasicMovementTypes.Ground;
            var position = _graph.GetRandomEmptyCellCoordinate();
            if (position == null)
            {
                throw new InvalidOperationException("Can't add more units to graph; Graph is full");
            }
            var unitState = new BasicUnitState(movementType, movementRange)
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