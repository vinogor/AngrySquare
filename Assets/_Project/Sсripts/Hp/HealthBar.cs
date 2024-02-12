using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace _Project.Sсripts.Hp
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;

        private Health _health;

        public void Initialize(Health health)
        {
            Assert.IsNotNull(health);
            Assert.IsNotNull(_slider);

            _health = health; 
            _health.Changed += OnHealthChanged;

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