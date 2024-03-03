using _Project.Sсripts.Config;
using _Project.Sсripts.Domain;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.View
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
            _enemyLevel.SetDefault += OnEnemyLevelChanged;

            SetText();
        }

        private void OnEnemyLevelChanged()
        {
            SetText();
        }

        private void SetText()
        {
            string introText = Lean.Localization.LeanLocalization.GetTranslationText(UiTextKeys.EnemyLevelKey);
            _textMeshPro.SetText($"{introText}: {_enemyLevel.Value}");
        }

        private void OnDestroy()
        {
            _enemyLevel.Changed -= OnEnemyLevelChanged;
            _enemyLevel.SetDefault -= OnEnemyLevelChanged;
        }
    }
}