using _Project.Sсripts.Dice;
using _Project.Sсripts.Movement;
using UnityEngine;

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
            _diceRoller = diceRoller;
            _playerMovement = playerMovement;
        }

        public override void Enter()
        {
            Debug.Log("PlayerTurnFsmState ENTERED");
            _diceRoller.MakeAvailable();
            _playerMovement.TurnCompleted += GoToNextState;
        }

        public override void Exit()
        {
            Debug.Log("PlayerTurnFsmState EXITED");
            _playerMovement.TurnCompleted -= GoToNextState;
        }

        private void GoToNextState()
        {
            FiniteStateMachine.SetState<EnemyTurnFsmState>();
        }
    }
}