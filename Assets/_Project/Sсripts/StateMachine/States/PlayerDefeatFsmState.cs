using _Project.Sсripts.UI;
using UnityEngine.Assertions;

namespace _Project.Sсripts.StateMachine.States
{
    public class PlayerDefeatFsmState : FsmState
    {
        private PopUpWinDefeat _popUpWinDefeat;

        public PlayerDefeatFsmState(FiniteStateMachine finiteStateMachine, PopUpWinDefeat popUpWinDefeat) : base(finiteStateMachine)
        {
            Assert.IsNotNull(popUpWinDefeat);
            _popUpWinDefeat = popUpWinDefeat;
        }

        public override void Enter()
        {
            base.Enter();

            _popUpWinDefeat.SetPlayerLoseInfo();
            _popUpWinDefeat.SetActive();
            
            // TODO: по клику - закрыть попап 
        }

        public override void Exit()
        {
            base.Exit();
            _popUpWinDefeat.SetNotActive();
        }
    }
}