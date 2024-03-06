using System;
using _Project.SDK;
using _Project.View;
using UnityEngine.Assertions;

namespace _Project.Controllers
{
    public class LanguageController : IDisposable
    {
        private readonly LanguageView _languageView;
        private readonly Localization _localization;
        private bool _isEnabled;

        public LanguageController(LanguageView languageView, Localization localization)
        {
            Assert.IsNotNull(languageView);
            Assert.IsNotNull(localization);
            
            _languageView = languageView;
            _localization = localization;

            _languageView.SetSprite(_localization.GetCurrent());
            _languageView.ButtonOnClick.AddListener(OnButtonClick);
        }

        public void Dispose() => _languageView.ButtonOnClick.RemoveListener(OnButtonClick);

        private void OnButtonClick()
        {
            string currentLang = _localization.GetCurrent();

            switch (currentLang)
            {
                case LanguageCodes.LeanRussianCode:
                    _localization.SetLanguage(LanguageCodes.LeanEnglishCode);
                    _languageView.SetSprite(LanguageCodes.LeanEnglishCode);
                    break;

                case LanguageCodes.LeanEnglishCode:
                    _localization.SetLanguage(LanguageCodes.LeanTurkishCode);
                    _languageView.SetSprite(LanguageCodes.LeanTurkishCode);
                    break;

                case LanguageCodes.LeanTurkishCode:
                    _localization.SetLanguage(LanguageCodes.LeanRussianCode);
                    _languageView.SetSprite(LanguageCodes.LeanRussianCode);
                    break;
            }
        }
    }
}