using System;
using _Project.Sсripts.DamageAndDefence;
using _Project.Sсripts.HealthAndMana;
using _Project.Sсripts.Movement;
using DG.Tweening;

namespace _Project.Sсripts.Model.Effects.Player
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
            sequence.Append(_playerJumper.PlayerJumpOnEnemy());
            sequence.AppendCallback(() => _enemyHealth.TakeDamage(_playerDamage.Value));
            sequence.Append(_playerJumper.PlayerJumpBackToCell());
            sequence.AppendCallback(onComplete.Invoke);
            sequence.Play();
        }
    }
}