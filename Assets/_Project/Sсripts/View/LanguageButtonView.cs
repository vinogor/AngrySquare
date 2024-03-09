using System;
using NaughtyAttributes;
using SDK;
using Services;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace View
{
    public class LanguageButtonView : MonoBehaviour
    {
        [SerializeField] [Required] private Button _button;
        [SerializeField] [Required] private Image _image;
        [SerializeField] [Required] private Sprite _spriteRu;
        [SerializeField] [Required] private Sprite _spriteEn;
        [SerializeField] [Required] private Sprite _spriteTr;

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
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _presenter.Disable();
            _button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            Clicked?.Invoke();
        }

        public void SetSprite(string languageCode)
        {
            switch (languageCode)
            {
                case LanguageCodes.LeanRussianCode:
                    _image.sprite = _spriteRu;
                    break;

                case LanguageCodes.LeanEnglishCode:
                    _image.sprite = _spriteEn;
                    break;

                case LanguageCodes.LeanTurkishCode:
                    _image.sprite = _spriteTr;
                    break;
            }
        }
    }
}