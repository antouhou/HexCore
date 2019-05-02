namespace HexCore.BattleCore
{
    public class Ability
    {
        public int BasePower;
        public int Cost;
        public int Range;

        public Ability(int range, int basePower, int cost = 1)
        {
            Range = range;
            BasePower = basePower;
            Cost = cost;
        }
    }
}