using System;
using System.Threading.Tasks;
using Config;
using Cysharp.Threading.Tasks;
using Lean.Localization;
using UnityEngine.Assertions;
using View;

namespace Controllers
{
    public class PopUpTutorialController : IDisposable
    {
        private readonly PopUpNotificationView _popUp;
        private bool _isClosed = true;
        private PopUpNotificationModel _model;

        public PopUpTutorialController(PopUpNotificationView popUp)
        {
            Assert.IsNotNull(popUp);

            _popUp = popUp;
            IsEnable = true;
            LeanLocalization.OnLocalizationChanged += SetContent;
        }

        public void Dispose()
        {
            LeanLocalization.OnLocalizationChanged -= SetContent;
        }

        public bool IsEnable { get; private set; }

        public void Switch(bool isEnabled)
        {
            IsEnable = isEnabled;
        }

        public async UniTask Show(TutorialStep tutorialStep)
        {
            if (!IsEnable) return;

            _isClosed = false;

            _model = UiTextKeys.Get(tutorialStep);
            
            SetContent();

            _popUp.Button.onClick.AddListener(Hide);
            _popUp.Show();

            if (tutorialStep == TutorialStep.LastTip)
            {
                IsEnable = false;
            }

            await UniTask.WaitUntil(() => _isClosed);
        }

        private void SetContent()
        {
            if (_model == null)
                return;
            
            string title = LeanLocalization.GetTranslationText(_model.Title);
            string info = LeanLocalization.GetTranslationText(_model.Info);
            _popUp.SetContent(title, info);
        }

        private void Hide()
        {
            _popUp.Button.onClick.RemoveListener(Hide);
            _popUp.Hide();
            _isClosed = true;
        }
    }
}