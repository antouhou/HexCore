using System;
using System.Collections.Generic;
using HexCore.DataStructures;
using HexCore.HexGraph;
using NUnit.Framework;
using Tests.Fixtures;
using Tests.GameExample.Abilities;
using Tests.GameExample.Effects;
using Tests.GameExample.Pawns;

namespace Tests.GameExample
{
    [TestFixture]
    public class GameTest
    {
        /**
         * 1. Create a map
         * 2. Instanciate units:
         *     Unit position is managed by battlefield manager, not by pawn;
         *     Add units to the battlefield like this:
         *         battleManager.spawn(uint, position);
         *     Movement should be managed by a manager as well:
         *         battleManager.move(unit, position);
         *     Using ability:
         *         battleManager.useAbility(unit, ability, target)
         *         Ability should have:
         *             - Range for casting
         *             - Area of effect application:
         *                 - Should be not just a specific number, but a shape of 3D Coords instead, with Coordinate3D(0,0,0)
         *                 being the center, and all other cells (may consider excluding 0,0,0 from the list, but if included,
         *                 it allows more ineteresting shapes, such as a circle around the selected target, but excluding
         *                 the exact target coordinate, i.e, if the range of casting is 0, and shape is a circle, caster
         *                 can cast it only in itself, but the effect will be applied only to pawns surronuding caster,
         *                 exluding himself). It is possible to rotate shape through Coordinate3D.RotateRight()
         *             - EffectDuration (0 for one-time effect, more for lasting effect)
         *                 - Applies right after the use
         *             - Possible targets (enemy, ally, empty, everyone)
         *             - Should have an Effect that applied to the target (or targets in range)
         *                     Effect can have:
         *                         - Stat change (can be change of any stat unit have - defence, attack, movementType, hp)
         *                         - Method for calculating caster and target bonuses and penalties based on their attributes
         *     Units should be able to perform a physical attack:
         *         battleManager.attack(attacker, target)
         *
         *                     
         *                     
         * Pawn has an information about its abilities, movement type, movement range, resistances
         * Pawn should have a state which lists current effects, current hp and so on.
         *
         * Turn has ended if user choose to end it, or if no possible moves for the team left.
         * At the end of the turn duration of all effects reduced by 1.
         *
         * It may be worthwile to have separate attributes class. Pawns have attributes and Effects also have attributes,
         * So effect attributes can be easily applied to the pawn. Pawn state also need to have a field to stored
         * effects applied to it.
         *     
         */
        private const string HealEffectName = "heal";
        private const string FireEffectName = "fire";
        private const string DefenseUpEffectName = "defenceUp";

        private const string HealAbilityName = "Heal";
        private const string FireBlastAbilityName = "Fire Blast";
        private const string FireBlastAreaAbilityName = "Fire Area Blast";
        private const string DefenceUpAbilityName = "Defence Up";

        private const string WizardPawnName = "Wizard";
        private const string ClericPawnName = "Cleric";
        private const string WarriorPawnName = "Warrior";
        private const string ArcherPawnName = "Archer";

