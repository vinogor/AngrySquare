using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sсripts.Hp
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;

        private Health _health;

        public void Initialize(Health health)
        {
            _health = health ?? throw new NullReferenceException("health cant be null");
            _health.Changed += OnHealthChanged;

            if (_slider == null)
                throw new NullReferenceException("slider cant be null");

            _slider.interactable = false;
            _slider.wholeNumbers = true;
            _slider.minValue = 0f;
            _slider.maxValue = _health.MaxValue;
            _slider.value = _health.Value;
        }

        private void OnDestroy()
        {
            _health.Changed -= OnHealthChanged;
        }

        private void OnHealthChanged(int newValue)
        {
            _slider.value = newValue;
        }
    }
}