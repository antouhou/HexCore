using System.Collections.Generic;
using Tests.GameExample.Abilities;
using Tests.GameExample.Effects;
using Tests.GameExample.Pawns;

namespace Tests.GameExample
{
    public class Game
    {
        public Dictionary<string, Ability> Abilities;
        public BattleManager BattleManager;
        public Dictionary<string, Effect> Effects;
        public Dictionary<string, PawnType> PawnTypes;

        public Game(
            Dictionary<string, Effect> effects, Dictionary<string, Ability> abilities,
            Dictionary<string, PawnType> pawnTypes, BattleManager battleManager
        )
        {
            Effects = effects;
            Abilities = abilities;
            PawnTypes = pawnTypes;
            BattleManager = battleManager;
        }
    }
}