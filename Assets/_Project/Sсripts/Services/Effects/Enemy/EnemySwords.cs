using System;
using _Project.Sсripts.Domain;
using _Project.Sсripts.Services.Movement;
using DG.Tweening;

namespace _Project.Sсripts.Services.Effects.Enemy{
    public class EnemySwords : Effect
    {
        private readonly EnemyJumper _enemyJumper;
        private readonly Health _playerHealth;
        private readonly Damage _enemyDamage;

        public EnemySwords(EnemyJumper enemyJumper, Health playerHealth, Damage enemyDamage)
        {
            _enemyJumper = enemyJumper;
            _playerHealth = playerHealth;
            _enemyDamage = enemyDamage;
        }

        protected override void Execute(Action onComplete)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_enemyJumper.JumpToTargetCell());
            sequence.Append(_enemyJumper.JumpOnPlayer());
            sequence.AppendCallback(() => _playerHealth.TakeDamage(_enemyDamage.Value));
            sequence.Append(_enemyJumper.JumpBackToBase());
            sequence.AppendCallback(onComplete.Invoke);
            sequence.Play();
        }
    }
}