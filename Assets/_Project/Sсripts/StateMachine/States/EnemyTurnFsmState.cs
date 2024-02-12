using _Project.Sсripts.Movement;

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
            base.Enter();
            _enemyMovement.Move();
            _enemyMovement.TurnCompleted += GoToNextState;
        }

        public override void Exit()
        {
            base.Exit();
            _enemyMovement.TurnCompleted -= GoToNextState;
        }

        private void GoToNextState()
        {
            FiniteStateMachine.SetState<PlayerTurnFsmState>();
        }
    }
}