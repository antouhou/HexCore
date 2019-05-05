using System.Collections.Generic;
using HexCore.DataStructures;
using HexCore.HexGraph;
using Tests.GameExample.Abilities;
using Tests.GameExample.Pawns;

namespace Tests.GameExample
{
    public class BattleManager
    {
        private Graph _map;

        public BattleManager(Graph map)
        {
            _map = map;
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