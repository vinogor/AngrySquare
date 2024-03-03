using _Project.SDK;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.View
{
    public class LanguageView : MonoBehaviour
    {
        [SerializeField] [Required] private Button _button;
        [SerializeField] [Required] private Image _image;
        [SerializeField] [Required] private Sprite _spriteRu;
        [SerializeField] [Required] private Sprite _spriteEn;
        [SerializeField] [Required] private Sprite _spriteTr;

        public Button.ButtonClickedEvent ButtonOnClick => _button.onClick;

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