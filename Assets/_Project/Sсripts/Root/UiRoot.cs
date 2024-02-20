using _Project.Sсripts.Domain;
using _Project.Sсripts.View;
using NaughtyAttributes;
using UnityEngine;

namespace _Project.Sсripts.Root
{
    public class UiRoot : MonoBehaviour
    {
        [Header("Common")]
        [SerializeField] [Required] private PopUpNotificationView _popUpNotificationView;

        [Space(10)]
        [Header("Player")]
        [SerializeField] [Required] private HealthBarView _playerHealthBarView;
        [SerializeField] [Required] private ManaBarView _playerManaBarView;
        [SerializeField] [Required] private DamageTextView _playerDamageTextView;
        [SerializeField] [Required] private DefenceTextView _playerDefenceTextView;
        [SerializeField] [Required] private PopUpChoiceView _popUpChoiceView;
        [SerializeField] [Required] private SpellBarView _spellBarView;

        [Space(10)]
        [Header("Enemy")]
        [SerializeField] [Required] private HealthBarView _enemyHealthBarView;
        [SerializeField] [Required] private DamageTextView _enemyDamageTextView;
        [SerializeField] [Required] private DefenceTextView _enemyDefenceTextView;

        public PopUpChoiceView PopUpChoiceView => _popUpChoiceView;
        public SpellBarView SpellBarView => _spellBarView;
        public PopUpNotificationView PopUpNotificationView => _popUpNotificationView;

        public void Initialize(Health playerHealth, Mana playerMana, Health enemyHealth, Damage playerDamage,
            Damage enemyDamage, Defence playerDefence, Defence enemyDefence, Spells spells)
        {
            _playerHealthBarView.Initialize(playerHealth);
            _playerManaBarView.Initialize(playerMana);
            _enemyHealthBarView.Initialize(enemyHealth);
            _playerDamageTextView.Initialize(playerDamage);
            _enemyDamageTextView.Initialize(enemyDamage);
            _playerDefenceTextView.Initialize(playerDefence);
            _enemyDefenceTextView.Initialize(enemyDefence);
            _spellBarView.Initialize(spells);
        }
    }
}