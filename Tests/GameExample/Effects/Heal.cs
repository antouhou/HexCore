namespace Tests.GameExample.Effects
{
    public class Heal : IEffect
    {
        public Heal()
        {
            Attributes = new Attributes(hp: 3);
        }

        public Attributes Attributes { get; }

        public Attributes GetCasterBonus(Attributes casterAttributes)
        {
            return new Attributes(hp: casterAttributes.MagicPower);
        }

        public Attributes GetTargetBonus(Attributes targetAttributes)
        {
            return new Attributes();
        }
    }
}