using System;
using HexCore.HexGraph;

namespace NUnitLite.Tests.GameExample
{
    [Serializable]
    public class Attributes
    {
        public int Attack;
        public int AttackRange;
        public bool CanAttack;
        public bool CanCast;
        public bool CanMove;
        public int Defense;
        public int HP;
        public int MagicPower;
        public int MovementRange;
        public MovementType MovementType;
        public int MP;

        public Attributes(MovementType movementType = null, int attack = 0, int defense = 0, int hp = 0, int mp = 0,
            int magicPower = 0, int movementRange = 0, int attackRange = 0)
        {
            Attack = attack;
            Defense = defense;
            HP = hp;
            MP = mp;
            MagicPower = magicPower;
            MovementRange = movementRange;
            MovementType = movementType;
            AttackRange = attackRange;
        }
    }
}