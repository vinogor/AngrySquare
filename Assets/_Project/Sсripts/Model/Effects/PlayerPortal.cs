using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Sсripts.Movement;
using _Project.Sсripts.Utility;
using DG.Tweening;

namespace _Project.Sсripts.Model.Effects
{
    public class PlayerPortal : Effect
    {
        private readonly PlayerJumper _playerJumper;
        private readonly List<Cell> _portalCells;
        private readonly PlayerMovement _playerMovement;

        public PlayerPortal(PlayerJumper playerJumper, List<Cell> portalCells, PlayerMovement playerMovement)
        {
            _playerJumper = playerJumper;
            _portalCells = portalCells;
            _playerMovement = playerMovement;
        }

        public event Action Teleporting;

        protected override void Execute(Action onComplete)
        {
            Teleporting?.Invoke();
            
            Cell currentCell = _playerMovement.PlayerStayCell;
            Cell targetCell = _portalCells.Where(cell => cell != currentCell).ToList().Shuffle().First();

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_playerJumper.PlayerJumpInPlace());
            sequence.Append(_playerJumper.PlayerTeleport(targetCell));
            sequence.AppendCallback(() =>
            {
                _playerMovement.SetNewStayCell(targetCell);
                onComplete.Invoke();
            });
            sequence.Play();
        }
    }
}