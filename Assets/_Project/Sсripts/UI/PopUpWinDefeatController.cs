using _Project.Sсripts.StateMachine;

namespace _Project.Sсripts.UI
{
    public class PopUpWinDefeatController
    {
        private readonly PopUpWinDefeat _popUp;
        private readonly FiniteStateMachine _stateMachine;

        public PopUpWinDefeatController(PopUpWinDefeat popUp, FiniteStateMachine stateMachine)
        {
            _popUp = popUp;
            _stateMachine = stateMachine;
        }

        public void ShowWin()
        {
            SetPlayerWinInfo();
            _popUp.Button.onClick.AddListener(Hide);
            _popUp.Show();
        }

        public void ShowDefeat()
        {
            SetPlayerLoseInfo();
            _popUp.Button.onClick.AddListener(Hide);
            _popUp.Show();
        }

        public void Hide()
        {
            _popUp.Hide();
            // TODO: set ne state
            // _stateMachine.SetState<...>();
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