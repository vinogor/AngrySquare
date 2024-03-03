using System;
using _Project.Config;
using Lean.Localization;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Project.View
{
    public class SpellItemView : MonoBehaviour
    {
        [SerializeField] [Required] private TextMeshProUGUI _text;
        [SerializeField] [Required] private Image _image;
        [SerializeField] [Required] private Button _button;
        [SerializeField] [Required] private Sprite _emptySprite;

        public void Disable()
        {
            _button.enabled = false;
        }

        public void Enable()
        {
            _button.enabled = true;
        }

        public void SetContent(Sprite sprite, int manaCost, UnityAction unityAction)
        {
            SetSprite(sprite);
            string introText = LeanLocalization.GetTranslationText(UiTextKeys.SpellBarCostKey);
            SetText(introText + ": " + manaCost);
            _button.onClick.AddListener(unityAction);
        }

        public void SetEmptyContent()
        {
            SetSprite(_emptySprite);
            SetText(String.Empty);
            _button.onClick.RemoveAllListeners();
        }

        private void SetSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        private void SetText(string text)
        {
            _text.SetText(text);
        }
    }
}