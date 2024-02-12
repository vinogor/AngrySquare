using System;
using System.Collections.Generic;
using _Project.Sсripts.Dice;
using _Project.Sсripts.Model;
using _Project.Sсripts.Model.Effects;
using _Project.Sсripts.Scriptable;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private int _currentCellIndex;
        [SerializeField] private DiceRoller _diceRoller;

        private List<Cell> _cells;
        private Dictionary<EffectName, Effect> _playerEffects;
        private BaseSettings _baseSettings;

        public void Initialize(List<Cell> cells, Dictionary<EffectName, Effect> playerEffects,
            BaseSettings baseSettings)
        {
            Assert.IsNotNull(_diceRoller);
            Assert.IsNotNull(cells);
            Assert.IsNotNull(playerEffects);
            Assert.IsNotNull(_baseSettings);

            _cells = cells;
            _playerEffects = playerEffects;
            _baseSettings = baseSettings;
        }

        public event Action TurnCompleted;
        public event Action<Cell> CurrentCell;

        private void Awake()
        {
            _diceRoller.PlayerMoveAmountSet += OnPlayerMoveAmountSet;
        }

        private void OnDestroy()
        {
            _diceRoller.PlayerMoveAmountSet -= OnPlayerMoveAmountSet;
        }

        private void OnPlayerMoveAmountSet(int amountMoves)
        {
            _diceRoller.MakeUnavailable();
            Move(amountMoves);
        }

        private void Move(int amountMoves)
        {
            if (amountMoves == 0)
            {
                EffectName effectName = _cells[_currentCellIndex].EffectName;
                _playerEffects[effectName].Activate(() => TurnCompleted?.Invoke());
                CurrentCell?.Invoke(_cells[_currentCellIndex]);
                return;
            }

            _currentCellIndex = ++_currentCellIndex % _cells.Count;
            Cell nextCell = _cells[_currentCellIndex];
            Vector3 nextCellCenter = nextCell.Center();

            transform
                .DOJump(nextCellCenter + Vector3.up * transform.lossyScale.y,
                    _baseSettings.JumpPower, 1, _baseSettings.JumpDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    AnimateCell(nextCell);
                    Move(--amountMoves);
                });
        }

        private void AnimateCell(Cell nextCell)
        {
            nextCell.transform.DOMoveY(nextCell.transform.position.y - 0.1f, _baseSettings.AnimationCellDuration)
                .SetLoops(2, LoopType.Yoyo);
        }
    }
}