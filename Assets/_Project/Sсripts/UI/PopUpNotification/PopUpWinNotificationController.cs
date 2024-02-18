using System;

namespace _Project.Sсripts.UI.PopUpNotification
{
    public class PopUpWinNotificationController
    {
        private readonly PopUpNotification _popUp;

        public PopUpWinNotificationController(PopUpNotification popUp)
        {
            _popUp = popUp;
        }

        public event Action OnSpawnNewEnemySelected;

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

            OnSpawnNewEnemySelected?.Invoke();
        }

        private void SetInfo()
        {
            _popUp.SetContent("Player Win", "It's time to fight a new opponent!");
        }
    }
}