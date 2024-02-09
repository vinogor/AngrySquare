using System;

namespace Sсripts.Dmg
{
    public class Damage
    {
        public Damage(int value)
        {
            if (value < 1)
                throw new ArgumentOutOfRangeException(nameof(value), "value cant be less then 1");

            Value = value;
        }

        public int Value { get; private set; }
    }
}