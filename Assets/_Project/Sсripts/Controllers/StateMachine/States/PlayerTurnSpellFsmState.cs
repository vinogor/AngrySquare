using UnityEngine.Assertions;

namespace Controllers.StateMachine.States
{
    public class PlayerTurnSpellFsmState : FsmState
    {
        private readonly SpellBarController _spellBarController;
        private readonly PopUpTutorialController _popUpTutorialController;

        public PlayerTurnSpellFsmState(FiniteStateMachine finiteStateMachine, SpellBarController spellBarController,
            PopUpTutorialController popUpTutorialController) :
            base(finiteStateMachine)
        {
            Assert.IsNotNull(finiteStateMachine);
            Assert.IsNotNull(spellBarController);
            Assert.IsNotNull(popUpTutorialController);

            _spellBarController = spellBarController;
            _popUpTutorialController = popUpTutorialController;
        }

        public override async void Enter()
        {
            base.Enter();

            await _popUpTutorialController.Show(TutorialStep.Intro);
            await _popUpTutorialController.Show(TutorialStep.SpellCast);

            _spellBarController.Enable(onSpellCompleted: GoToNextState);
        }

        private void GoToNextState()
        {
            _spellBarController.Disable();
            FiniteStateMachine.SetState<PlayerTurnMoveFsmState>();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}