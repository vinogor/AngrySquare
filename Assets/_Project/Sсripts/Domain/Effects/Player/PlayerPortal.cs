using System;
using System.Linq;
using _Project.Sсripts.Controllers.Sound;
using _Project.Sсripts.Domain.Movement;
using _Project.Sсripts.Services;
using _Project.Sсripts.Services.Utility;
using DG.Tweening;

namespace _Project.Sсripts.Domain.Effects.Player
{
    public class PlayerPortal : Effect
    {
        private readonly PlayerJumper _playerJumper;
        private readonly CellsManager _cellsManager;
        private readonly PlayerMovement _playerMovement;

        public PlayerPortal(PlayerJumper playerJumper, CellsManager cellsManager, PlayerMovement playerMovement)
        {
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