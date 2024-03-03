using System;
using _Project.Domain.Movement;
using DG.Tweening;

namespace _Project.Domain.Effects.Player
{
    public class PlayerSwords : Effect
    {
        private readonly PlayerJumper _playerJumper;
        private readonly Health _enemyHealth;
        private readonly Damage _playerDamage;

        public PlayerSwords(PlayerJumper playerJumper, Health enemyHealth, Damage playerDamage)
        {
            _playerJumper = playerJumper;
            _enemyHealth = enemyHealth;
            _playerDamage = playerDamage;
        }

        protected override void Execute(Action onComplete)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_playerJumper.JumpOnEnemy());
            sequence.AppendCallback(() => _enemyHealth.TakeDamage(_playerDamage.Value));
            sequence.Append(_playerJumper.JumpBackToCell());
            sequence.AppendCallback(onComplete.Invoke);
            sequence.Play();
        }
    }
}