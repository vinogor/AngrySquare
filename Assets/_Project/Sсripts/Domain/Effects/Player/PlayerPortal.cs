using System;
using System.Linq;
using DG.Tweening;
using Domain.Movement;
using Services;
using Services.Utility;
using UnityEngine;
using UnityEngine.Assertions;

namespace Domain.Effects.Player
{
    public class PlayerPortal : Effect
    {
        private readonly PlayerJumper _playerJumper;
        private readonly CellsManager _cellsManager;
        private readonly PlayerMovement _playerMovement;

        public PlayerPortal(PlayerJumper playerJumper, CellsManager cellsManager, PlayerMovement playerMovement)
        {
            Assert.IsNotNull(playerJumper);
            Assert.IsNotNull(cellsManager);
            Assert.IsNotNull(playerMovement);

            _playerJumper = playerJumper;
            _cellsManager = cellsManager;
            _playerMovement = playerMovement;
        }

        public event Action Teleporting;

        protected override void Execute(Action onComplete)
        {
            Teleporting?.Invoke();

            Cell currentCell = _playerMovement.PlayerStayCell;

            Cell[] portalCells = _cellsManager.Find(EffectName.Portal);
            Cell targetCell = portalCells.Where(cell => cell != currentCell).ToList().Shuffle().First();

            Debug.Log(
                $"PlayerPortal - Execute - currentCell: index={_cellsManager.Index(currentCell)}, effectName={currentCell.EffectName}");
            Debug.Log(
                $"PlayerPortal - Execute - targetCell: index={_cellsManager.Index(targetCell)}, effectName={targetCell.EffectName}");

            Sequence = DOTween.Sequence()
                .Append(_playerJumper.JumpInPlace())
                .Append(_playerJumper.Teleport(targetCell))
                .AppendCallback(() =>
                {
                    _playerMovement.SetNewStayCell(targetCell);
                    onComplete.Invoke();
                })
                .Play();
        }
    }
}