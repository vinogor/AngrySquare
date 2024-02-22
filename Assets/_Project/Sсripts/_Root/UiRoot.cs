using _Project.Sсripts.Domain;
using _Project.Sсripts.View;
using NaughtyAttributes;
using UnityEngine;

namespace _Project.Sсripts._Root
{
    public class UiRoot : MonoBehaviour
    {
        [Header("Common")]
        [SerializeField] [Required] private PopUpNotificationView _popUpNotificationView;

        [Space(10)]
        [Header("Player")]
        [SerializeField] [Required] private HealthBarView _playerHealthBarView;
        [SerializeField] [Required] private ManaBarView _playerManaBarView;
        [SerializeField] [Required] private DamageView _playerDamageView;
        [SerializeField] [Required] private DefenceTextView _playerDefenceTextView;
        [SerializeField] [Required] private PopUpChoiceView _popUpChoiceView;
        [SerializeField] [Required] private SpellBarView _spellBarView;
        [SerializeField] [Required] private SpellBarShaker _spellBarShaker;

        [Space(10)]
        [Header("Enemy")]
        [SerializeField] [Required] private HealthBarView _enemyHealthBarView;
        [SerializeField] [Required] private DamageView _enemyDamageView;
        [SerializeField] [Required] private DefenceTextView _enemyDefenceTextView;
        [SerializeField] [Required] private EnemyLevelTextView _enemyLevelTextView;

        public PopUpChoiceView PopUpChoiceView => _popUpChoiceView;
        public SpellBarView SpellBarView => _spellBarView;
        public PopUpNotificationView PopUpNotificationView => _popUpNotificationView;
        public SpellBarShaker SpellBarShaker => _spellBarShaker;

        public void Initialize(Health playerHealth, Mana playerMana, Health enemyHealth, Damage playerDamage,
            Damage enemyDamage, Defence playerDefence, Defence enemyDefence, AvailableSpells availableSpells,
            EnemyLevel enemyLevel)
        {
            _playerHealthBarView.Initialize(playerHealth);
            _playerManaBarView.Initialize(playerMana);
            _enemyHealthBarView.Initialize(enemyHealth);
            _playerDamageView.Initialize(playerDamage);
            _enemyDamageView.Initialize(enemyDamage);
            _playerDefenceTextView.Initialize(playerDefence);
            _enemyDefenceTextView.Initialize(enemyDefence);
            _spellBarView.Initialize(availableSpells);
            _enemyLevelTextView.Initialize(enemyLevel);
            _spellBarShaker.Initialize();
        }
    }
}