using System;

namespace _Project.Sсripts.UI
{
    public class PopUpWinDefeatController
    {
        private readonly PopUpWinDefeat _popUp;
        
        private bool _IsPlayerWin;

        public PopUpWinDefeatController(PopUpWinDefeat popUp)
        {
            _popUp = popUp;
        }

        public event Action OnSpawnNewEnemySelected;
        public event Action OnRestartGameSelected;

        public void ShowWin()
        {
            _IsPlayerWin = true;
            SetPlayerWinInfo();
            _popUp.Button.onClick.AddListener(Hide);
            _popUp.Show();
        }

        public void ShowDefeat()
        {
            _IsPlayerWin = false;
            SetPlayerLoseInfo();
            _popUp.Button.onClick.AddListener(Hide);
            _popUp.Show();
        }

        public void Hide()
        {
            _popUp.Button.onClick.RemoveListener(Hide);
            _popUp.Hide();

            if (_IsPlayerWin)
                OnSpawnNewEnemySelected?.Invoke();
            else
                OnRestartGameSelected?.Invoke();
        }

        private void SetPlayerLoseInfo()
        {
            _popUp.SetContent("Player Lose", "You lost the game!");
        }

        private void SetPlayerWinInfo()
        {
            _popUp.SetContent("Player Win", "It's time to fight a new opponent!");
        }
    }
}