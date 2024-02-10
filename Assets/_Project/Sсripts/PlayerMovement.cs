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
        private BaseSettings _baseSettings;

        public void Initialize(List<Cell> cells, Dictionary<EffectName, Effect> playerEffects,
            Dictionary<EffectName, Effect> enemyEffects, BaseSettings baseSettings)
        {
            _cells = cells;
            _playerEffects = playerEffects;
            _enemyEffects = enemyEffects;
            _baseSettings = baseSettings;
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