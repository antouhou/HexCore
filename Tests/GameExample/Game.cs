//using HexCore.DataStructures;
//using HexCore.HexGraph;
//using NUnit.Framework;
//using Tests.Fixtures;
//using Tests.GameExample.Abilities;
//using Tests.GameExample.Effects;
//using Tests.GameExample.Pawns;
//
//namespace Tests.GameExample
//{
//    [TestFixture]
//    public class Game
//    {
//        /**
//         * 1. Create a map
//         * 2. Instanciate units:
//         *     Unit position is managed by battlefield manager, not by pawn;
//         *     Add units to the battlefield like this:
//         *         battleManager.spawn(uint, position);
//         *     Movement should be managed by a manager as well:
//         *         battleManager.move(unit, position);
//         *     Using ability:
//         *         battleManager.useAbility(unit, ability, target)
//         *         Ability should have:
//         *             - Range for casting
//         *             - Area of effect application:
//         *                 - Should be not just a specific number, but a shape of 3D Coords instead, with Coordinate3D(0,0,0)
//         *                 being the center, and all other cells (may consider excluding 0,0,0 from the list, but if included,
//         *                 it allows more ineteresting shapes, such as a circle around the selected target, but excluding
//         *                 the exact target coordinate, i.e, if the range of casting is 0, and shape is a circle, caster
//         *                 can cast it only in itself, but the effect will be applied only to pawns surronuding caster,
//         *                 exluding himself). It is possible to rotate shape through Coordinate3D.RotateRight()
//         *             - Duration (0 for one-time effect, more for lasting effect)
//         *                 - Applies right after the use
//         *             - Possible targets (enemy, ally, empty, everyone)
//         *             - Should have an Effect that applied to the target (or targets in range)
//         *                     Effect can have:
//         *                         - Stat change (can be change of any stat unit have - defence, attack, movementType, hp)
//         *                         - Method for calculating caster and target bonuses and penalties based on their attributes
//         *     Units should be able to perform a physical attack:
//         *         battleManager.attack(attacker, target)
//         *
//         *                     
//         *                     
//         * Pawn has an information about its abilities, movement type, movement range, resistances
//         * Pawn should have a state which lists current effects, current hp and so on.
//         *
//         * Turn has ended if user choose to end it, or if no possible moves for the team left.
//         * At the end of the turn duration of all effects reduced by 1.
//         *
//         * It may be worthwile to have separate attributes class. Pawns have attributes and Effects also have attributes,
//         * So effect attributes can be easily applied to the pawn. Pawn state also need to have a field to stored
//         * effects applied to it.
//         *     
//         */
//        [Test]
//        public void ShouldBeAbleToPlayAGame()
//        {
//            // Create map
//            var map = GraphFactory.CreateRectangularGraph(height: 10, width: 10);
//
//            // Create manager
//            var battleManager = new BattleManager(map);
//
//            // Create abilities and effects
//            var heal = new AreaAbility("Heal", new Heal());
//            var defenseUp = new AreaAbility("Defence Up", new DefenceUp());
//            var fireBall = new AreaAbility("Fire Ball", new Fire());
//            var fireBlast = new DirectionalAbility("Fire blast", new Fire());
//
//            // Create unit types
//            var meleeType = new PawnType("Melee", new Attributes(MovementTypes.Ground, 3, 2, 10, 0, 0, 3, 1),
//                new IAbility[] { });
//            var rangeType = new PawnType("Ranger", new Attributes(MovementTypes.Ground, 3, 2, 8, 0, 0, 3, 2),
//                new IAbility[] { });
//            var mageType = new PawnType("Mage", new Attributes(MovementTypes.Ground, 3, 1, 8, 10, 1, 3, 1),
//                new[] {fireBall});
//            var supportType = new PawnType("Support", new Attributes(MovementTypes.Ground, 3, 1, 8, 10, 1, 3, 1),
//                new[] {defenseUp, heal});
//
//            // Init units
//            var melee = meleeType.CreatePawn();
//            var range = rangeType.CreatePawn();
//            var mage = mageType.CreatePawn();
//            var support = supportType.CreatePawn();
//            // todo: It also should be possible to load pawn from a file
//
//            // Should spawn with coordinate 2d
//            battleManager.Spawn(melee, new Coordinate2D(1, 2, OffsetTypes.OddRowsRight));
//            // Should spawn with coordinate 3
//            battleManager.Spawn(range, new Coordinate2D(0, 0, OffsetTypes.OddRowsRight).To3D());
//            // Should spawn a list of pawns
//            battleManager.Spawn(new[]
//            {
//                (mage, new Coordinate2D(0, 1, OffsetTypes.OddRowsRight).To3D()),
//                (support, new Coordinate2D(1, 0, OffsetTypes.OddRowsRight).To3D())
//            });
//
//            // Should move to coordinate 2D
//            battleManager.Move(range, new Coordinate2D(1, 1, OffsetTypes.OddRowsRight));
//            // Should move to coordinate 3D
//            battleManager.Move(melee, new Coordinate2D(2, 1, OffsetTypes.OddRowsRight).To3D());
//
//            var meleePosition = battleManager.GetPosition(melee);
//            // Should do damage
//            battleManager.UseAbility(mage, fireBall, meleePosition);
//            // Also shouldn't cast if the target outside of casting range, or isn't valid target
//
//            var rangerPosition = battleManager.GetPosition(range);
//            // Should last more than one turn
//            battleManager.UseAbility(support, defenseUp, rangerPosition);
//        }
//    }
//}

