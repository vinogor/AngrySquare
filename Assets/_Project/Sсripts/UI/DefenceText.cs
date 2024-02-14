using _Project.Sсripts.DamageAndDefence;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.UI
{
    public class DefenceText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshPro;

        private Defence _defence;

        public void Initialize(Defence defence)
        {
            Assert.IsNotNull(defence);
            Assert.IsNotNull(_textMeshPro);

            _defence = defence;
            _defence.Changed += OnDefenceChanged;

            _textMeshPro.SetText($"{_defence.Value}");
        }

        private void OnDefenceChanged()
        {
            _textMeshPro.SetText($"{_defence.Value}");
        }

        private void OnDestroy()
        {
            _defence.Changed -= OnDefenceChanged;
        }
    }
}