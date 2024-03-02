using _Project.Sсripts.Domain;
using _Project.Sсripts.Domain.Movement;
using _Project.Sсripts.Services;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Controllers.StateMachine.States
{
    public class PlayerTurnMoveFsmState : FsmState
    {
        private readonly DiceRoller _diceRoller;
        private readonly PlayerMovement _playerMovement;
        private readonly Health _enemyHealth;
        private readonly PopUpTutorialController _popUpTutorialController;

        public PlayerTurnMoveFsmState(FiniteStateMachine finiteStateMachine, DiceRoller diceRoller,
            PlayerMovement playerMovement, Health enemyHealth, PopUpTutorialController popUpTutorialController)
            : base(finiteStateMachine)
        {
            Assert.IsNotNull(finiteStateMachine);
            Assert.IsNotNull(diceRoller);
            Assert.IsNotNull(playerMovement);
            Assert.IsNotNull(enemyHealth);
            Assert.IsNotNull(popUpTutorialController);

            _diceRoller = diceRoller;
            _playerMovement = playerMovement;
            _enemyHealth = enemyHealth;
            _popUpTutorialController = popUpTutorialController;
        }

        public override async void Enter()
        {
            base.Enter();
            
            await _popUpTutorialController.Show(TutorialStep.RollDice);
            
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