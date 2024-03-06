using _Project.Domain.Effects.Player;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.View.Vfx
{
    public class TeleportVfx : MonoBehaviour
    {
        [SerializeField] [Required] private ParticleSystem _particleSystem;

        private PlayerPortal _playerPortal;

        public void Initialize(PlayerPortal playerPortal)
        {
            Assert.IsNotNull(playerPortal);
            _playerPortal = playerPortal;
            _playerPortal.Teleporting += OnTeleporting;
        }

        private void OnTeleporting()
        {
            _particleSystem.Play();
        }

        private void OnDestroy()
        {
            _playerPortal.Teleporting -= OnTeleporting;
        }
    }
}