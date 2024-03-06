using Agava.YandexGames;
using Lean.Localization;

namespace _Project.SDK
{
    public class Localization
    {
        private readonly LeanLocalization _leanLocalization;

        public Localization(LeanLocalization leanLocalization)
        {
            _leanLocalization = leanLocalization;
        }

        public void SetLanguageFromYandex()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
         Execute();
#endif
        }

        private void Execute()
        {
            string langCode = YandexGamesSdk.Environment.i18n.lang;

            switch (langCode)
            {
                case LanguageCodes.YandexEnglishCode:
                    _leanLocalization.SetCurrentLanguage(LanguageCodes.LeanEnglishCode);
                    break;

                case LanguageCodes.YandexRussianCode:
                    _leanLocalization.SetCurrentLanguage(LanguageCodes.LeanRussianCode);
                    break;

                case LanguageCodes.YandexTurkishCode:
                    _leanLocalization.SetCurrentLanguage(LanguageCodes.LeanTurkishCode);
                    break;
            }
        }

        public void SetLanguage(string leanLanguageCode)
        {
            _leanLocalization.SetCurrentLanguage(leanLanguageCode);
        }

        public string GetCurrent()
        {
            return _leanLocalization.CurrentLanguage;
        }
    }
}