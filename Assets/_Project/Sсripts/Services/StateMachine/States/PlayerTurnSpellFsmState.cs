using _Project.Sсripts.UI.SpellCast;

namespace _Project.Sсripts{
    public class PlayerTurnSpellFsmState : FsmState
    {
        private readonly SpellBarController _spellBarController;

        public PlayerTurnSpellFsmState(FiniteStateMachine finiteStateMachine, SpellBarController spellBarController) :
            base(finiteStateMachine)
        {
            _spellBarController = spellBarController;
        }

        public override void Enter()
        {
            base.Enter();
            _spellBarController.SpellCompleted += GoToNextState;

            // TODO: добавить эффект активации 
            _spellBarController.Enable(); 
        }

        private void GoToNextState()
        {
            _spellBarController.Disable();
            FiniteStateMachine.SetState<PlayerTurnMoveFsmState>();
        }

        public override void Exit()
        {
            base.Exit();
            _spellBarController.SpellCompleted -= GoToNextState;
        }
    }
}