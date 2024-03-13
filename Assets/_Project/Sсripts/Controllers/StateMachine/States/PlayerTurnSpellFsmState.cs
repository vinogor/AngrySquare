using UnityEngine.Assertions;

namespace Controllers.StateMachine.States
{
    public class PlayerTurnSpellFsmState : FsmState
    {
        private readonly SpellBarController _spellBarController;
        private readonly TutorialController _tutorialController;

        public PlayerTurnSpellFsmState(FiniteStateMachine finiteStateMachine, SpellBarController spellBarController,
            TutorialController tutorialController) :
            base(finiteStateMachine)
        {
            Assert.IsNotNull(finiteStateMachine);
            Assert.IsNotNull(spellBarController);
            Assert.IsNotNull(tutorialController);

            _spellBarController = spellBarController;
            _tutorialController = tutorialController;
        }

        public override async void Enter()
        {
            base.Enter();

            await _tutorialController.Show(TutorialStep.Intro);
            await _tutorialController.Show(TutorialStep.SpellCast);

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