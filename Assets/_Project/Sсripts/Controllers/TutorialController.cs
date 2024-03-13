using System;
using Config;
using Cysharp.Threading.Tasks;
using Lean.Localization;
using UnityEngine.Assertions;
using View;

namespace Controllers
{
    public class TutorialController : IDisposable
    {
        private readonly PopUpNotificationView _popUp;
        private readonly TutorialButtonView _tutorialButtonView;

        private bool _isClosed = true;
        private PopUpNotificationModel _model;

        public TutorialController(PopUpNotificationView popUp, TutorialButtonView tutorialButtonView)
        {
            Assert.IsNotNull(popUp);
            Assert.IsNotNull(tutorialButtonView);
            _popUp = popUp;
            _tutorialButtonView = tutorialButtonView;

            _popUp.Hide();

            IsEnable = true;
            LeanLocalization.OnLocalizationChanged += SetContent;

            _tutorialButtonView.SetOn();
            _tutorialButtonView.Clicked += OnButtonClick;
        }

        public bool IsEnable { get; private set; }

        private void OnButtonClick()
        {
            IsEnable = !IsEnable;

            SwitchButtonView();
        }

        public void Dispose()
        {
            LeanLocalization.OnLocalizationChanged -= SetContent;
            _tutorialButtonView.Clicked -= OnButtonClick;
        }

        public void Switch(bool isEnabled)
        {
            IsEnable = isEnabled;

            SwitchButtonView();
        }

        public async UniTask Show(TutorialStep tutorialStep)
        {
            if (!IsEnable) return;

            _isClosed = false;

            _model = UiTextKeys.Get(tutorialStep);

            SetContent();

            _popUp.Clicked += Hide;
            _popUp.Show();

            if (tutorialStep == TutorialStep.LastTip)
            {
                IsEnable = false;
                SwitchButtonView();
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
            _popUp.Clicked -= Hide;
            _popUp.Hide();
            _isClosed = true;
        }

        private void SwitchButtonView()
        {
            if (IsEnable)
                _tutorialButtonView.SetOn();
            else
                _tutorialButtonView.SetOff();
        }
    }
}