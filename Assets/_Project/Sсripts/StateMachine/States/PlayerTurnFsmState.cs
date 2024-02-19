using _Project.Sсripts.Dice;
using _Project.Sсripts.HealthAndMana;
using _Project.Sсripts.Movement;
using UnityEngine.Assertions;

namespace _Project.Sсripts.StateMachine.States
{
    public class PlayerTurnFsmState : FsmState
    {
        private readonly DiceRoller _diceRoller;
        private readonly PlayerMovement _playerMovement;
        private readonly Health _enemyHealth;

        public PlayerTurnFsmState(
            FiniteStateMachine finiteStateMachine, DiceRoller diceRoller, PlayerMovement playerMovement, Health enemyHealth)
            : base(finiteStateMachine)
        {
            Assert.IsNotNull(finiteStateMachine);
            Assert.IsNotNull(diceRoller);
            Assert.IsNotNull(playerMovement);
            Assert.IsNotNull(enemyHealth);

            _diceRoller = diceRoller;
            _playerMovement = playerMovement;
            _enemyHealth = enemyHealth;
        }

        public override void Enter()
        {
            base.Enter();
            _diceRoller.MakeAvailable();
            _playerMovement.TurnCompleted += GoToNextState;
        }

        public override void Exit()
        {
            base.Exit();
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