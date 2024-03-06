using _Project.Config;
using _Project.Domain;
using Lean.Localization;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.View
{
    public class EnemyLevelTextView : MonoBehaviour
    {
        [SerializeField] [Required] private TextMeshProUGUI _textMeshPro;

        private EnemyLevel _enemyLevel;

        public void Initialize(EnemyLevel enemyLevel)
        {
            Assert.IsNotNull(enemyLevel);

            _enemyLevel = enemyLevel;
            _enemyLevel.Changed += OnEnemyLevelChanged;
            LeanLocalization.OnLocalizationChanged += OnEnemyLevelChanged;

            SetText();
        }

        private void OnEnemyLevelChanged()
        {
            SetText();
        }

        private void SetText()
        {
            string introText = LeanLocalization.GetTranslationText(UiTextKeys.EnemyLevelKey);
            _textMeshPro.SetText($"{introText}: {_enemyLevel.Value}");
        }

        private void OnDestroy()
        {
            _enemyLevel.Changed -= OnEnemyLevelChanged;
            LeanLocalization.OnLocalizationChanged -= OnEnemyLevelChanged;
        }
    }
}