        /// <summary>
        ///     This function creates effects and bonuses for effects. Returns a dictionary with all effects.
        ///     Bonuses are lambdas passed to the Effect constructor. Bonus function should accept effect attributes as a
        ///     first parameter, caster/target attributes as a second parameters, and returns an instance of Attributes
        ///     that will be applied to effect attributes. There should be two bonuses: the first one is the caster bonus,
        ///     and, for example, can amplify spell damage based on caster magic power. The second is the defense bonus,
        ///     and can reduce negative/amplify positive effects based on the target attributes.
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, Effect> BootstrapEffects()
        {
            Attributes NoBonus(Attributes effectAttributes, Attributes targetAttributes)
            {
                return new Attributes();
            }

            Attributes MagicalCasterBonus(Attributes effectAttributes, Attributes casterAttributes)
            {
                return new Attributes(
                    hp: effectAttributes.HP * (casterAttributes.MagicPower * 0.2),
                    defense: effectAttributes.HP * (casterAttributes.MagicPower * 0.2)
                );
            }

            Attributes MagicalTargetBonus(Attributes effectAttributes, Attributes casterAttributes)
            {
                return new Attributes(
                    hp: effectAttributes.HP * (casterAttributes.MagicPower * 0.2),
                    defense: effectAttributes.HP * (casterAttributes.MagicPower * 0.2)
                );
            }

            var healEffect = new Effect(
                new Attributes(hp: 2),
                MagicalCasterBonus,
                NoBonus
            );
            var fireEffect = new Effect(
                new Attributes(hp: -2),
                MagicalCasterBonus,
                MagicalTargetBonus
            );
            var defenceUpEffect = new Effect(
                new Attributes(defense: 1),
                MagicalCasterBonus,
                NoBonus
            );
            return new Dictionary<string, Effect>
            {
                [HealEffectName] = healEffect,
                [FireEffectName] = fireEffect,
                [DefenseUpEffectName] = defenceUpEffect
            };
        }

        /// <summary>
        ///     This method bootstraps abilities based on effects.
        /// </summary>
        /// <param name="effects"></param>
        /// <returns></returns>
        private static Dictionary<string, Ability> BootstrapAbilities(Dictionary<string, Effect> effects)
        {
            var heal = new Ability(HealAbilityName, effects[HealEffectName], new[] {new Coordinate3D()}, 3, 1);
            var defenseUp = new Ability(DefenceUpAbilityName, effects[DefenseUpEffectName], new[] {new Coordinate3D()},
                3, 1);
            var fireBlast = new Ability(FireBlastAbilityName, effects[FireEffectName], new[] {new Coordinate3D()}, 3,
                1);
            var fireBlastArea = new Ability(FireBlastAreaAbilityName, effects[FireEffectName],
                new[] {new Coordinate3D()}, 3, 1);

            return new Dictionary<string, Ability>
            {
                [HealAbilityName] = heal,
                [DefenceUpAbilityName] = defenseUp,
                [FireBlastAbilityName] = fireBlast,
                [FireBlastAreaAbilityName] = fireBlastArea
            };
        }

        /// <summary>
        ///     This method bootstraps pawn factories that can be used to created new pawn based on the pawn template
        /// </summary>
        /// <param name="abilities"></param>
        /// <returns></returns>
        private static Dictionary<string, PawnType> BootstrapPawnTypes(Dictionary<string, Ability> abilities)
        {
            var warriorPawnType = new PawnType(WarriorPawnName,
                new Attributes(MovementTypes.Ground, 3, 2, 10, 0, 0, 3, 1),
                new Ability[] { });
            var archerPawnType = new PawnType(ArcherPawnName, new Attributes(MovementTypes.Ground, 3, 2, 8, 0, 0, 3, 2),
                new Ability[] { });
            var wizardPawnType = new PawnType(WizardPawnName,
                new Attributes(MovementTypes.Ground, 3, 1, 8, 10, 1, 3, 1),
                new[] {abilities[FireBlastAbilityName], abilities[FireBlastAreaAbilityName]});
            var clericPawnType = new PawnType(ClericPawnName,
                new Attributes(MovementTypes.Ground, 3, 1, 8, 10, 1, 3, 1),
                new[] {abilities[DefenceUpAbilityName], abilities[HealAbilityName]});

            return new Dictionary<string, PawnType>
            {
                [WarriorPawnName] = warriorPawnType,
                [ArcherPawnName] = archerPawnType,
                [WizardPawnName] = wizardPawnType,
                [ClericPawnName] = clericPawnType
            };
        }

