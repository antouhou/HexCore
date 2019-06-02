using System;
using System.Collections.Generic;
using System.Linq;
using HexCore.DataStructures;
using HexCore.HexGraph;
using Tests.GameExample.Abilities;
using Tests.GameExample.Pawns;

namespace Tests.GameExample
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
        private Dictionary<string, Team> _teams;

        public BattleManager(Graph map, Dictionary<string, Team> teams = null)
        {
            if (teams == null)
            {
                teams = new Dictionary<string, Team>();
            }

            _map = map;
            _teams = teams;
        }

        public bool Spawn(Pawn pawn, Coordinate3D position)
        {
            return true;
        }

        public bool Spawn(Pawn pawn, Coordinate2D position)
        {
            return Spawn(pawn, position.To3D());
        }

        public bool Spawn(IEnumerable<(Pawn, Coordinate3D)> pawns)
        {
            var res = false;
            // Todo: make this atomic, i.e. check that all pawns can be spawn before spawning them
            foreach (var (pawn, position) in pawns)
            {
                res = Spawn(pawn, position);
                if (!res) return false;
            }

            return res;
        }

        public void AddTeam(string id, IEnumerable<Pawn> pawns)
        {
            if (_teams.ContainsKey(id))
            {
                throw new InvalidOperationException("Team with such id was already added");
            }

            var pawnsList = pawns.ToList();
            foreach (var team in _teams)
            {
                var teamPawns = team.Value.Pawns;
                var samePawns = teamPawns.Intersect(pawnsList);
                if (samePawns.Any())
                {
                    throw new InvalidOperationException("Can't add team whose pawn is already in another team");
                }
            }

            var newTeam = new Team(id);
            newTeam.Pawns.AddRange(pawnsList);
            _teams.Add(id, newTeam);
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