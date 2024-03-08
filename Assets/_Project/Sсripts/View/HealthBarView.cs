using Domain;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace View
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField] [Required] private Slider _slider;
        [SerializeField] [Required] private TextMeshProUGUI _textMeshPro;
        [SerializeField] [Required] private ParticleSystem _vfx;

        private Health _health;

        public void Initialize(Health health)
        {
            Assert.IsNotNull(health);

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
            _vfx.Play();
            _slider.value = newValue;
            _textMeshPro.SetText($"{_slider.value}/{_slider.maxValue}");
        }

        private void OnHealthMaxValueChanged(int newValue)
        {
            _vfx.Play();
            _slider.maxValue = newValue;
            _textMeshPro.SetText($"{_slider.value}/{_slider.maxValue}");
        }
    }
}