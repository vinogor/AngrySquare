using _Project.Sсripts.Model.Effects;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Animation
{
    public class TeleportEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;

        private PlayerPortal _playerPortal;

        public void Initialize(PlayerPortal playerPortal)
        {
            Assert.IsNotNull(_particleSystem);
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