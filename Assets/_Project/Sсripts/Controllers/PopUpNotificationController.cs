using System;
using Lean.Localization;
using Services;
using UnityEngine.Assertions;
using View;

namespace Controllers
{
    public class PopUpNotificationController : IPresenter
    {
        private readonly PopUpNotificationView _popUp;
        private readonly PopUpNotificationModel _model;

        private Action _onClose;

        public PopUpNotificationController(PopUpNotificationView popUp, PopUpNotificationModel model)
        {
            Assert.IsNotNull(popUp);
            Assert.IsNotNull(model);

            _popUp = popUp;
            _model = model;
            
            Hide();
        }

        public void Enable()
        {
            LeanLocalization.OnLocalizationChanged += SetContent;
        }

        public void Disable()
        {
            LeanLocalization.OnLocalizationChanged -= SetContent;
        }

        public void Show(Action onClose)
        {
            _onClose = onClose;
            SetContent();
            _popUp.Clicked += Hide;
            _popUp.Show();
        }

        private void Hide()
        {
            _popUp.Clicked -= Hide;
            _popUp.Hide();
            _onClose?.Invoke();
        }

        private void SetContent()
        {
            string title = LeanLocalization.GetTranslationText(_model.Title);
            string info = LeanLocalization.GetTranslationText(_model.Info);
            _popUp.SetContent(title, info);
        }
    }
}