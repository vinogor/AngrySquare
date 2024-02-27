using System;
using _Project.Sсripts.Controllers.Sound;
using _Project.Sсripts.Domain.Effects;
using UnityEngine;

namespace _Project.Sсripts.Domain.Spells
{
    public abstract class Spell : Effect
    {
        private readonly GameSounds _gameSounds;

        protected Spell(GameSounds gameSounds)
        {
            _gameSounds = gameSounds;
        }

        public override void Activate(Action onComplete)
        {
            Debug.Log($"Spell - {GetType().Name} - Activate");
            _gameSounds.PlaySpellCast();
            Execute(onComplete);
        }
    }
}