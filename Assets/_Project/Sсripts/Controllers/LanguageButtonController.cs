using SDK;
using Services;
using UnityEngine.Assertions;
using View;

namespace Controllers
{
    public class LanguageButtonController : IPresenter
    {
        private readonly LanguageButtonView _languageButtonView;
        private readonly Localization _localization;
        
        private bool _isEnabled;

        public LanguageButtonController(LanguageButtonView languageButtonView, Localization localization)
        {
            Assert.IsNotNull(languageButtonView);
            Assert.IsNotNull(localization);

            _languageButtonView = languageButtonView;
            _localization = localization;
        }

        public void Enable()
        {
            _languageButtonView.SetSprite(_localization.GetCurrent());
            _languageButtonView.Clicked += OnButtonClick;
        }

        public void Disable()
        {
            _languageButtonView.Clicked -= OnButtonClick;
        }

        private void OnButtonClick()
        {
            string currentLang = _localization.GetCurrent();

            switch (currentLang)
            {
                case LanguageCodes.LeanRussianCode:
                    _localization.SetLanguage(LanguageCodes.LeanEnglishCode);
                    _languageButtonView.SetSprite(LanguageCodes.LeanEnglishCode);
                    break;

                case LanguageCodes.LeanEnglishCode:
                    _localization.SetLanguage(LanguageCodes.LeanTurkishCode);
                    _languageButtonView.SetSprite(LanguageCodes.LeanTurkishCode);
                    break;

                case LanguageCodes.LeanTurkishCode:
                    _localization.SetLanguage(LanguageCodes.LeanRussianCode);
                    _languageButtonView.SetSprite(LanguageCodes.LeanRussianCode);
                    break;
            }
        }
    }
}