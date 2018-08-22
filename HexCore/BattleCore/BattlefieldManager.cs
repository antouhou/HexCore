using System.Collections.Generic;
using System.Linq;
using HexCore.DataStructures;
using HexCore.HexGraph;

namespace HexCore.BattleCore
{
    public class BattlefieldManager
    {
        private readonly Graph _graph;
        private readonly List<Team> _teams = new List<Team>();
        private bool _isBattleStarted = false;
        private bool _isBattleFinished = false;

        public BattlefieldManager(Graph map, IEnumerable<Team> teams)
        {
            _graph = map;
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

        public List<Coordinate3D> GetPawnMovementRange(string teamId, string pawnId)
        {
            var pawn = GetPawnById(teamId, pawnId);
            // TODO: maybe it's a good idea to have some pawn parameters immutable, and have an `Effects` property, 
            // which will modify pawn's params. Also it's good to have something like pawn.clearAllEffects();
            return _graph.GetMovementRange(pawn.Position, pawn.MovementPoints, pawn.MovementType);
        }

        public void MovePawn(string teamId, string unitId, Coordinate3D newPosition)
        {
            // TODO: check if it's that team's turn now
            var pawn = GetPawnById(teamId, unitId);
            if (!CanMoveTo(pawn, newPosition)) return;
            
            _graph.SetOneCellBlocked(pawn.Position, false);
            pawn.Position = newPosition;
            _graph.SetOneCellBlocked(pawn.Position, true);
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

        public List<Pawn> GetAllAttackablePawnsByPawn(string teamId, string pawnId)
        {
            var pawn = GetPawnById(teamId, pawnId);
            var attackRange = _graph.GetRange(pawn.Position, pawn.PhysicalAttackRange);
            // TODO: complete this method
            var pawns = new List<Pawn>();
            return pawns;
        }
        
        public void RemoveUnit() {}

        public bool CanMoveTo(Pawn pawn, Coordinate3D position)
        {
            var movementRange = GetPawnMovementRange(pawn.TeamId, pawn.Id);
            return movementRange.Contains(position);
        }

        public bool CanAttack(Pawn attackingPawn, Pawn targetPawn)
        {
            // TODO: redo all pawns to use only pawn id
            var attackablePawns = GetAllAttackablePawnsByPawn(attackingPawn.TeamId, attackingPawn.Id);
            // TODO: this won't work because of equality of pawns
            return attackablePawns.Contains(targetPawn);
        }

        public int PerformPhysicalAttack(string attackingTeamId, string attackingPawnId, string defendingTeamId, string defendingPawnId)
        {
            var attackingPawn = GetPawnById(attackingTeamId, attackingPawnId);
            var defendingPawn = GetPawnById(defendingTeamId, defendingPawnId);
            if (CanAttack(attackingPawn, defendingPawn))
            {
                // TODO: Here we need to calculate attacking pawn's power and defending pawn's defence
                return 1;
            }

            // TODO: probably should throw in this case
            return 0;
        }
        
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