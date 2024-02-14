using _Project.Sсripts.DamageAndDefence;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.UI
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshPro;

        private Damage _damage;

        public void Initialize(Damage damage)
        {
            Assert.IsNotNull(damage);
            Assert.IsNotNull(_textMeshPro);

            _damage = damage;
            _damage.Changed += OnDamageChanged;

            _textMeshPro.SetText($"{_damage.Value}");
        }

        private void OnDamageChanged()
        {
            _textMeshPro.SetText($"{_damage.Value}");
        }

        private void OnDestroy()
        {
            _damage.Changed -= OnDamageChanged;
        }
    }
}