        private Game BootstrapGame(int mapWidth, int mapHeight)
        {
            var effects = BootstrapEffects();
            var abilities = BootstrapAbilities(effects);
            var pawnTypes = BootstrapPawnTypes(abilities);
            var battleManager = new BattleManager(GraphFactory.CreateRectangularGraph(mapWidth, mapHeight));

            return new Game(effects, abilities, pawnTypes, battleManager);
        }

        [Test]
        public void AddTeam_ShouldAddTeams()
        {
            var game = BootstrapGame(2, 2);
            var battleManager = game.BattleManager;

            const string teamRedId = "team";
            var team = new[] {game.PawnTypes[WarriorPawnName].CreatePawn()};

            battleManager.AddTeam(teamRedId, team);

            Assert.That(battleManager.GetTeams().ContainsKey(teamRedId));

            const string teamBlueId = "teamBlue";
            var teamBlue = new[] {game.PawnTypes[WarriorPawnName].CreatePawn()};

            battleManager.AddTeam(teamBlueId, teamBlue);

            Assert.That(battleManager.GetTeams().ContainsKey(teamBlueId));
        }

        [Test]
        public void AddTeam_ShouldThrowWhenAddingTeamWithTheSameId()
        {
            var game = BootstrapGame(2, 2);
            var battleManager = game.BattleManager;

            const string redTeamId = "team";

            var teamRed = new[] {game.PawnTypes[WarriorPawnName].CreatePawn()};
            var teamBlue = new[] {game.PawnTypes[WarriorPawnName].CreatePawn()};

            battleManager.AddTeam(redTeamId, teamRed);
            Assert.True(battleManager.GetTeams().ContainsKey(redTeamId));

            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                battleManager.AddTeam(redTeamId, teamBlue);
            });
            Assert.That(exception.Message, Is.EqualTo("Team with such id was already added"));
        }

        [Test]
        public void AddTeam_ShouldThrowWhenAddingTeamWithTheSamePawnTwice()
        {
            var game = BootstrapGame(2, 2);
            var battleManager = game.BattleManager;

            const string redTeamId = "team";
            const string blueTeamId = "teamBlue";
            var pawn = game.PawnTypes[WarriorPawnName].CreatePawn();
            var teamRed = new[] {pawn};
            var teamBlue = new[] {pawn};

            battleManager.AddTeam(redTeamId, teamRed);
            Assert.True(battleManager.GetTeams().ContainsKey(redTeamId));

            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                battleManager.AddTeam(blueTeamId, teamBlue);
            });
            Assert.That(exception.Message, Is.EqualTo("Can't add team whose pawn is already in another team"));
            Assert.False(battleManager.GetTeams().ContainsKey(blueTeamId));
        }

        [Test]
        public void Attack()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void Move()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void Move_ShouldMoveSpawnedPawn()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void Move_ShouldNotMoveNotSpawnedPawn()
        {
            // TODO: Do we even need to have not spawned pawns? I.e. always spawn
            // all pawns with the team? We probably do
            throw new NotImplementedException();
        }

        [Test]
        public void Move_ShouldNotMovePawnIfItIsAnotherTeamTurn()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void Move_ShouldNotMovePawnIfPawnWasAlreadyMovedInThatTurn()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void Move_ShouldNotMovePawnToAlreadyTakenPosition()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void Move_ShouldNotMovePawnToBlockedPosition()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void Move_ShouldNotMovePawnToPositionOutsideOfTheMapBorder()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void Spawn_ShouldNotSpawnAlreadySpawnedPawn()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void Spawn_ShouldNotSpawnAtBlockedCoordinate()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void Spawn_ShouldNotSpawnAtTheSameCoordinateTwice()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void Spawn_ShouldNotSpawnWhenDesiredPositionIsAlreadyBlocked()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void Spawn_ShouldSpawnAListOfPawns()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void Spawn_ShouldSpawnPawnAtTheGivenCoordinate()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void Spawn_ShouldThrowWhenPawnsCountIsNotEqualToPositionsCount()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void SpawnTeam_ShouldAddTeamAndSpawnAllItsPawns()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void UseAbility()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void Z_ShouldBeAbleToPlayAGame()
        {
            // Bootstrap effects, abilities, and pawn types
            var effects = BootstrapEffects();
            var abilities = BootstrapAbilities(effects);
            var pawnTypes = BootstrapPawnTypes(abilities);
            // Create map
            var map = GraphFactory.CreateRectangularGraph(height: 10, width: 10);

            // Init units
            var warrior = pawnTypes[WarriorPawnName].CreatePawn();
            var archer = pawnTypes[ArcherPawnName].CreatePawn();
            var wizard = pawnTypes[WizardPawnName].CreatePawn();
            var cleric = pawnTypes[ClericPawnName].CreatePawn();
            // todo: It also should be possible to load pawn from a file

            /**
             * - It should be possible to create teams
             * - It should be not possible to move the same pawn twice during one turn
             * - It should be available to perform only one action per pawn per turn:
             * One attack or ability.
             * - Turn should end automatically when no more actions is available for the team
             * (i.e there is no more units that can cast, attack or move)
             * - When the turn is ended for one team, turn of the next team should start.
             *
             * var teamOne = {warrior, cleric};
             * var teamTwo = {archer, wizard};
             * 
             * battleManager.AddTeam("team1", teamOne);
             * battleManager.AddTeam("team2", teamTwo);
             * // It shouldn't be possible to add two teams with the same
             * // It also shouldn't be possible to add same pawn twice to the same or to a different team
             *
             * battleManager.Move(warrior, new Coordinate3D());
             * // battleManager.Move(warrior, new Coordinate3D()); -> should throw, as already moved
             * // battleManager.Move(archer, new Coordinate3D()); -> should throw, it's another team turn
             * var targets = battleManager.GetPossibleAttackTargets(warrior); // { ["archer"]: archer }
             * battleManager.Attack(warrior, targets["archer"]);
             * // battleManager.Attack(warrior, archer); -> should throw, no more than one attack per turn
             * // battleManager.CastAbility(warrior, ability, archer); -> should throw, warrior have no such ability
             * targets = battleManager.GetPossibleAbilityTargets(cleric, defenseUp); // { ["warrior"]: warrior }
             * battleManager.CastAbility(cleric, defenseUp, targets["warrior"]);
             * // battleManager.CastAbility(cleric, defenseUp, targets["warrior"]); -> should throw, no more than one
             * // cast per unit per turn
             * // same rules as for attack, i.e. no more than one ability per turn, etc.
             */

            // Create manager
            var battleManager = new BattleManager(map);

            battleManager.SpawnTeam("red", new[] {warrior, cleric}, new[]
                {
                    new Coordinate2D(0, 1, OffsetTypes.OddRowsRight).To3D(),
                    new Coordinate2D(1, 0, OffsetTypes.OddRowsRight).To3D()
                }
            );
            battleManager.SpawnTeam("blue", new[] {wizard, archer}, new[]
            {
                new Coordinate2D(0, 0, OffsetTypes.OddRowsRight).To3D(),
                new Coordinate2D(2, 1, OffsetTypes.OddRowsRight).To3D()
            });

            // Should move to coordinate 2D
            battleManager.Move(archer, new Coordinate2D(1, 1, OffsetTypes.OddRowsRight));
            // Should move to coordinate 3D
            battleManager.Move(warrior, new Coordinate2D(2, 1, OffsetTypes.OddRowsRight).To3D());

            var meleePosition = battleManager.GetPosition(warrior);
            // Should do damage
            battleManager.UseAbility(wizard, abilities[FireBlastAbilityName], meleePosition);
            // Also shouldn't cast if the target outside of casting range, or isn't valid target

            var rangerPosition = battleManager.GetPosition(archer);
            // Should last more than one turn
            battleManager.UseAbility(cleric, abilities[DefenceUpAbilityName], rangerPosition);
        }
    }
}