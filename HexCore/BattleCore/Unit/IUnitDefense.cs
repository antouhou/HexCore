namespace HexCore.BattleCore.Unit
{
    public interface IUnitDefense<in TUnitAttack>
    {
        double GetBlockedDamageAmount(TUnitAttack attack, double attackPower);
    }
}