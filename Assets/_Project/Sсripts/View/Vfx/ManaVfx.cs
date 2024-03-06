using _Project.Domain;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.View.Vfx
{
    public class ManaVfx : MonoBehaviour
    {
        [SerializeField] [Required] private ParticleSystem _particleSystem;

        private Mana _mana;

        public void Initialize(Mana mana)
        {
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