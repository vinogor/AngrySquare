using _Project.Sсripts.Domain;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Controllers.Vfx{
    public class ManaVfx : MonoBehaviour
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