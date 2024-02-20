using System;
using System.Linq;
using _Project.Sсripts.Domain;
using _Project.Sсripts.Services.Movement;
using _Project.Sсripts.Services.Utility;
using DG.Tweening;

namespace _Project.Sсripts.Services.Effects.Player{
    public class PlayerPortal : Effect
    {
        private readonly PlayerJumper _playerJumper;
        private readonly Cell[] _portalCells;
        private readonly PlayerMovement _playerMovement;

        public PlayerPortal(PlayerJumper playerJumper, Cell[] portalCells, PlayerMovement playerMovement)
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
            sequence.Append(_playerJumper.JumpInPlace());
            sequence.Append(_playerJumper.Teleport(targetCell));
            sequence.AppendCallback(() =>
            {
                _playerMovement.SetNewStayCell(targetCell);
                onComplete.Invoke();
            });
            sequence.Play();
        }
    }
}