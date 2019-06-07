using System;

namespace GameExample.Pawns
{
    [Serializable]
    public class PawnType
    {
        public PawnType(string name, Attributes attributes, Ability[] abilities)
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