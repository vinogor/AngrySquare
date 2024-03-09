using Config;
using Cysharp.Threading.Tasks;
using Lean.Localization;
using Services;
using UnityEngine.Assertions;
using View;

namespace Controllers
{
    public class PopUpTutorialController : IPresenter
    {
        private readonly PopUpNotificationView _popUp;
        private bool _isClosed = true;
        private PopUpNotificationModel _model;

        public PopUpTutorialController(PopUpNotificationView popUp)
        {
            Assert.IsNotNull(popUp);
            _popUp = popUp;
            Hide();
        }

        public bool IsEnable { get; private set; }

        public void Enable()
        {
            IsEnable = true;
            LeanLocalization.OnLocalizationChanged += SetContent;
        }

        public void Disable()
        {
            LeanLocalization.OnLocalizationChanged -= SetContent;
        }

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

            _popUp.Clicked += Hide;
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
            _popUp.Clicked -= Hide;
            _popUp.Hide();
            _isClosed = true;
        }
    }
}