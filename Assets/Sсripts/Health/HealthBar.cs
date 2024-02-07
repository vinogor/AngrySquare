using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sсripts
{
    public class HealthBar : MonoBehaviour
    {
        // TODO: когда лучше задавать поле через UI Unity, а когда через конструктор?

        private Slider _slider;
        private Health _health;

        public HealthBar(Slider slider, Health health)
        {
            if (slider == null)
                throw new NullReferenceException("slider cant be null");

            if (health == null)
                throw new NullReferenceException("health cant be null");

            _slider = slider;
            _health = health;

            _slider.interactable = false;
            _slider.wholeNumbers = true;
            _slider.minValue = 0f;
            _slider.maxValue = _health.Value;
            _slider.value = _health.Value;
        }

        private void OnEnable()
        {
            _health.HealthChanged += OnHealthChanged;
        }

        private void OnDisable()
        {
            _health.HealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(int newValue)
        {
            _slider.value = newValue;
        }
    }
}