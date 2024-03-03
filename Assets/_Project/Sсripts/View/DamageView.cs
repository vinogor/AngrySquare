using _Project.Domain;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.View
{
    public class DamageView : MonoBehaviour
    {
        [SerializeField] [Required] private TextMeshProUGUI _textMeshPro;
        [SerializeField] [Required] private ParticleSystem _vfx;

        private Damage _damage;

        public void Initialize(Damage damage)
        {
            Assert.IsNotNull(damage);

            _damage = damage;
            _damage.Changed += OnDamageChanged;

            _textMeshPro.SetText($"{_damage.Value}");
        }

        private void OnDestroy()
        {
            _damage.Changed -= OnDamageChanged;
        }

        private void OnDamageChanged()
        {
            _vfx.Play();
            _textMeshPro.SetText($"{_damage.Value}");
        }
    }
}