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
            const string redTeamId = "red_team";
            const string blueTeamId = "blue_team";
            var redTeam = new Team(new List<Pawn>
            {
                new Pawn("red_meele"), new Pawn("red_range"), new Pawn("red_mage")
            }, redTeamId);
            var blueTeam = new Team(new List<Pawn>
            {
                new Pawn("blue_meele"), new Pawn("blue_range"), new Pawn("blue_mage")
            }, blueTeamId);
            var teams = new List<Team> {redTeam, blueTeam};

            // 3. Let's add our units to the battlefield
            var battlefieldManager = new BattlefieldManager(map, teams);
            
            
            // I think it's a good idea to have something that starts the game and checks if everything is set
            battlefieldManager.StartBattle();
            
            // todo: we need to initialize units positions
            // For now let's consider that red time is 0,0; 0,1; 1,0 and blue team is 9,9; 9,8; 8,9;
            // Let's think how battle should go: meelee should be trying to get as close as possible,
            // range to run from meele and attacking from the distance, same for mage.
            // Also it should not be possible for team to move it's units when it's not their turn.
            // (Probably it's good to have something like 'FinishTurn' method, that can be called automatically when no
            // more actions could be performed or invoked manually)
            
            // Let's assume that fisrt turn is the red's team turn;
            battlefieldManager.MovePawn(redTeamId, "red_meelee", ccorr.ConvertOneOffsetToCube(new Coordinate2D { X = 1, Y = 1}));
            battlefieldManager.MovePawn(redTeamId, "red_range", ccorr.ConvertOneOffsetToCube(new Coordinate2D { X = 1, Y = 1}));
            battlefieldManager.MovePawn(redTeamId, "red_mage", ccorr.ConvertOneOffsetToCube(new Coordinate2D { X = 1, Y = 1}));
            
            // Now red's team turn should be over; Time to move blue team
            battlefieldManager.MovePawn(blueTeamId, "blue_meelee", ccorr.ConvertOneOffsetToCube(new Coordinate2D { X = 8, Y = 8}));
            battlefieldManager.MovePawn(blueTeamId, "blue_range", ccorr.ConvertOneOffsetToCube(new Coordinate2D { X = 8, Y = 8}));
            battlefieldManager.MovePawn(blueTeamId, "blue_mage", ccorr.ConvertOneOffsetToCube(new Coordinate2D { X = 8, Y = 8}));
            
            // Repeat until we are close enough
            // To show the list of possible options
            battlefieldManager.GetAllAttackablePawns(blueTeamId, "blue_meelee");

        }
    }
}