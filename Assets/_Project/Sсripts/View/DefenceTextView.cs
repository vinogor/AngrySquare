using _Project.Sсripts.Domain;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.View
{
    public class DefenceTextView : MonoBehaviour
    {
        [SerializeField] [Required] private TextMeshProUGUI _textMeshPro;
        [SerializeField] [Required] private ParticleSystem _vfx;

        private Defence _defence;

        public void Initialize(Defence defence)
        {
            Assert.IsNotNull(defence);

            _defence = defence;
            _defence.Changed += OnDefenceChanged;

            _textMeshPro.SetText($"{_defence.Value}");
        }

        private void OnDestroy()
        {
            _defence.Changed -= OnDefenceChanged;
        }

        private void OnDefenceChanged()
        {
            _vfx.Play();
            _textMeshPro.SetText($"{_defence.Value}");
        }
    }
}