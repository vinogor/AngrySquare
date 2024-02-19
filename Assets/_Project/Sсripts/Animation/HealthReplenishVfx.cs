using _Project.Sсripts.HealthAndMana;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Animation
{
    public class HealthReplenishVfx : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;

        private Health _health;

        public void Initialize(Health health)
        {
            Assert.IsNotNull(_particleSystem);
            Assert.IsNotNull(health);
            _health = health;
            _health.Replenished += OnReplenished;
        }

        private void OnDestroy()
        {
            _health.Replenished -= OnReplenished;
        }

        private void OnReplenished()
        {
            _particleSystem.Play();
        }
    }
}