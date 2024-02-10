using _Project.Sсripts.Movement;
using UnityEngine;

namespace _Project.Sсripts.StateMachine.States
{
    public class EnemyTurnFsmState : FsmState
    {
        private readonly EnemyMovement _enemyMovement;

        public EnemyTurnFsmState(FiniteStateMachine finiteStateMachine, EnemyMovement enemyMovement) : base(
            finiteStateMachine)
        {
            _enemyMovement = enemyMovement;
        }

        public override void Enter()
        {
            Debug.Log("EnemyTurnFsmState ENTERED");
            _enemyMovement.Move();
            _enemyMovement.TurnCompleted += GoToNextState;
        }

        public override void Exit()
        {
            Debug.Log("EnemyTurnFsmState EXITED");
            _enemyMovement.TurnCompleted -= GoToNextState;
        }

        private void GoToNextState()
        {
            
            FiniteStateMachine.SetState<PlayerTurnFsmState>();
        }
    }
}