using System.Collections.Generic;
using DG.Tweening;
using Sсripts.Dice;
using Sсripts.Model;
using Sсripts.Model.Effects;
using Sсripts.Scriptable;
using UnityEngine;

namespace Sсripts
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private int _currentCellIndex;
        [SerializeField] private DiceRoller _diceRoller;

        private List<Cell> _cells;
        private Dictionary<EffectName, Effect> _playerEffects;
        private Dictionary<EffectName, Effect> _enemyEffects;

        public void Initialize(List<Cell> cells, Dictionary<EffectName, Effect> playerEffects,
            Dictionary<EffectName, Effect> enemyEffects)
        {
            _cells = cells;
            _playerEffects = playerEffects;
            _enemyEffects = enemyEffects;
        }

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
            Move(amountMoves);
        }

        private void Move(int amountMoves)
        {
            if (amountMoves == 0)
            {
                EffectName effectName = _cells[_currentCellIndex].EffectName;
                _playerEffects[effectName].Activate();
                return;
            }

            _currentCellIndex = ++_currentCellIndex % _cells.Count;
            Cell nextCell = _cells[_currentCellIndex];
            Vector3 nextCellCenter = nextCell.Center();

            transform
                .DOJump(nextCellCenter + Vector3.up * transform.lossyScale.y,
                    Constants.JumpPower, 1, Constants.JumpDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    AnimateCell(nextCell);
                    Move(--amountMoves);
                });
        }

        private void AnimateCell(Cell nextCell)
        {
            nextCell.transform.DOMoveY(nextCell.transform.position.y - 0.1f, 0.2f).SetLoops(2, LoopType.Yoyo);
        }
    }
}