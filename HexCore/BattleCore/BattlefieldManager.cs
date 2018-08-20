using System.Collections.Generic;
using System.Linq;
using HexCore.DataStructures;
using HexCore.HexGraph;

namespace HexCore.BattleCore
{
    public class BattlefieldManager
    {
        private Graph _map;
        private List<Team> _teams = new List<Team>();
        private bool _isBattleStarted = false;
        private bool _isBattleFinished = false;

        public BattlefieldManager(Graph map, IEnumerable<Team> teams)
        {
            _map = map;
            AddTeams(teams);
        }
        
        public void AddTeams(IEnumerable<Team> teams)
        {
            // TODO: check if pawn ids and positions don't conflict
            // check that all units fit the battlefield
            _teams.AddRange(teams);
        }

        public void StartBattle()
        {
            // TODO: check if everything is set and we can start the battle.
            // some conditions:
            // there are at least two teams;
            // each team has at least one pawn that can perform actions;
            // can't start battle if it's already started
            _isBattleStarted = true;
        }

        public void FinishBattle()
        {
            // TODO: check all conditions and finish the battle
            // can't finish battle if it's isn't started or already finished
            _isBattleFinished = true;
        }

        public bool IsBattleStarted()
        {
            return _isBattleStarted;
        }

        public bool IsBattleFinished()
        {
            return _isBattleFinished;
        }

        public void MovePawn(string teamId, string unitId, Coordinate3D newPosition)
        {
            var pawn = GetPawnById(teamId, unitId);
            
            pawn.Position = newPosition;
        }

        public Team GetTeamById(string teamId)
        {
            var team = _teams.Single(t => t.Id == teamId);
            // TODO: check if there is such a team
            return team;
        }

        public Pawn GetPawnById(string teamId, string pawnId)
        {
            return GetTeamById(teamId).GetPawnById(pawnId);
        }
        
        public void RemoveUnit() {}
        
        public void CanMoveTo() {}
        public void CanAttack() {}
        
        public void PerformMeeleAttack() {}
        public void UseAbility() {}
        
        public void EndTurn() {}
        
        public void SaveBattleState() {}
        public void LoadBattleState() {}
    }

    public class Ability
    {
        
    }

    public class MeeleAttack
    {
        
    }
}