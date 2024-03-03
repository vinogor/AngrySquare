using System;
using _Project.Controllers.Sound;
using _Project.Domain.Effects;
using UnityEngine;

namespace _Project.Domain.Spells
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