using _Project.Sсripts.Domain;
using _Project.Sсripts.Services.Movement;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Services.StateMachine.States{
    public class EnemyTurnFsmState : FsmState
    {
        private readonly EnemyMovement _enemyMovement;
        private readonly Health _playerHealth;

        public EnemyTurnFsmState(FiniteStateMachine finiteStateMachine, EnemyMovement enemyMovement,
            Health playerHealth) : base(
            finiteStateMachine)
        {
            Assert.IsNotNull(finiteStateMachine);
            Assert.IsNotNull(enemyMovement);
            Assert.IsNotNull(playerHealth);
            _enemyMovement = enemyMovement;
            _playerHealth = playerHealth;
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
            if (_playerHealth.IsAlive)
                FiniteStateMachine.SetState<PlayerTurnSpellFsmState>();
            else
                FiniteStateMachine.SetState<PlayerDefeatFsmState>();
        }
    }
}