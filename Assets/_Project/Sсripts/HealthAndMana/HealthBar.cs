using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace _Project.Sсripts.HealthAndMana
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _textMeshPro;

        private Health _health;

        public void Initialize(Health health)
        {
            Assert.IsNotNull(health);
            Assert.IsNotNull(_slider);
            Assert.IsNotNull(_textMeshPro);

            _health = health; 
            _health.ValueChanged += OnHealthValueChanged;
            _health.MaxValueChanged += OnHealthMaxValueChanged;

            _slider.interactable = false;
            _slider.wholeNumbers = true;
            _slider.minValue = 0f;
            _slider.maxValue = _health.MaxValue;
            _slider.value = _health.Value;
            
            _textMeshPro.SetText($"{_slider.value}/{_slider.maxValue}");
        }

        private void OnDestroy()
        {
            _health.ValueChanged -= OnHealthValueChanged;
        }

        private void OnHealthValueChanged(int newValue)
        {
            _slider.value = newValue;
            _textMeshPro.SetText($"{_slider.value}/{_slider.maxValue}");
        }

        private void OnHealthMaxValueChanged(int newValue)
        {
            _slider.maxValue = newValue;
            _textMeshPro.SetText($"{_slider.value}/{_slider.maxValue}");
        }
    }
}