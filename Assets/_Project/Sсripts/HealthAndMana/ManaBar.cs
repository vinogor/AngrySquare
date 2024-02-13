using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace _Project.Sсripts.HealthAndMana
{
    public class ManaBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;

        private Mana _mana;

        public void Initialize(Mana mana)
        {
            Assert.IsNotNull(mana);
            Assert.IsNotNull(_slider);

            _mana = mana; 
            _mana.Changed += OnManaChanged;

            _slider.interactable = false;
            _slider.wholeNumbers = true;
            _slider.minValue = 0f;
            _slider.maxValue = _mana.MaxValue;
            _slider.value = _mana.Value;
        }

        private void OnDestroy()
        {
            _mana.Changed -= OnManaChanged;
        }

        private void OnManaChanged(int newValue)
        {
            _slider.value = newValue;
        }
    }
}