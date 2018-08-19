using HexCore.BattleCore;
using HexCore.HexGraph;
using NUnit.Framework;

namespace Tests.BattleCore
{
    [TestFixture]
    public class BattlefieldManagerTest
    {
        [Test]
        public void ShouldBeAbleToRunBattleFromTheBeginningToTheEnd()
        {
            //This is an integration test for battlefild, aimed to help to design structure and interfaces.

            // 1. Let's initialize the map:
            var map = GraphFactory.CreateSquareGraph(height: 5, width: 5);

            // 2. Let's create some units. Let's consider one meele, one ranged and one mage on each side.
            var redTeamMeele = new Unit();
            var redTeamRange = new Unit();
            var redTeamMage = new Unit();
            // We also need to manage different teams.
            // It will be useful to add something like this: team.GetAllUnits();
            // or battlefiled.GetAllUnitsThatCanPerformActionsByTeamId(); (Looks shitty, definitly need a better name :D)
            // Maybe it also can be useful to have something like: battlefield.GetTeamById(); and store team id inside team class
            // battlefield.IsTeamTurn(teamId);
            var redTeam = new List();
            var blueTeam = new List();

            // 3. Let's add our units to the battlefield
            var battlefieldManager = new BattlefieldManager();
            battlefieldManager.AddUnits(redTeam, blueTeam);
            
            // 4. Now we're set. Let's play a game!
            battlefieldManager.MoveTo();
        }
    }
}