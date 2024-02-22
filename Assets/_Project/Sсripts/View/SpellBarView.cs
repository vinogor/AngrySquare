using System;
using System.Collections.Generic;
using _Project.Sсripts.Config;
using _Project.Sсripts.Domain;
using _Project.Sсripts.Domain.Spells;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace _Project.Sсripts.View
{
    public class SpellBarView : MonoBehaviour
    {
        [SerializeField] private List<SpellItemView> _items;
        [SerializeField] [Required] private Button _skipButton;
        [SerializeField] [Required] private SpellsSettings _spellsSettings;

        private AvailableSpells _availableSpells;

        public void Initialize(AvailableSpells availableSpells)
        {
            Assert.AreEqual(5, _items.Count);

            _availableSpells = availableSpells;
            _availableSpells.Updated += OnAvailableSpellsUpdated;
            _skipButton.onClick.AddListener(() => Skipped?.Invoke());
        }

        public event Action<int, SpellName> SpellsActivated;
        public event Action Skipped;

        private void OnAvailableSpellsUpdated()
        {
            Clean();

            List<SpellName> spellNames = _availableSpells.SpellNames;

            for (var i = 0; i < spellNames.Count; i++)
            {
                int index = i;
                SpellName spellName = spellNames[index];
                Sprite sprite = _spellsSettings.GetSprite(spellName);
                int manaCost = _spellsSettings.GetManaCost(spellName);
                _items[index].SetContent(sprite, manaCost, () => SpellsActivated?.Invoke(index, spellName));
            }
        }

        public void Clean()
        {
            _items.ForEach(item => item.SetEmptyContent());
        }

        public void Disable()
        {
            _skipButton.enabled = false;
            _items.ForEach(item => item.Disable());
        }

        public void Enable()
        {
            _skipButton.enabled = true;
            _items.ForEach(item => item.Enable());
        }

        private void OnDestroy()
        {
            _availableSpells.Updated -= OnAvailableSpellsUpdated;
            _skipButton.onClick.RemoveAllListeners();
        }
    }
}