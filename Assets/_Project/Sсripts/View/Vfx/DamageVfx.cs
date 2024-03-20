using Domain;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

namespace View.Vfx
{
    public class DamageVfx : MonoBehaviour
    {
        [SerializeField] [Required] private ParticleSystem _vfxDamageReceived;
        [SerializeField] [Required] private ParticleSystem _vfxDamageBlocked;

        private Health _health;

        public void Initialize(Health health)
        {
            Assert.IsNotNull(health);
            _health = health;
            
            _health.DamageReceived += OnDamageReceived;
            _health.DamageBlocked += OnDamageBlocked;
        }

        private void OnDestroy()
        {
            _health.DamageReceived -= OnDamageReceived;
            _health.DamageBlocked -= OnDamageBlocked;
        }

        private void OnDamageReceived()
        {
            _vfxDamageReceived.Play();
        }

        private void OnDamageBlocked()
        {
            _vfxDamageBlocked.Play();
        }
    }
}