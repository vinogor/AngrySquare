using Domain;
using Domain.Movement;
using Services;
using UnityEngine.Assertions;

namespace Controllers.StateMachine.States
{
    public class PlayerTurnMoveFsmState : FsmState
    {
        private readonly DiceRoller _diceRoller;
        private readonly PlayerMovement _playerMovement;
        private readonly Health _enemyHealth;
        private readonly TutorialController _tutorialController;

        public PlayerTurnMoveFsmState(FiniteStateMachine finiteStateMachine, DiceRoller diceRoller,
            PlayerMovement playerMovement, Health enemyHealth, TutorialController tutorialController)
            : base(finiteStateMachine)
        {
            Assert.IsNotNull(finiteStateMachine);
            Assert.IsNotNull(diceRoller);
            Assert.IsNotNull(playerMovement);
            Assert.IsNotNull(enemyHealth);
            Assert.IsNotNull(tutorialController);

            _diceRoller = diceRoller;
            _playerMovement = playerMovement;
            _enemyHealth = enemyHealth;
            _tutorialController = tutorialController;
        }

        public override async void Enter()
        {
            base.Enter();
            
            await _tutorialController.Show(TutorialStep.RollDice);
            
            _diceRoller.MakeAvailable();
            _playerMovement.TurnCompleted += GoToNextState;
        }

        public override void Exit()
        {
            base.Exit();
            _diceRoller.MakeUnavailable();
            _playerMovement.TurnCompleted -= GoToNextState;
        }

        private void GoToNextState()
        {
            if (_enemyHealth.IsAlive)
                FiniteStateMachine.SetState<EnemyTurnFsmState>();
            else
                FiniteStateMachine.SetState<PlayerWinFsmState>();
        }
    }
}