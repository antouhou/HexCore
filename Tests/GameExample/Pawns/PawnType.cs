using System;
using Tests.GameExample.Abilities;

namespace Tests.GameExample.Pawns
{
    [Serializable]
    public class PawnType
    {
        public PawnType(string name, Attributes attributes, IAbility[] abilities)
        {
        }

        public Pawn CreatePawn()
        {
            return new Pawn();
        }

        // Todo: We need to store and load pawn types
        public void Save()
        {
        }

        public void Load()
        {
        }
    }
}