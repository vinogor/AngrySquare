using _Project.Sсripts.DamageAndDefence;
using _Project.Sсripts.HealthAndMana;
using _Project.Sсripts.Scriptable;
using _Project.Sсripts.UI;
using _Project.Sсripts.UI.PopupChoice;
using _Project.Sсripts.UI.PopUpNotification;
using NaughtyAttributes;
using UnityEngine;

namespace _Project.Sсripts
{
    public class UiRoot : MonoBehaviour
    {
        [Header("Common")]
        [SerializeField] [Required] private PopUpNotification _popUpNotification;

        [Space(10)]
        [Header("Player")]
        [SerializeField] [Required] private HealthBar _playerHealthBar;
        [SerializeField] [Required] private ManaBar _playerManaBar;
        [SerializeField] [Required] private DamageText _playerDamageText;
        [SerializeField] [Required] private DefenceText _playerDefenceText;
        [SerializeField] [Required] private PopUpChoice _popUpChoice;
        [SerializeField] [Required] private SpellBar _spellBar;

        [Space(10)]
        [Header("Enemy")]
        [SerializeField] [Required] private HealthBar _enemyHealthBar;
        [SerializeField] [Required] private DamageText _enemyDamageText;
        [SerializeField] [Required] private DefenceText _enemyDefenceText;

        public PopUpNotification PopUpNotification => _popUpNotification;
        public PopUpChoice PopUpChoice => _popUpChoice;

        public PopUpNotificationController PopUpPlayerWinNotificationController { get; private set; }
        public PopUpNotificationController PopPlayerDefeatNotificationController { get; private set; }
        public SpellBarController SpellBarController { get; private set; }

        public void Initialize(Health playerHealth, Mana playerMana, Health enemyHealth, Damage playerDamage,
            Damage enemyDamage, Defence playerDefence, Defence enemyDefence, SpellsSettings spellsSettings)
        {
            _playerHealthBar.Initialize(playerHealth);
            _playerManaBar.Initialize(playerMana);
            _enemyHealthBar.Initialize(enemyHealth);
            _playerDamageText.Initialize(playerDamage);
            _enemyDamageText.Initialize(enemyDamage);
            _playerDefenceText.Initialize(playerDefence);
            _enemyDefenceText.Initialize(enemyDefence);

            PopUpPlayerWinNotificationController = new PopUpNotificationController(_popUpNotification, 
                new PopUpNotificationModel("Player Win", "It's time to fight a new opponent!"));
            PopPlayerDefeatNotificationController = new PopUpNotificationController(_popUpNotification,
                new PopUpNotificationModel("Player Lose", "You lost the game!"));
            SpellBarController = new SpellBarController(_spellBar, spellsSettings);
        }
    }
}