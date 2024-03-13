using Domain;
using Domain.Movement;
using UnityEngine.Assertions;

namespace Controllers.StateMachine.States
{
    public class EnemyTurnFsmState : FsmState
    {
        private readonly EnemyMovement _enemyMovement;
        private readonly Health _playerHealth;
        private readonly TutorialController _tutorialController;

        public EnemyTurnFsmState(FiniteStateMachine finiteStateMachine, EnemyMovement enemyMovement,
            Health playerHealth, TutorialController tutorialController) : base(
            finiteStateMachine)
        {
            Assert.IsNotNull(finiteStateMachine);
            Assert.IsNotNull(enemyMovement);
            Assert.IsNotNull(playerHealth);
            Assert.IsNotNull(tutorialController);
            _enemyMovement = enemyMovement;
            _playerHealth = playerHealth;
            _tutorialController = tutorialController;
        }

        public override async void Enter()
        {
            base.Enter();

            await _tutorialController.Show(TutorialStep.EnemyTurn);

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
            await _tutorialController.Show(TutorialStep.LastTip);

            if (_playerHealth.IsAlive)
                FiniteStateMachine.SetState<PlayerTurnSpellFsmState>();
            else
                FiniteStateMachine.SetState<PlayerDefeatFsmState>();
        }
    }
}