using System;

namespace _Project.Sсripts.Model.Effects
{
    public abstract class Effect
    {
        public abstract void Activate(Action callNextTurn);
    }
}