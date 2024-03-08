using System;
using System.Collections.Generic;
using Config;
using DG.Tweening;
using Domain.Effects;
using Services;
using UnityEngine;
using UnityEngine.Assertions;

namespace Domain.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private DiceRoller _diceRoller;

        private CellsManager _cellsManager;
        private Dictionary<EffectName, Effect> _playerEffects;
        private Coefficients _coefficients;
        private PlayerJumper _playerJumper;

        private readonly int _startingPlayersCellIndex = 0;
        private readonly float _cellLoweringDepth = 0.1f;
        public void Initialize(CellsManager cellsManager, Dictionary<EffectName, Effect> playerEffects,
            Coefficients coefficients, PlayerJumper playerJumper)
        {
            Assert.IsNotNull(_diceRoller);
            Assert.IsNotNull(cellsManager);
            Assert.IsNotNull(playerEffects);
            Assert.IsNotNull(coefficients);
            Assert.IsNotNull(playerJumper);

            _cellsManager = cellsManager;
            _playerEffects = playerEffects;
            _coefficients = coefficients;
            _playerJumper = playerJumper;
            PlayersCellIndex = _startingPlayersCellIndex;
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
            nextCell.transform.DOMoveY(nextCell.transform.position.y - _cellLoweringDepth, _coefficients.AnimationCellDuration)
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
            PlayersCellIndex = _startingPlayersCellIndex;
            _playerJumper.JumpToNextCell(PlayerStayCell, true);
        }
    }
}