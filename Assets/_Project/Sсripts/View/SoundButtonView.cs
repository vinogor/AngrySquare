using System;
using NaughtyAttributes;
using Services;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace View
{
    public class SoundButtonView : MonoBehaviour
    {
        [SerializeField] [Required] private Button _button;
        [SerializeField] [Required] private Image _image;
        [SerializeField] [Required] private Sprite _spriteOn;
        [SerializeField] [Required] private Sprite _spriteOff;

        private IPresenter _presenter;

        public void Initialize(IPresenter presenter)
        {
            Assert.IsNotNull(presenter);
            _presenter = presenter;
        }    
        public event Action Clicked;

        private void OnEnable()
        {
            _presenter.Enable();
            _button.onClick.AddListener(Click);
        }

        private void OnDisable()
        {
            _presenter.Disable();
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