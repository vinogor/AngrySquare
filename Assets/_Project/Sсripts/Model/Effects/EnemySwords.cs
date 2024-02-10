using System;
using _Project.Sсripts.Dmg;
using _Project.Sсripts.Hp;
using _Project.Sсripts.Movement;
using _Project.Sсripts.Scriptable;
using DG.Tweening;
using UnityEngine;

namespace _Project.Sсripts.Model.Effects
{
    public class EnemySwords : Effect
    {
        private Transform _enemyTransform;
        private Cell _targetCell;
        private Health _playerHealth;
        private Damage _enemyDamage;
        private PlayerMovement _playerMovement;
        private BaseSettings _baseSettings;

        private Cell _playerCell;

        public EnemySwords(Transform enemyTransform, Cell targetCell, Health playerHealth,
            Damage enemyDamage, PlayerMovement playerMovement, BaseSettings baseSettings)
        {
            _enemyTransform = enemyTransform;
            _targetCell = targetCell;
            _playerHealth = playerHealth;
            _enemyDamage = enemyDamage;
            _playerMovement = playerMovement;
            _baseSettings = baseSettings;
        }

        public void Initialize()
        {
            // TODO: надо как то отписываться? в какой момент лучше? когда кончится стейт хода игрока? 
            //       а подписываться когда начнётся ход игрока? 
            _playerMovement.CurrentCell += OnPlayerCurrentCell;
        }

        private void OnPlayerCurrentCell(Cell playerCell)
        {
            _playerCell = playerCell;
        }

        public override void Activate(Action callNextTurn)
        {
            Debug.Log("Effect - Swords - Activate");

            Vector3 startEnemyPosition = _enemyTransform.position;

            _enemyTransform
                .DOJump(_targetCell.Center() + Vector3.up * _baseSettings.EnemyHeight,
                    _baseSettings.JumpPower, 1, _baseSettings.JumpDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    int enemyDamage = _enemyDamage.Value;

                    if (_targetCell == _playerCell)
                    {
                        // TODO: тройной урон - как то выделить анимацией по особенному
                        _playerHealth.TakeDamage(enemyDamage * 3);
                        JumpToBase(callNextTurn, startEnemyPosition);
                    }
                    else
                    {
                        JumpOnPlayer(callNextTurn, enemyDamage, startEnemyPosition);
                    }
                });
        }

        private void JumpOnPlayer(Action callNextTurn, int enemyDamage, Vector3 startEnemyPosition)
        {
            _enemyTransform
                .DOJump(_playerCell.Center() + Vector3.up * _baseSettings.EnemyHeight,
                    _baseSettings.JumpPower, 1, _baseSettings.JumpDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _playerHealth.TakeDamage(enemyDamage);
                    JumpToBase(callNextTurn, startEnemyPosition);
                });
        }

        private void JumpToBase(Action callNextTurn, Vector3 startEnemyPosition)
        {
            _enemyTransform
                .DOJump(startEnemyPosition, _baseSettings.JumpPower, 1, _baseSettings.JumpDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    // задать новую target !!

                    callNextTurn.Invoke();
                });
        }
    }
}