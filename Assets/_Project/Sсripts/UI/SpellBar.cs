using System.Collections.Generic;
using _Project.Sсripts.Scriptable;
using _Project.Sсripts.Utility;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Sсripts.UI
{
    public class SpellBar : MonoBehaviour
    {
        [SerializeField] private List<SpellItem> _items;

        // TODO: попробовать без кнопки пропуска? 
        [SerializeField] [Required] private Button _skipButton;

        private void Awake()
        {
            Validator.ValidateAmount(_items, 5); 
        }

        public void Clean()
        {
            _items.ForEach(item => item.SetEmptyContent());
            // TODO: отписки от кнопок 
        }

        public void Disable()
        {
            _skipButton.enabled = false;
            _items.ForEach(item => item.Disable());
        }

        public void AddSpell(SpellName spellName, Sprite sprite, int manaCost)
        {
            Debug.Log($"AddSpell {spellName}");

            for (var i = _items.Count - 1; i >= 1; i--)
            {
                _items[i].SetContent(_items[i - 1]);
            }

            _items[0].SetContent(sprite, manaCost);
        }

        public void ActivateSpell(SpellName spellName)
        {
            Debug.Log($"Activate spell {spellName}");
        }
    }
}