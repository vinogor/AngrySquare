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
        private const string IntroText = "Enemy level: ";

        public void Initialize(EnemyLevel enemyLevel)
        {
            Assert.IsNotNull(enemyLevel);

            _enemyLevel = enemyLevel;
            _enemyLevel.Increased += OnEnemyLevelChanged;
            _enemyLevel.SetDefault += OnEnemyLevelChanged;

            _textMeshPro.SetText($"{IntroText} {_enemyLevel.Value}");
        }

        private void OnEnemyLevelChanged()
        {
            _textMeshPro.SetText($"{IntroText} {_enemyLevel.Value}");
        }

        private void OnDestroy()
        {
            _enemyLevel.Increased -= OnEnemyLevelChanged;
            _enemyLevel.SetDefault -= OnEnemyLevelChanged;
        }
    }
}