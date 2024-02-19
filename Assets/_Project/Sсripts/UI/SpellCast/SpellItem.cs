using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Project.Sсripts.UI.SpellCast
{
    public class SpellItem : MonoBehaviour
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

        public void SetContent(Sprite sprite, int manaCost,  UnityAction unityAction)
        {
            SetSprite(sprite);
            SetText("cost: " + manaCost);
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