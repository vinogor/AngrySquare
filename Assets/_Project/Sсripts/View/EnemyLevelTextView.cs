using Config;
using Domain;
using Lean.Localization;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace View
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

        private void OnDestroy()
        {
            _enemyLevel.Changed -= OnEnemyLevelChanged;
            LeanLocalization.OnLocalizationChanged -= OnEnemyLevelChanged;
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
    }
}