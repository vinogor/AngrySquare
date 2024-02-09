using Sсripts.Hp;
using UnityEngine;

namespace Sсripts.Animation
{
    public class DamageTaker : MonoBehaviour
    {
        private Health _health;
        private ParticleSystem _damageParticleSystem;

        public void Initialize(Health health, ParticleSystem damageParticleSystem)
        {
            _health = health;
            _damageParticleSystem = damageParticleSystem;
            _health.DamageReceived += OnDamageReceived;
        }

        private void OnDestroy()
        {
            _health.DamageReceived -= OnDamageReceived;
        }

        private void OnDamageReceived()
        {
            _damageParticleSystem.Play();
        }
    }
}