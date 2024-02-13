using _Project.Sсripts.Hp;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Animation
{
    public class HealthReplenishEffect: MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        
        private Health _health;
        
        public void Initialize(Health health)
        {
            Assert.IsNotNull(_particleSystem);
            Assert.IsNotNull(health);
            _health = health;
            _health.HealthReplenished += OnHealthReplenished;
        }

        private void OnDestroy()
        {
            _health.HealthReplenished -= OnHealthReplenished;
        }

        private void OnHealthReplenished()
        {
            _particleSystem.Play();
        }
    }
}