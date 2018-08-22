using HexCore.BattleCore;
using HexCore.HexGraph;
using NUnit.Framework;
using System.Collections.Generic;
using HexCore.DataStructures;
using HexCore.Helpers;

namespace Tests.BattleCore
{   
    [TestFixture]
    public class BattlefieldManagerTest
    {
        [Test]
        public void ShouldBeAbleToRunBattleFromTheBeginningToTheEnd()
        {
            // Yeah, that's not a good name. I need to redesign that converter thing I think
            var ccorr = new CoordinateConverter(OffsetTypes.OddRowsRight);
            //This is an integration test for battlefild, aimed to help to design structure and interfaces.

            // 1. Let's initialize the map:
            var map = GraphFactory.CreateSquareGraph(height: 10, width: 10);

            // 2. Let's create some units. Let's consider one meele, one ranged and one mage on each side.
            // We also need to manage different teams.
            // or battlefiled.GetAllUnitsThatCanPerformActionsByTeamId(); (Looks shitty, definitly need a better name :D)
            // Maybe it also can be useful to have something like:
            
            // battlefield.GetAllTeamIds();
            // battlefield.GetTeamById();
            // battlefield.IsTeamTurn(teamId);
            
            // Some distant plans: 
            // team.AddUnit()
            // team.GetAllUnits();
            // team.GetAllUnitsIds();
            // ?? team.GetAllUnitsPositions();            
            var redMeele = new Pawn("red_meele", ccorr.ConvertOneOffsetToCube(new Coordinate2D(0, 1)), BasicMovementTypes.Ground, 3, 1, "red_team");
            var redRange = new Pawn("red_range", ccorr.ConvertOneOffsetToCube(new Coordinate2D(1, 0)), BasicMovementTypes.Ground, 3, 2, "red_team");
            var redMage = new Pawn("red_mage", ccorr.ConvertOneOffsetToCube(new Coordinate2D(0, 0)), BasicMovementTypes.Ground, 3, 1, "red_team");
            var redTeam = new Team(new List<Pawn>{ redMeele, redRange, redMage }, "red_team");
            
            
            var blueMeele = new Pawn("blue_meele", ccorr.ConvertOneOffsetToCube(new Coordinate2D(9, 8)), BasicMovementTypes.Ground, 3, 1, "blue_team");
            var blueRange = new Pawn("blue_range", ccorr.ConvertOneOffsetToCube(new Coordinate2D(8, 9)), BasicMovementTypes.Ground, 3, 2, "blue_team");
            var blueMage = new Pawn("blue_mage", ccorr.ConvertOneOffsetToCube(new Coordinate2D(9, 9)), BasicMovementTypes.Ground, 3, 1, "blue_team");
            var blueTeam = new Team(new List<Pawn>{ blueMeele, blueRange, blueMage }, "blue_team");

            // 3. Let's add our units to the battlefield
            var battlefieldManager = new BattlefieldManager(map, new List<Team> { redTeam, blueTeam });
            
            
            // I think it's a good idea to have something that starts the game and checks if everything is set
            battlefieldManager.StartBattle();
            
            // todo: we need to initialize units positions
            // For now let's consider that red time is 0,0; 0,1; 1,0 and blue team is 9,9; 9,8; 8,9;
            // Let's think how battle should go: meelee should be trying to get as close as possible,
            // range to run from meele and attacking from the distance, same for mage.
            // Also it should not be possible for team to move it's units when it's not their turn.
            // (Probably it's good to have something like 'FinishTurn' method, that can be called automatically when no
            // more actions could be performed or invoked manually)
            
            // Before actually performing the move, we need to display possible movement range.
            battlefieldManager.GetPawnMovementRange(redTeam.Id, redMeele.Id);
            
            // Desirable locations would be: 4,4 for red meele, 5,3 for for range and 4,3 for mage
            // For the blue team - 5,4 for blue meelee, 4, 5 for range and 5,5 for mage
            // Let's assume that fisrt turn is the red's team turn;
            battlefieldManager.MovePawn(redTeam.Id, "red_meelee", ccorr.ConvertOneOffsetToCube(new Coordinate2D { X = 1, Y = 1}));
            battlefieldManager.MovePawn(redTeam.Id, "red_range", ccorr.ConvertOneOffsetToCube(new Coordinate2D { X = 1, Y = 1}));
            battlefieldManager.MovePawn(redTeam.Id, "red_mage", ccorr.ConvertOneOffsetToCube(new Coordinate2D { X = 1, Y = 1}));
            
            // Now red's team turn should be over; Time to move blue team
            battlefieldManager.MovePawn(blueTeam.Id, "blue_meelee", ccorr.ConvertOneOffsetToCube(new Coordinate2D { X = 8, Y = 8}));
            battlefieldManager.MovePawn(blueTeam.Id, "blue_range", ccorr.ConvertOneOffsetToCube(new Coordinate2D { X = 8, Y = 8}));
            battlefieldManager.MovePawn(blueTeam.Id, "blue_mage", ccorr.ConvertOneOffsetToCube(new Coordinate2D { X = 8, Y = 8}));
            
            // Repeat until we are close enough
            // To show the list of possible options
            var attackablePawns = battlefieldManager.GetAllAttackablePawnsByPawn(blueTeam.Id, blueMeele.Id);
            var attackResult = battlefieldManager.PerformPhysicalAttack(blueMeele.TeamId, blueMeele.Id, redMeele.TeamId, redMeele.Id);
            
            // Now we need the mage to cast a damaging spell
            // First, we need to understand range
            var possibleAbilityTargets = battlefieldManager.GetPossibleAbilityTargets(blueMage, ability);
            var castResult = battlefieldManager.UseAbility(blueMage, ability, target);
        }
    }
}