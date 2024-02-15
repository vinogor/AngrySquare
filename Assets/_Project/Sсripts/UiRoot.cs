using _Project.Sсripts.DamageAndDefence;
using _Project.Sсripts.HealthAndMana;
using _Project.Sсripts.UI;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts
{
    public class UiRoot : MonoBehaviour
    {
        // [Header("Common")]
        // [Required]
        // [ValidateInput("IsNotNull")]
        [field: SerializeField] public PopUpWinDefeat PopUpWinDefeat { get; private set; }

        // [Space(10)]
        // [Header("Player")]
        [field: SerializeField] public HealthBar PlayerHealthBar { get; private set; }
        [field: SerializeField] public ManaBar PlayerManaBar { get; private set; }
        [field: SerializeField] public DamageText PlayerDamageText { get; private set; }
        [field: SerializeField] public DefenceText PlayerDefenceText { get; private set; }
        [field: SerializeField] public PopUpQuestion PopUpQuestion { get; private set; }

        // [Space(10)]
        // [Header("Enemy")]
        [field: SerializeField] public HealthBar EnemyHealthBar { get; private set; }

        [field: SerializeField] public DamageText EnemyDamageText { get; private set; }
        [field: SerializeField] public DefenceText EnemyDefenceText { get; private set; }

        private void Awake()
        {
            Assert.IsNotNull(PopUpWinDefeat);

            Assert.IsNotNull(PlayerHealthBar);
            Assert.IsNotNull(PlayerManaBar);
            Assert.IsNotNull(PlayerDamageText);
            Assert.IsNotNull(PlayerDefenceText);
            Assert.IsNotNull(PopUpQuestion);

            Assert.IsNotNull(EnemyHealthBar);
            Assert.IsNotNull(EnemyDamageText);
            Assert.IsNotNull(EnemyDefenceText);
        }

        public void Initialize(Health playerHealth, Mana playerMana, Health enemyHealth, Damage playerDamage,
            Damage enemyDamage, Defence playerDefence, Defence enemyDefence)
        {
            PlayerHealthBar.Initialize(playerHealth);
            PlayerManaBar.Initialize(playerMana);
            EnemyHealthBar.Initialize(enemyHealth);
            PlayerDamageText.Initialize(playerDamage);
            EnemyDamageText.Initialize(enemyDamage);
            PlayerDefenceText.Initialize(playerDefence);
            EnemyDefenceText.Initialize(enemyDefence);
        }
    }
}