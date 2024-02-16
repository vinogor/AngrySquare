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
        private Coefficients _coefficients;
        private PlayerJumper _playerJumper;

        public void Initialize(List<Cell> cells, Dictionary<EffectName, Effect> playerEffects,
            Coefficients coefficients, PlayerJumper playerJumper)
        {
            Assert.IsNotNull(_diceRoller);
            Assert.IsNotNull(cells);
            Assert.IsNotNull(playerEffects);
            Assert.IsNotNull(coefficients);

            _cells = cells;
            _playerEffects = playerEffects;
            _coefficients = coefficients;
            _playerJumper = playerJumper;
        }

        public event Action TurnCompleted;

        public Cell PlayerStayCell => _cells[_currentCellIndex];

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
                return;
            }

            _currentCellIndex = ++_currentCellIndex % _cells.Count;
            Cell nextCell = _cells[_currentCellIndex];

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_playerJumper.JumpToNextCell(nextCell));
            sequence.AppendCallback(() =>
            {
                AnimateCell(nextCell);
                Move(--amountMoves);
            });
            sequence.Play();
        }

        private void AnimateCell(Cell nextCell)
        {
            nextCell.transform.DOMoveY(nextCell.transform.position.y - 0.1f, _coefficients.AnimationCellDuration)
                .SetLoops(2, LoopType.Yoyo);
        }

        public void SetNewStayCell(Cell newStayCell)
        {
            _currentCellIndex = _cells.IndexOf(newStayCell);
        }
    }
}