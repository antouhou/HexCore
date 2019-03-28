using System;
using HexCore.DataStructures;
using HexCore.HexGraph;
using NUnitLite.Tests.GameExample.UnitTypes;
using Tests.GameExample.Abilities;

namespace NUnitLite.Tests.GameExample
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

        public bool Spawn(ValueTuple<Pawn, Coordinate3D>[] pawns)
        {
            var res = false;
            // Todo: make this atomic, i.e. check that all pawns can be spawn before spawning them
            foreach (var pawn in pawns)
            {
                res = Spawn(pawn.Item1, pawn.Item2);
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