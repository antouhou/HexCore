namespace HexCore.BattleCore.Unit
{
    public class BasicUnitDefense: IUnitDefense<BasicUnitAttack>
    {
        private const double Defense = 1;

        public double GetBlockedDamageAmount(BasicUnitAttack attack, double attackPower)
        {
            return attackPower - Defense;
        }
    }
}