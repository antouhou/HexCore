using NUnitLite.Tests.GameExample;

namespace Tests.GameExample.Effects
{
    public class Fire : IEffect
    {
        public Fire(uint duration)
        {
            Duration = duration;
            Attributes = new Attributes(hp: -3);
        }

        public Attributes Attributes { get; }

        public uint Duration { get; }

        public Attributes GetCasterBonus(Attributes casterAttributes)
        {
            return new Attributes(hp: -casterAttributes.MagicPower);
        }

        public Attributes GetTargetBonus(Attributes targetAttributes)
        {
            return new Attributes();
        }
    }
}