using HexCore.BattleCore;
using HexCore.DataStructures;
using HexCore.Helpers;
using HexCore.HexGraph;
using NUnit.Framework;
using Tests.Fixtures;

namespace Tests.BattleCore.Unit
{
    [TestFixture]
    public class UnitControllerTest
    {
        private readonly CoordinateConverter _coordinateConverter = new CoordinateConverter(OffsetTypes.OddRowsRight);

        [Test]
        public void ShouldCalculateWhetherOneUnitCanAttackOtherOrNot()
        {
            var graph = new Graph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            var manager = new MapManager(graph);
            var unitFactory = new UnitFactory(graph);

            var unit1 = unitFactory.GetUnit(MovementTypes.Ground, 3, 1);
            var unit2 = unitFactory.GetUnit(MovementTypes.Ground, 3, 1);

            // Unit 3 will be a guy with a range attack.
            // With 3x3 map and attack range is equal to 3 he always can attack anyone on the map, even from the corners
            var unit3 = unitFactory.GetUnit(MovementTypes.Ground, 3, 3);

            manager.AddUnit(unit1);
            manager.MoveUnitTo(unit1, _coordinateConverter.ConvertOneOffsetToCube(new Coordinate2D(0, 0)));
            manager.AddUnit(unit2);
            manager.MoveUnitTo(unit2, _coordinateConverter.ConvertOneOffsetToCube(new Coordinate2D(0, 1)));
            manager.AddUnit(unit3);
            manager.MoveUnitTo(unit3, _coordinateConverter.ConvertOneOffsetToCube(new Coordinate2D(2, 2)));

            Assert.That(unit1.CanAttack(unit2), Is.True);
            Assert.That(unit2.CanAttack(unit1), Is.True);

            // Unit with the range attack can attack anyone on the small map
            Assert.That(unit3.CanAttack(unit1), Is.True);
            Assert.That(unit3.CanAttack(unit2), Is.True);

            // Now let's move unit 1 from unit 2
            manager.MoveUnitTo(unit1, _coordinateConverter.ConvertOneOffsetToCube(new Coordinate2D(2, 1)));

            Assert.That(unit1.CanAttack(unit2), Is.False);
            Assert.That(unit2.CanAttack(unit1), Is.False);

            // Range guy still can attack everyone on the map
            Assert.That(unit3.CanAttack(unit1), Is.True);
            Assert.That(unit3.CanAttack(unit2), Is.True);
        }
        
        [Test]
        public void ShouldNotBeAbleToAttackItself()
        {
            var graph = new Graph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            var manager = new MapManager(graph);
            var unitFactory = new UnitFactory(graph);

            var unit1 = unitFactory.GetUnit(MovementTypes.Ground, 3, 1);
            manager.AddUnit(unit1);
            Assert.That(unit1.CanAttack(unit1), Is.False);
        }
    }
}