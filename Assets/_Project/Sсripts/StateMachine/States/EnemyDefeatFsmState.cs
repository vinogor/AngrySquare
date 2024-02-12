using _Project.Sсripts.UI;
using UnityEngine.Assertions;

namespace _Project.Sсripts.StateMachine.States
{
    public class EnemyDefeatFsmState : FsmState
    {
        private PopUp _popUp;

        public EnemyDefeatFsmState(FiniteStateMachine finiteStateMachine, PopUp popUp) : base(finiteStateMachine)
        {
            Assert.IsNotNull(popUp);
            _popUp = popUp;
        }

        public override void Enter()
        {
            base.Enter();

            _popUp.SetPlayerWinInfo();
            _popUp.SetActive();

            // TODO: по клику - закрыть попап 
        }

        public override void Exit()
        {
            base.Exit();
            _popUp.SetNotActive();
        }
    }
}