using _Project.Domain;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.View.Vfx
{
    public class DamageVfx : MonoBehaviour
    {
        [SerializeField] [Required] private ParticleSystem _particleSystem;

        private Health _health;

        public void Initialize(Health health)
        {
            Assert.IsNotNull(health);
            _health = health;
            _health.DamageReceived += OnDamageReceived;
        }

        private void OnDestroy()
        {
            _health.DamageReceived -= OnDamageReceived;
        }

        private void OnDamageReceived()
        {
            _particleSystem.Play();
        }
    }
}