using HexCore.DataStructures;
using HexCore.Helpers;
using HexCore.HexGraph;
using NUnit.Framework;
using Tests.Fixtures;
using System;
using HexCore.BattleCore;
using HexCore.BattleCore.Unit;
using System.Linq;

namespace Tests.BattleCore.Unit
{
    [TestFixture]
    public class UnitBehaviorTest
    {
        private static readonly Random Random = new Random();
        private readonly CoordinateConverter _coordinateConverter = new CoordinateConverter(OffsetTypes.OddRowsRight);

        [Test]
        public void ShouldCalculateWhetherOneUnitCanAttackOtherOrNot()
        {
            var graph = GraphFactory.CreateSquareGraph(3, 3);
            var unitFactory = new BasicUnitFactory(graph);

            var unit1 = unitFactory.GetBasicUnit();
            var unit2 = unitFactory.GetBasicUnit();

            // Unit 3 will be a guy with a range attack.
            // With 3x3 map and attack range is equal to 3 he always can attack anyone on the map, even from the corners
            var unit3 = unitFactory.GetBasicUnit(attackRange: 3);

            unit1.MoveTo(_coordinateConverter.ConvertOneOffsetToCube(new Coordinate2D(0, 0)));
            unit2.MoveTo(_coordinateConverter.ConvertOneOffsetToCube(new Coordinate2D(0, 1)));
            unit3.MoveTo(_coordinateConverter.ConvertOneOffsetToCube(new Coordinate2D(2, 2)));

            Assert.That(unit1.CanAttack(unit2), Is.True);
            Assert.That(unit2.CanAttack(unit1), Is.True);

            // Unit with the range attack can attack anyone on the small map
            Assert.That(unit3.CanAttack(unit1), Is.True);
            Assert.That(unit3.CanAttack(unit2), Is.True);

            // Now let's move unit 1 from unit 2
            unit1.MoveTo(_coordinateConverter.ConvertOneOffsetToCube(new Coordinate2D(2, 1)));

            Assert.That(unit1.CanAttack(unit2), Is.False);
            Assert.That(unit2.CanAttack(unit1), Is.False);

            // Range guy still can attack everyone on the map
            Assert.That(unit3.CanAttack(unit1), Is.True);
            Assert.That(unit3.CanAttack(unit2), Is.True);
        }

        [Test]
        public void ShouldNotBeAbleToAttackItself()
        {
            var graph = GraphFactory.CreateSquareGraph(3, 3);
            var unitFactory = new BasicUnitFactory(graph);

            var unit1 = unitFactory.GetBasicUnit();
            Assert.That(unit1.CanAttack(unit1), Is.False);
        }
        
        [Test]
        public void ShouldAddUnitsToTheMapIfThereIsAPlaceForThem()
        {
            var graph = GraphFactory.CreateSquareGraph(2, 2);
            var unitFactory = new BasicUnitFactory(graph);
            var unit = unitFactory.GetBasicUnit(movementRange: 2);
            Assert.Contains(unit.State.Position, graph.GetAllCellsCoordinates());
        }

        [Test]
        public void ShouldBeAbleToMoveAroundTheMap()
        {
            var graph = GraphFactory.CreateSquareGraph(10, 10);
            var unitFactory = new BasicUnitFactory(graph);
            var unit = unitFactory.GetBasicUnit(movementRange: 2);
            var movementRange = unit.GetMovementRange();
            var randomPointInUnitsMovementRange = movementRange[Random.Next(movementRange.Count)];

            Assert.That(unit.State.Position, Is.Not.EqualTo(randomPointInUnitsMovementRange));
            Assert.That(graph.IsCellBlocked(randomPointInUnitsMovementRange), Is.False);

            Assert.That(unit.MoveTo(randomPointInUnitsMovementRange), Is.True);
            Assert.That(unit.State.Position, Is.EqualTo(randomPointInUnitsMovementRange));
            Assert.That(graph.IsCellBlocked(randomPointInUnitsMovementRange), Is.True);
        }

