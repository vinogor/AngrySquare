using System;
using System.Linq;
using _Project.Sсripts.Controllers.Sound;
using _Project.Sсripts.Domain.Movement;
using _Project.Sсripts.Services.Utility;
using DG.Tweening;

namespace _Project.Sсripts.Domain.Effects.Player
{
    public class PlayerPortal : Effect
    {
        private readonly PlayerJumper _playerJumper;
        private readonly Cell[] _portalCells;
        private readonly PlayerMovement _playerMovement;
        private readonly GameSounds _gameSounds;

        public PlayerPortal(PlayerJumper playerJumper, Cell[] portalCells, PlayerMovement playerMovement,
            GameSounds gameSounds)
        {
            _playerJumper = playerJumper;
            _portalCells = portalCells;
            _playerMovement = playerMovement;
            _gameSounds = gameSounds;
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