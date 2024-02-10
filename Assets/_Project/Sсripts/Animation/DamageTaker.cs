using Sсripts.Hp;
using UnityEngine;

namespace Sсripts.Animation
{
    public class DamageTaker : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _damageParticleSystem;
        
        private Health _health;

        public void Initialize(Health health )
        {
            _health = health;
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