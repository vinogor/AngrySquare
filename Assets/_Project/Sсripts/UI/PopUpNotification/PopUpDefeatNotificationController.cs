using System;

namespace _Project.Sсripts.UI.PopUpNotification
{
    // TODO: вынести в абстрактный класс общее 

    public class PopUpDefeatNotificationController
    {
        private readonly PopUpNotification _popUp;

        public PopUpDefeatNotificationController(PopUpNotification popUp)
        {
            _popUp = popUp;
        }

        public event Action OnRestartGameSelected;

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

            OnRestartGameSelected?.Invoke();
        }

        private void SetInfo()
        {
            _popUp.SetContent("Player Lose", "You lost the game!");
        }
    }
}