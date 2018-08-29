using System.Collections.Generic;
using System.Linq;

namespace HexCore.BattleCore
{
    public class Team
    {
        private List<Pawn> _pawns = new List<Pawn>();
        public string Id { get; }
        
        public Team(IEnumerable<Pawn> units, string id)
        {
            Id = id;
            AddPawns(units);
        }

        public void AddPawns(IEnumerable<Pawn> pawns)
        {
           _pawns.AddRange(pawns);
        }

        public List<Pawn> GetAllPawns()
        {
            return _pawns;
        }

        public Pawn GetPawnById(string pawnId)
        {
            var pawn = _pawns.Single(p => p.Id == pawnId);
            // Throw if there is no such pawn
            return pawn;
        }
    }
}