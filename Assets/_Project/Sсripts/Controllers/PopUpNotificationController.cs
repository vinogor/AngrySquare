using System;
using _Project.Sсripts.View;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Controllers
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
        }

        public event Action OnClose;

        public void Show()
        {
            SetInfo();
            _popUp.Button.onClick.AddListener(Hide);
            _popUp.Show();
        }

        public void Hide()
        {
            _popUp.Button.onClick.RemoveListener(Hide);
            _popUp.Hide();

            OnClose?.Invoke();
        }

        private void SetInfo()
        {
            _popUp.SetContent(_model.Title, _model.Info);
        }
    }
}