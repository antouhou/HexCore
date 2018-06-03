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
            var unitFactory = new UnitFactory(graph);
            var unit = unitFactory.GetUnit(MovementTypes.Ground, 2, 1);
            manager.AddUnit(unit);
            Assert.That(manager.Units.Count, Is.EqualTo(1));
            Assert.Contains(unit.State.Position, graph.GetAllCoordinate3Ds());
        }

        [Test]
        public void ShouldBeAbleToMoveUnitsAroundTheMap()
        {
            var graph = new Graph(10, 10, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            var manager = new MapManager(graph);
            var unitFactory = new UnitFactory(graph);
            var unit = unitFactory.GetUnit(MovementTypes.Ground, 2, 1);
            manager.AddUnit(unit);
            var movementRange = unit.GetMovementRange();
            var randomPointInUnitsMovementRange = movementRange[Random.Next(movementRange.Count)];

            Assert.That(unit.State.Position, Is.Not.EqualTo(randomPointInUnitsMovementRange));
            Assert.That(graph.IsCellBlocked(randomPointInUnitsMovementRange), Is.False);

            Assert.That(manager.MoveUnitTo(unit, randomPointInUnitsMovementRange), Is.True);
            Assert.That(unit.State.Position, Is.EqualTo(randomPointInUnitsMovementRange));
            Assert.That(graph.IsCellBlocked(randomPointInUnitsMovementRange), Is.True);
        }

        [Test]
        public void ShouldNotAddUnitsToTheMapIfThereIsNoPlaceForThem()
        {
            // 4 units max
            var graph = new Graph(2, 2, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            var manager = new MapManager(graph);
            var unitFactory = new UnitFactory(graph);
            for (var i = 0; i < 4; i++) Assert.That(manager.AddUnit(unitFactory.GetBasicMeele()), Is.True);
            Assert.That(manager.Units.Count, Is.EqualTo(4));
            Assert.That(manager.AddUnit(unitFactory.GetBasicMeele()), Is.False);
            Assert.That(manager.Units.Count, Is.EqualTo(4));
        }

        [Test]
        public void ShouldNotMoveUnitOverItsMovementRange()
        {
            var graph = new Graph(10, 10, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            var manager = new MapManager(graph);
            var unitFactory = new UnitFactory(graph);
            var unit = unitFactory.GetBasicMeele();
            manager.AddUnit(unit);
            var movementRange = unit.GetMovementRange();
            var allEmptyCells = manager.Graph.GetAllEmptyCellsCoordinate3Ds();
            var notInMovementRange = allEmptyCells.Except(movementRange).ToList();
            var randomPointNotInMovementRange = notInMovementRange[Random.Next(notInMovementRange.Count)];
            Assert.That(manager.MoveUnitTo(unit, randomPointNotInMovementRange), Is.False);
        }

        [Test]
        public void ShouldNotMoveUnitToOtherUnitPosition()
        {
            // With 3x3 graph and movement radius is equal to 3, any unit can move to any position
            var graph = new Graph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            var manager = new MapManager(graph);
            var unitFactory = new UnitFactory(graph);
            var unit = unitFactory.GetBasicMeele();
            manager.AddUnit(unit);
            var movementRange = unit.GetMovementRange();
            var randomPointInMovementRange = movementRange[Random.Next(movementRange.Count)];
            manager.MoveUnitTo(unit, randomPointInMovementRange);
            var unit2 = unitFactory.GetBasicMeele();
            manager.AddUnit(unit);
            Assert.That(manager.MoveUnitTo(unit2, randomPointInMovementRange), Is.False);
        }
    }
}