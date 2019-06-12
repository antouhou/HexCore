using System;
using System.Collections.Generic;
using System.Linq;
using GameExample.Pawns;
using HexCore.DataStructures;
using HexCore.HexGraph;

namespace GameExample
{
    public class Team
    {
        public string Id;
        public List<Pawn> Pawns = new List<Pawn>();

        public Team(string id)
        {
            Id = id;
        }
    }

    public class BattleManager
    {
        private Graph _map;
        private readonly Dictionary<string, Team> _teams;

        public BattleManager(Graph map, Dictionary<string, Team> teams = null)
        {
            if (teams == null) teams = new Dictionary<string, Team>();

            _map = map;
            _teams = teams;
        }

        public void AddTeam(string id, IEnumerable<Pawn> pawns)
        {
            if (_teams.ContainsKey(id)) throw new InvalidOperationException("Team with such id was already added");

            var pawnsList = pawns.ToList();
            foreach (var team in _teams)
            {
                var teamPawns = team.Value.Pawns;
                var samePawns = teamPawns.Intersect(pawnsList);
                if (samePawns.Any())
                    throw new InvalidOperationException("Can't add team whose pawn is already in another team");
            }

            var newTeam = new Team(id);
            newTeam.Pawns.AddRange(pawnsList);
            _teams.Add(id, newTeam);
        }

        public void Spawn(Pawn pawn, Coordinate3D position)
        {
        }

        public void Spawn(Pawn pawn, Coordinate2D position)
        {
            Spawn(pawn, position.To3D());
        }

        public void Spawn(IEnumerable<Pawn> pawns, IEnumerable<Coordinate3D> positions)
        {
            var pawnsList = pawns.ToList();
            var positionsList = positions.ToList();
            if (pawnsList.Count != positionsList.Count)
                throw new InvalidOperationException("Pawns count is not equal to positions count");

            var pawnPositionTuples = pawnsList.Zip(positionsList, (pawn, position) => (pawn, position));
            foreach (var (pawn, position) in pawnPositionTuples) Spawn(pawn, position);
        }

        // TODO: Add team and spawn all pawns in one go
        public void SpawnTeam(string id, IEnumerable<Pawn> pawns, IEnumerable<Coordinate3D> positions)
        {
            var pawnsList = pawns.ToList();
            AddTeam(id, pawnsList);
            Spawn(pawnsList, positions);
        }

        public List<Pawn> GetTeam(string teamId)
        {
            return _teams[teamId].Pawns;
        }

        public Dictionary<string, Team> GetTeams()
        {
            return _teams;
        }

        public bool Move(Pawn pawn, Coordinate3D position)
        {
            return true;
        }

        public bool Move(Pawn pawn, Coordinate2D position)
        {
            return Move(pawn, position.To3D());
        }

        public bool Attack(Pawn attacker, Pawn target)
        {
            return true;
        }

        public bool UseAbility(Pawn caster, Ability ability, Coordinate3D target)
        {
            return true;
        }

        public Coordinate3D GetPosition(Pawn pawn)
        {
            return new Coordinate3D(1, 2, 3);
        }
    }
}