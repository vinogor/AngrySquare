using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sсripts.Hp
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Health _health;

        private void Awake()
        {
            _health.HealthChanged += OnHealthChanged;
        }

        private void OnDestroy()
        {
            _health.HealthChanged -= OnHealthChanged;
        }

        public void Initialize()
        {
            if (_slider == null)
                throw new NullReferenceException("slider cant be null");

            if (_health == null)
                throw new NullReferenceException("health cant be null");

            _slider.interactable = false;
            _slider.wholeNumbers = true;
            _slider.minValue = 0f;
            _slider.maxValue = _health.MaxValue;
            _slider.value = _health.Value;
        }

        private void OnHealthChanged(int newValue)
        {
            _slider.value = newValue;
        }
    }
}