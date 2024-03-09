using Domain;
using Domain.Movement;
using UnityEngine.Assertions;

namespace Controllers.StateMachine.States
{
    public class EnemyTurnFsmState : FsmState
    {
        private readonly EnemyMovement _enemyMovement;
        private readonly Health _playerHealth;
        private readonly PopUpTutorialController _popUpTutorialController;

        public EnemyTurnFsmState(FiniteStateMachine finiteStateMachine, EnemyMovement enemyMovement,
            Health playerHealth, PopUpTutorialController popUpTutorialController) : base(
            finiteStateMachine)
        {
            Assert.IsNotNull(finiteStateMachine);
            Assert.IsNotNull(enemyMovement);
            Assert.IsNotNull(playerHealth);
            Assert.IsNotNull(popUpTutorialController);
            _enemyMovement = enemyMovement;
            _playerHealth = playerHealth;
            _popUpTutorialController = popUpTutorialController;
        }

        public override async void Enter()
        {
            base.Enter();

            await _popUpTutorialController.Show(TutorialStep.EnemyTurn);

            _enemyMovement.Move();
            _enemyMovement.TurnCompleted += GoToNextState;
        }

        public override void Exit()
        {
            base.Exit();
            _enemyMovement.TurnCompleted -= GoToNextState;
        }

        private async void GoToNextState()
        {
            await _popUpTutorialController.Show(TutorialStep.LastTip);

            if (_playerHealth.IsAlive)
                FiniteStateMachine.SetState<PlayerTurnSpellFsmState>();
            else
                FiniteStateMachine.SetState<PlayerDefeatFsmState>();
        }
    }
}