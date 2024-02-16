using _Project.Sсripts.UI;
using UnityEngine.Assertions;

namespace _Project.Sсripts.StateMachine.States
{
    public class EnemyDefeatFsmState : FsmState
    {
        private readonly PopUpWinDefeatController _popUpController;

        public EnemyDefeatFsmState(FiniteStateMachine finiteStateMachine, PopUpWinDefeatController popUpController) : base(finiteStateMachine)
        {
            Assert.IsNotNull(popUpController);
            _popUpController = popUpController;
        }

        public override void Enter()
        {
            base.Enter();

            _popUpController.ShowWin();
        }

        public override void Exit()
        {
            base.Exit();
            _popUpController.Hide();
        }
    }
}