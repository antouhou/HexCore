using System;
using System.Linq;
using HexCore.BattleCore;
using HexCore.HexGraph;
using NUnit.Framework;
using Tests.Fixtures;

namespace Tests.BattleCore
{
    [TestFixture]
    public class MapManagerTest
    {
        private static readonly Random Random = new Random();
        
        [Test]
        public void ShouldAddUnitsToTheMapIfThereIsAPlaceForThem()
        {
            var graph = new Graph(2, 2, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            var manager = new MapManager(graph);
            var unit = new UnitState(MovementTypes.Ground, 2);
            manager.AddUnit(unit);
            Assert.That(manager.Units.Count, Is.EqualTo(1));
            Assert.Contains(unit.Coordinate3D, graph.GetAllCoordinate3Ds());
        }
        
        [Test]
        public void ShouldNotAddUnitsToTheMapIfThereIsNoPlaceForThem()
        {
            // 4 units max
            var graph = new Graph(2, 2, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            var manager = new MapManager(graph);
            for (var i = 0; i < 4; i++)
            {
                Assert.That(manager.AddUnit(new UnitState(MovementTypes.Ground, 2)), Is.True);
            }
            Assert.That(manager.Units.Count, Is.EqualTo(4));
            Assert.That(manager.AddUnit(new UnitState(MovementTypes.Ground, 2)), Is.False);
            Assert.That(manager.Units.Count, Is.EqualTo(4));
        }
        
        [Test]
        public void ShouldBeAbleToMoveUnitsAroundTheMap()
        {
            var graph = new Graph(10, 10, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            var manager = new MapManager(graph);
            var unit = new UnitState(MovementTypes.Ground, 2);
            manager.AddUnit(unit);
            var area = manager.GetMovableArea(unit);
            var randomPointInUnitsMovementArea = area[Random.Next(area.Count)];
            
            Assert.That(unit.Coordinate3D, Is.Not.EqualTo(randomPointInUnitsMovementArea));
            Assert.That(manager.IsCellBlocked(randomPointInUnitsMovementArea), Is.False);
            
            Assert.That(manager.MoveUnitTo(unit, randomPointInUnitsMovementArea), Is.True);
            Assert.That(unit.Coordinate3D, Is.EqualTo(randomPointInUnitsMovementArea));
            Assert.That(manager.IsCellBlocked(randomPointInUnitsMovementArea), Is.True);
        }

        [Test]
        public void ShouldNotMoveUnitToOtherUnitPosition()
        {
            // With 3x3 graph and movement radius is equal to 3, any unit can move to any position
            var graph = new Graph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            var manager = new MapManager(graph);
            var unit = new UnitState(MovementTypes.Ground, 3);
            manager.AddUnit(unit);
            var area = manager.GetMovableArea(unit);
            var randomPointInUnitsMovementArea = area[Random.Next(area.Count)];
            manager.MoveUnitTo(unit, randomPointInUnitsMovementArea);
            var unit2 = new UnitState(MovementTypes.Ground, 2);
            manager.AddUnit(unit);
            Assert.That(manager.MoveUnitTo(unit2, randomPointInUnitsMovementArea), Is.False);
        }

        [Test]
        public void ShouldNotMoveUnitOverItsMovementArea()
        {
            var graph = new Graph(10, 10, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            var manager = new MapManager(graph);
            var unit = new UnitState(MovementTypes.Ground, 3);
            manager.AddUnit(unit);
            var movementArea = manager.GetMovableArea(unit);
            var allEmptyCells = manager.Graph.GetAllEmptyCellsCoordinate3Ds();
            var outsideOfMovementArea = allEmptyCells.Except(movementArea).ToList();
            var randomPointOutsideOfUnitsMovementArea = outsideOfMovementArea[Random.Next(outsideOfMovementArea.Count)];
            Assert.That(manager.MoveUnitTo(unit, randomPointOutsideOfUnitsMovementArea), Is.False);
        }
    }
}