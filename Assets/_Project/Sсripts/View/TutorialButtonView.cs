using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class TutorialButtonView : MonoBehaviour
    {
        [SerializeField] [Required] private Button _button;
        [SerializeField] [Required] private Image _image;
        [SerializeField] [Required] private Sprite _spriteOn;
        [SerializeField] [Required] private Sprite _spriteOff;

        public event Action Clicked;

        private void OnEnable()
        {
            _button.onClick.AddListener(Click);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Click);
        }

        private void Click()
        {
            Clicked?.Invoke();
        }

        public void SetOn()
        {
            _image.sprite = _spriteOn;
        }

        public void SetOff()
        {
            _image.sprite = _spriteOff;
        }
    }
}