using System;
using _Project.View;
using Lean.Localization;
using UnityEngine.Assertions;

namespace _Project.Controllers
{
    public class PopUpNotificationController
    {
        private readonly PopUpNotificationView _popUp;
        private readonly PopUpNotificationModel _model;

        public PopUpNotificationController(PopUpNotificationView popUp, PopUpNotificationModel model)
        {
            Assert.IsNotNull(popUp);
            Assert.IsNotNull(model);

            _popUp = popUp;
            _model = model;
            
            LeanLocalization.OnLocalizationChanged += SetContent;
        }

        public event Action OnClose;

        public void Show()
        {
            SetContent();
            _popUp.Button.onClick.AddListener(Hide);
            _popUp.Show();
        }

        public void Hide()
        {
            _popUp.Button.onClick.RemoveListener(Hide);
            _popUp.Hide();
            LeanLocalization.OnLocalizationChanged -= SetContent;
            OnClose?.Invoke();
        }
        
        private void SetContent()
        {
            string title = LeanLocalization.GetTranslationText(_model.Title);
            string info = LeanLocalization.GetTranslationText(_model.Info);
            _popUp.SetContent(title, info);
        }
    }
}