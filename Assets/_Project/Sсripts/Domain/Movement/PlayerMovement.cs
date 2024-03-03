using System;
using System.Collections.Generic;
using _Project.Config;
using _Project.Domain.Effects;
using _Project.Services;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Domain.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private DiceRoller _diceRoller;

        private CellsManager _cellsManager;
        private Dictionary<EffectName, Effect> _playerEffects;
        private Coefficients _coefficients;
        private PlayerJumper _playerJumper;

        int startingPlayersCellIndex = 0;

        public void Initialize(CellsManager cellsManager, Dictionary<EffectName, Effect> playerEffects,
            Coefficients coefficients, PlayerJumper playerJumper)
        {
            Assert.IsNotNull(_diceRoller);
            Assert.IsNotNull(cellsManager);
            Assert.IsNotNull(playerEffects);
            Assert.IsNotNull(coefficients);

            _cellsManager = cellsManager;
            _playerEffects = playerEffects;
            _coefficients = coefficients;
            _playerJumper = playerJumper;
            PlayersCellIndex = startingPlayersCellIndex;
        }

        public event Action TurnCompleted;

        public int PlayersCellIndex { get; private set; }
        public Cell PlayerStayCell => _cellsManager.Get(PlayersCellIndex);

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
                EffectName effectName = _cellsManager.Get(PlayersCellIndex).EffectName;
                Debug.Log($"Cell with index {PlayersCellIndex}, Activating effect: {effectName}");
                ActivateEffect(effectName);
                return;
            }

            PlayersCellIndex = ++PlayersCellIndex % _cellsManager.Length;
            Cell nextCell = _cellsManager.Get(PlayersCellIndex);

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_playerJumper.JumpToNextCell(nextCell));
            sequence.AppendCallback(() =>
            {
                AnimateCell(nextCell);
                Move(--amountMoves);
            });
            sequence.Play();
        }

        public void ActivateEffect(EffectName effectName)
        {
            _playerEffects[effectName].Activate(() => TurnCompleted?.Invoke());
        }

        private void AnimateCell(Cell nextCell)
        {
            nextCell.transform.DOMoveY(nextCell.transform.position.y - 0.1f, _coefficients.AnimationCellDuration)
                .SetLoops(2, LoopType.Yoyo);
        }

        public void SetNewStayCell(Cell newStayCell)
        {
            PlayersCellIndex = _cellsManager.Index(newStayCell);
        }
        
        public void SetNewStayCell(int newStayCellIndex)
        {
            PlayersCellIndex = newStayCellIndex;
            _playerJumper.JumpToNextCell(PlayerStayCell, true);
        }

        public void SetDefaultStayCell()
        {
            PlayersCellIndex = startingPlayersCellIndex;
            _playerJumper.JumpToNextCell(PlayerStayCell, true);
        }
    }
}