using _Project.Sсripts.Dice;
using _Project.Sсripts.Movement;
using UnityEngine.Assertions;

namespace _Project.Sсripts.StateMachine.States
{
    public class PlayerTurnFsmState : FsmState
    {
        private readonly DiceRoller _diceRoller;
        private readonly PlayerMovement _playerMovement;

        public PlayerTurnFsmState(
            FiniteStateMachine finiteStateMachine, DiceRoller diceRoller, PlayerMovement playerMovement)
            : base(finiteStateMachine)
        {
            Assert.IsNotNull(finiteStateMachine);
            Assert.IsNotNull(diceRoller);
            Assert.IsNotNull(playerMovement);

            _diceRoller = diceRoller;
            _playerMovement = playerMovement;
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
            FiniteStateMachine.SetState<EnemyTurnFsmState>();
        }
    }
}