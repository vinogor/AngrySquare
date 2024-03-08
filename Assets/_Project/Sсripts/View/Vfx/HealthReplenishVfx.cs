using Domain;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

namespace View.Vfx
{
    public class HealthReplenishVfx : MonoBehaviour
    {
        [SerializeField] [Required] private ParticleSystem _particleSystem;

        private Health _health;

        public void Initialize(Health health)
        {
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