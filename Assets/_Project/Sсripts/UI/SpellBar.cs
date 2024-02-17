using System;
using System.Collections.Generic;
using _Project.Sсripts.Model;
using _Project.Sсripts.Model.Effects;
using _Project.Sсripts.Scriptable;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Sсripts.UI
{
    public class SpellBar : MonoBehaviour
    {
        [SerializeField] private List<Button> _buttons;
        [SerializeField] [Required] private Button _skipButton;

        public void Clean(Sprite emptySprite)
        {
            _buttons.ForEach(button =>
            {
                SetSprite(button, emptySprite);
                SetText(button, String.Empty);
            });
        }

        public void Disable()
        {
            _skipButton.enabled = false;
            _buttons.ForEach(button => button.enabled = false);
        }

        public void AddSpell(SpellName spellName, Sprite sprite, int manaCost)
        {
            Debug.Log($"AddSpell {spellName}");

            for (var i = _buttons.Count - 1; i >= 1; i--)
            {
                SetSprite(_buttons[i], GetSprite(_buttons[i - 1]));
                SetText(_buttons[i], GetText(_buttons[i - 1]));
            }

            SetSprite(_buttons[0], sprite);
            SetText(_buttons[0], "cost: " + manaCost);
        }

        public void ActivateSpell(SpellName spellName)
        {
            Debug.Log($"Activate spell {spellName}");
        }

        private string GetText(Button button)
        {
            return button.GetComponentInChildren<ManaCostText>().GetComponent<TextMeshProUGUI>()
                .GetParsedText();
        }

        private void SetText(Button button, string text)
        {
            button.GetComponentInChildren<ManaCostText>().GetComponent<TextMeshProUGUI>().SetText(text);
        }
        
        private Sprite GetSprite(Button button)
        {
            return button.GetComponentInChildren<ButtonImage>().GetComponent<Image>().sprite;
        }
        
        private void SetSprite(Button button, Sprite sprite)
        {
            button.GetComponentInChildren<ButtonImage>().GetComponent<Image>().sprite = sprite;
        }
    }
}