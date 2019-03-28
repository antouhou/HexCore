using NUnitLite.Tests.GameExample;

namespace Tests.GameExample.Effects
{
    public interface IEffect
    {
        Attributes Attributes { get; }
        uint Duration { get; }

        Attributes GetCasterBonus(Attributes casterAttributes);
        Attributes GetTargetBonus(Attributes targetAttributes);
    }
}