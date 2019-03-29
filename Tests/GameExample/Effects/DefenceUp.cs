namespace Tests.GameExample.Effects
{
    public class DefenceUp : IEffect
    {
        public DefenceUp()
        {
            Attributes = new Attributes(defense: 1);
        }

        public Attributes Attributes { get; }

        public Attributes GetCasterBonus(Attributes casterAttributes)
        {
            return new Attributes(defense: casterAttributes.MagicPower);
        }

        public Attributes GetTargetBonus(Attributes targetAttributes)
        {
            return new Attributes();
        }
    }
}