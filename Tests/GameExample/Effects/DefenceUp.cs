using NUnitLite.Tests.GameExample;

namespace Tests.GameExample.Effects
{
    public class DefenceUp : IEffect
    {
        public DefenceUp(uint duration)
        {
            Duration = duration;
            Attributes = new Attributes(defense: 1);
        }

        public Attributes Attributes { get; }

        public uint Duration { get; }

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