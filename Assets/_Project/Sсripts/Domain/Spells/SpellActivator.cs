using System;
using System.Collections.Generic;
using Controllers.Sound;
using Domain.Effects;
using UnityEngine.Assertions;

namespace Domain.Spells
{
    public class SpellActivator
    {
        private readonly Dictionary<SpellName, Effect> _playerSpells;
        private readonly GameSoundsPresenter _gameSoundsPresenter;

        public SpellActivator(Dictionary<SpellName, Effect> playerSpells, GameSoundsPresenter gameSoundsPresenter)
        {
            Assert.IsNotNull(playerSpells);
            Assert.IsNotNull(gameSoundsPresenter);

            _playerSpells = playerSpells;
            _gameSoundsPresenter = gameSoundsPresenter;
        }

        public void Activate(SpellName spellName, Action onComplete)
        {
            _gameSoundsPresenter.PlaySpellCast();
            _playerSpells[spellName].Activate(onComplete);
        }
    }
}