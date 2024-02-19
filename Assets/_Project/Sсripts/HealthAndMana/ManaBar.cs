using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace _Project.Sсripts.HealthAndMana
{
    public class ManaBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _textMeshPro;

        private Mana _mana;

        public void Initialize(Mana mana)
        {
            Assert.IsNotNull(mana);
            Assert.IsNotNull(_slider);
            Assert.IsNotNull(_textMeshPro);

            _mana = mana;
            _mana.ValueChanged += OnManaValueChanged;
            _mana.MaxValueChanged += OnManaMaxValueChanged;

            _slider.interactable = false;
            _slider.wholeNumbers = true;
            _slider.minValue = 0f;
            _slider.maxValue = _mana.MaxValue;
            _slider.value = _mana.Value;

            _textMeshPro.SetText($"{_slider.value}/{_slider.maxValue}");
        }

        private void OnDestroy()
        {
            _mana.ValueChanged -= OnManaValueChanged;
        }

        private void OnManaValueChanged(int newValue)
        {
            _slider.value = newValue;
            _textMeshPro.SetText($"{_slider.value}/{_slider.maxValue}");
        }

        private void OnManaMaxValueChanged(int newValue)
        {
            _slider.maxValue = newValue;
            _textMeshPro.SetText($"{_slider.value}/{_slider.maxValue}");
        }
    }
}