        [Test]
        public void ShouldThrowExecptionIfPositionAssignedToUnitAlreadyTaken()
        {
            // 4 units max
            var graph = GraphFactory.CreateSquareGraph(2, 2);
            var unitFactory = new BasicUnitFactory(graph);
            for (var i = 0; i < 4; i++) unitFactory.GetBasicMeele();
            var unitState = new BasicUnitState(BasicMovementTypes.Ground, 2) { Position = graph.GetRandomCellCoordinate() };
            Assert.Throws<InvalidOperationException>(() => new BasicUnitBehavior(unitState, graph));
        }

        [Test]
        public void ShouldNotMoveOverItsMovementRange()
        {
            var graph = GraphFactory.CreateSquareGraph(10, 10);
            var unitFactory = new BasicUnitFactory(graph);
            var unit = unitFactory.GetBasicMeele();
            var movementRange = unit.GetMovementRange();
            var allEmptyCells = graph.GetAllEmptyCellsCoordinates();
            var notInMovementRange = allEmptyCells.Except(movementRange).ToList();
            var randomPointNotInMovementRange = notInMovementRange[Random.Next(notInMovementRange.Count)];
            Assert.That(unit.MoveTo(randomPointNotInMovementRange), Is.False);
        }

        [Test]
        public void ShouldNotMoveToOtherUnitPosition()
        {
            // With 3x3 graph and movement radius is equal to 3, any unit can move to any position
            var graph = GraphFactory.CreateSquareGraph(3, 3);
            var unitFactory = new BasicUnitFactory(graph);
            var unit = unitFactory.GetBasicMeele();
            var movementRange = unit.GetMovementRange();
            var randomPointInMovementRange = movementRange[Random.Next(movementRange.Count)];
            unit.MoveTo(randomPointInMovementRange);
            var unit2 = unitFactory.GetBasicMeele();
            Assert.That(unit2.MoveTo(randomPointInMovementRange), Is.False);
        }

        [Test]
        public void ShouldBeCompatibleWithOtherImplementationsOfIUnitBehavior()
        {
            var graph = GraphFactory.CreateSquareGraph(3, 3);
            
            var unit1 = new BasicUnitBehavior(new BasicUnitState(BasicMovementTypes.Ground, 1)
            {
                Attack = new Attack {Range = 1}, Position = _coordinateConverter.ConvertOneOffsetToCube(new Coordinate2D(0, 1))
            }, graph);
            var unit2 = new CustomUnitBehavior(new CustomUnitState(BasicMovementTypes.Ground, 1)
            {
                Attack = new Attack {Range = 1}, Position = _coordinateConverter.ConvertOneOffsetToCube(new Coordinate2D(1, 1))
            }, graph);

            Assert.True(unit2.CanAttack(unit1));
            Assert.True(unit1.CanAttack(unit2));
        }
        
        [Test]
        public void ShouldBeAbleToPerformAttack()
        {
            var graph = GraphFactory.CreateSquareGraph(3, 3);
            
            var unit1 = new BasicUnitBehavior(new BasicUnitState(BasicMovementTypes.Ground, 1)
            {
                Attack = new Attack {Range = 1}, Position = _coordinateConverter.ConvertOneOffsetToCube(new Coordinate2D(0, 1))
            }, graph);
            var unit2 = new BasicUnitBehavior(new BasicUnitState(BasicMovementTypes.Ground, 1)
            {
                Attack = new Attack {Range = 1}, Position = _coordinateConverter.ConvertOneOffsetToCube(new Coordinate2D(1, 1))
            }, graph);

            Assert.AreEqual(unit2.HealthPoints, 3.0);
            Assert.AreEqual(unit1.GetAttackPower(), 2.0);
            Assert.AreEqual(unit2.Defense.GetBlockedDamageAmount(unit1.Attack, unit1.GetAttackPower()), 1.0);
            var attackResult = unit1.PerformAttack(unit2);
            // HP - (attackPower - blokedDamage)
            Assert.AreEqual(unit2.HealthPoints, 2.0);
            Assert.AreEqual(attackResult.HPLeft, unit2.HealthPoints);
            Assert.AreEqual(attackResult.totalDamageAmount, unit1.GetAttackPower() - unit2.Defense.GetBlockedDamageAmount(unit1.Attack, unit1.GetAttackPower()));
        }
    }
}