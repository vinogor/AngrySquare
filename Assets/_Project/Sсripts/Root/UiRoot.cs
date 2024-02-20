using _Project.Sсripts.DamageAndDefence;
using _Project.Sсripts.HealthAndMana;
using _Project.Sсripts.UI.PopUpNotification;
using _Project.Sсripts.UI.SpellCast;
using NaughtyAttributes;
using UnityEngine;

namespace _Project.Sсripts
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

        public PopUpNotificationController PopUpPlayerWinNotificationController { get; private set; }
        public PopUpNotificationController PopPlayerDefeatNotificationController { get; private set; }

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

            // TODO: вынести в композит рут? 

            PopUpPlayerWinNotificationController = new PopUpNotificationController(_popUpNotificationView,
                new PopUpNotificationModel("Player Win", "It's time to fight a new opponent!"));
            PopPlayerDefeatNotificationController = new PopUpNotificationController(_popUpNotificationView,
                new PopUpNotificationModel("Player Lose", "You lost the game!"));
        }
    }
}