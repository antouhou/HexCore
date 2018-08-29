namespace HexCore.BattleCore
{
    public class Ability
    {
        public int Range;
        public int BasePower;
        public int Cost;

        public Ability(int range, int basePower, int cost = 1)
        {
            Range = range;
            BasePower = basePower;
            Cost = cost;
        }
    }
}