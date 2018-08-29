using System.Collections.Generic;
using System.Linq;
using HexCore.DataStructures;
using HexCore.HexGraph;

namespace HexCore.BattleCore
{
    public class BattlefieldManager
    {
        public readonly Graph Graph;
        private readonly List<Team> _teams = new List<Team>();
        private bool _isBattleStarted = false;
        private bool _isBattleFinished = false;

        public BattlefieldManager(Graph map, IEnumerable<Team> teams)
        {
            Graph = map;
            AddTeams(teams);
        }

        public List<Pawn> GetAllPawns()
        {
            return _teams.SelectMany(team => team.GetAllPawns()).ToList();
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

        public List<Coordinate3D> GetPawnMovementRange(Pawn pawn)
        {
            // TODO: maybe it's a good idea to have some pawn parameters immutable, and have an `Effects` property, 
            // which will modify pawn's params. Also it's good to have something like pawn.clearAllEffects();
            return Graph.GetMovementRange(pawn.Position, pawn.MovementPoints, pawn.MovementType);
        }

        public bool MovePawn(Pawn pawn, Coordinate3D newPosition)
        {
            // TODO: check if it's that team's turn now
            // Also check if there is such pawn at the battlefield
            if (!CanMoveTo(pawn, newPosition))
            {
                return false;
            }
            
            Graph.SetOneCellBlocked(pawn.Position, false);
            pawn.Position = newPosition;
            Graph.SetOneCellBlocked(pawn.Position, true);
            return true;
        }

        public List<Pawn> GetAllAttackablePawnsByPawn(Pawn pawn)
        {
            var pawns = GetAllPawnsInRange(pawn.Position, pawn.PhysicalAttackRange).ToList();
            return pawns;
        }

        public bool CanMoveTo(Pawn pawn, Coordinate3D position)
        {
            var movementRange = GetPawnMovementRange(pawn);
            return movementRange.Contains(position);
        }

        public bool CanAttack(Pawn attackingPawn, Pawn targetPawn)
        {
            // TODO: redo all pawns to use only pawn id
            var attackablePawns = GetAllAttackablePawnsByPawn(attackingPawn);
            // TODO: this won't work because of equality of pawns
            return attackablePawns.Contains(targetPawn);
        }

        public int PerformPhysicalAttack(Pawn attackingPawn, Pawn targetPawn)
        {
            if (CanAttack(attackingPawn, targetPawn))
            {
                // TODO: Here we need to calculate attacking pawn's power and defending pawn's defence
                return 1;
            }

            // TODO: probably should throw in this case
            return 0;
        }

        public IEnumerable<Pawn> GetAllPawnsInRange(Coordinate3D center, int radius)
        {
            var range = Graph.GetRange(center, radius);
            var pawns = GetAllPawns();
            var pawnsInRange = pawns.Where(pawn => range.Contains(pawn.Position));
            return pawnsInRange;
        }

        public IEnumerable<Pawn> GetPossibleAbilityTargets(Pawn pawn, Ability ability)
        {
            // TODO: Check that pawn has an ability
            var pawns = GetAllPawnsInRange(pawn.Position, ability.Range);
            // TODO: Filter pawns
            return pawns;
        }

        public int UseAbility(Pawn caster, Ability ability, Pawn target)
        {
            return 1;
            // Get caster bonus
            // Apply to ability's power
            // Apply damage/effect to the target
            // TODO
        }
        
        public void RemovePawn() {}
        public void EndTurn() {}
        
        public void SaveBattleState() {}
        public void LoadBattleState() {}
    }
}