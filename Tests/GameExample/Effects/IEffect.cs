namespace Tests.GameExample.Effects
{
    public interface IEffect
    {
        Attributes Attributes { get; }

        Attributes GetCasterBonus(Attributes casterAttributes);
        Attributes GetTargetBonus(Attributes targetAttributes);
    }
}