using _Project.Sсripts.HealthAndMana;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Animation
{
    public class ManaEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;

        private Mana _mana;

        public void Initialize(Mana mana)
        {
            Assert.IsNotNull(_particleSystem);
            Assert.IsNotNull(mana);
            _mana = mana;
            _mana.Replenished += OnReplenished;
        }

        private void OnDestroy()
        {
            _mana.Replenished -= OnReplenished;
        }

        private void OnReplenished()
        {
            _particleSystem.Play();
        }
    }
}