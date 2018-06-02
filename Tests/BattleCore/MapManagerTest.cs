using System;
using System.Linq;
using HexCore.BattleCore;
using HexCore.DataStructures;
using HexCore.Helpers;
using HexCore.HexGraph;
using NUnit.Framework;
using Tests.Fixtures;

namespace Tests.BattleCore
{
    [TestFixture]
    public class MapManagerTest
    {
        private static readonly Random Random = new Random();
        private readonly CoordinateConverter _coordinateConverter = new CoordinateConverter(OffsetTypes.OddRowsRight);

        [Test]
        public void ShouldAddUnitsToTheMapIfThereIsAPlaceForThem()
        {
            var graph = new Graph(2, 2, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            var manager = new MapManager(graph);
            var unit = new BasicUnit(MovementTypes.Ground, 2);
            manager.AddUnit(unit);
            Assert.That(manager.Units.Count, Is.EqualTo(1));
            Assert.Contains(unit.Position, graph.GetAllCoordinate3Ds());
        }

        [Test]
        public void ShouldBeAbleToMoveUnitsAroundTheMap()
        {
            var graph = new Graph(10, 10, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            var manager = new MapManager(graph);
            var unit = new BasicUnit(MovementTypes.Ground, 2);
            manager.AddUnit(unit);
            var movementRange = unit.GetMovementRange(graph);
            var randomPointInUnitsMovementRange = movementRange[Random.Next(movementRange.Count)];

            Assert.That(unit.Position, Is.Not.EqualTo(randomPointInUnitsMovementRange));
            Assert.That(graph.IsCellBlocked(randomPointInUnitsMovementRange), Is.False);

            Assert.That(manager.MoveUnitTo(unit, randomPointInUnitsMovementRange), Is.True);
            Assert.That(unit.Position, Is.EqualTo(randomPointInUnitsMovementRange));
            Assert.That(graph.IsCellBlocked(randomPointInUnitsMovementRange), Is.True);
        }

        [Test]
        public void ShouldCalculateWhetherOneUnitCanAttackOtherOrNot()
        {
            var graph = new Graph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            var manager = new MapManager(graph);

            var unit1 = new BasicUnit(MovementTypes.Ground, 3);
            var unit2 = new BasicUnit(MovementTypes.Ground, 3);

            // Unit 3 will be a guy with a range attack.
            // With 3x3 map and attack range is equal to 3 he always can attack anyone on the map, even from the corners
            var unit3 = new BasicUnit(MovementTypes.Ground, 3) {Attack = new Attack {Range = 3}};

            var kek = _coordinateConverter.ConvertManyCubeToOffset(unit3.GetAttackRange(graph));

            manager.AddUnit(unit1);
            manager.MoveUnitTo(unit1, _coordinateConverter.ConvertOneOffsetToCube(new Coordinate2D(0, 0)));
            manager.AddUnit(unit2);
            manager.MoveUnitTo(unit2, _coordinateConverter.ConvertOneOffsetToCube(new Coordinate2D(0, 1)));
            manager.AddUnit(unit3);
            manager.MoveUnitTo(unit3, _coordinateConverter.ConvertOneOffsetToCube(new Coordinate2D(2, 2)));

            var kek2 = unit1.GetAttackRange(graph);

            Assert.That(manager.CanAttack(unit1, unit2), Is.True);
            Assert.That(manager.CanAttack(unit2, unit1), Is.True);

            // Unit with the range attack can attack anyone on the small map
            Assert.That(manager.CanAttack(unit3, unit1), Is.True);
            Assert.That(manager.CanAttack(unit3, unit2), Is.True);

            // Now let's move unit 1 from unit 2
            manager.MoveUnitTo(unit1, _coordinateConverter.ConvertOneOffsetToCube(new Coordinate2D(2, 1)));

            Assert.That(manager.CanAttack(unit1, unit2), Is.False);
            Assert.That(manager.CanAttack(unit2, unit1), Is.False);

            // Range guy still can attack everyone on the map
            Assert.That(manager.CanAttack(unit3, unit1), Is.True);
            Assert.That(manager.CanAttack(unit3, unit2), Is.True);
        }

        [Test]
        public void ShouldNotAddUnitsToTheMapIfThereIsNoPlaceForThem()
        {
            // 4 units max
            var graph = new Graph(2, 2, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            var manager = new MapManager(graph);
            for (var i = 0; i < 4; i++) Assert.That(manager.AddUnit(new BasicUnit(MovementTypes.Ground, 2)), Is.True);
            Assert.That(manager.Units.Count, Is.EqualTo(4));
            Assert.That(manager.AddUnit(new BasicUnit(MovementTypes.Ground, 2)), Is.False);
            Assert.That(manager.Units.Count, Is.EqualTo(4));
        }

        [Test]
        public void ShouldNotMoveUnitOverItsMovementRange()
        {
            var graph = new Graph(10, 10, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            var manager = new MapManager(graph);
            var unit = new BasicUnit(MovementTypes.Ground, 3);
            manager.AddUnit(unit);
            var movementRange = unit.GetMovementRange(graph);
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
            var unit = new BasicUnit(MovementTypes.Ground, 3);
            manager.AddUnit(unit);
            var movementRange = unit.GetMovementRange(graph);
            var randomPointInMovementRange = movementRange[Random.Next(movementRange.Count)];
            manager.MoveUnitTo(unit, randomPointInMovementRange);
            var unit2 = new BasicUnit(MovementTypes.Ground, 2);
            manager.AddUnit(unit);
            Assert.That(manager.MoveUnitTo(unit2, randomPointInMovementRange), Is.False);
        }
    }
}