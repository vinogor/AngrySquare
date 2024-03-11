using Domain;
using NaughtyAttributes;
using Services;
using UnityEngine;
using View;

namespace Root
{
    public class UiRoot : MonoBehaviour
    {
        [Header("Common")]
        [SerializeField] [Required] private PopUpNotificationView _popUpNotificationView;
        [SerializeField] [Required] private PopUpNotificationView _popUpTutorialView;
        [SerializeField] [Required] private RestartGameView _restartGameView;
        [SerializeField] [Required] private LanguageButtonView _languageButtonView;
        [SerializeField] [Required] private LeaderboardButtonView _leaderboardButtonView;
        [SerializeField] [Required] private LeaderboardPopupView _leaderboardPopupView;
        [SerializeField] [Required] private SoundButtonView _soundButtonView;

        [Space(10)]
        [Header("Player")]
        [SerializeField] [Required] private HealthBarView _playerHealthBarView;
        [SerializeField] [Required] private ManaBarView _playerManaBarView;
        [SerializeField] [Required] private DamageView _playerDamageView;
        [SerializeField] [Required] private DefenceTextView _playerDefenceTextView;
        [SerializeField] [Required] private PopUpChoiceView _popUpChoiceView;
        [SerializeField] [Required] private SpellBarView _spellBarView;
        [SerializeField] [Required] private SpellBarFameScaler _spellBarFameScaler;

        [Space(10)]
        [Header("Enemy")]
        [SerializeField] [Required] private HealthBarView _enemyHealthBarView;
        [SerializeField] [Required] private DamageView _enemyDamageView;
        [SerializeField] [Required] private DefenceTextView _enemyDefenceTextView;
        [SerializeField] [Required] private EnemyLevelTextView _enemyLevelTextView;

        public PopUpChoiceView PopUpChoiceView => _popUpChoiceView;
        public SpellBarView SpellBarView => _spellBarView;
        public PopUpNotificationView PopUpNotificationView => _popUpNotificationView;
        public PopUpNotificationView PopUpTutorialView => _popUpTutorialView;
        public SpellBarFameScaler SpellBarFameScaler => _spellBarFameScaler;
        public RestartGameView RestartGameView => _restartGameView;
        public LanguageButtonView LanguageButtonView => _languageButtonView;
        public LeaderboardButtonView LeaderboardButtonView => _leaderboardButtonView;
        public LeaderboardPopupView LeaderboardPopupView => _leaderboardPopupView;
        public SoundButtonView SoundButtonView => _soundButtonView;

        public void Initialize(Health playerHealth, Mana playerMana, Health enemyHealth, Damage playerDamage,
            Damage enemyDamage, Defence playerDefence, Defence enemyDefence, AvailableSpells availableSpells,
            EnemyLevel enemyLevel, IPresenter languageController, IPresenter soundButtonPresenter,
            IPresenter popUpNotificationController, IPresenter popUpTutorialController,
            IPresenter leaderboardController, IPresenter restartGameController)
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
            _languageButtonView.Initialize(languageController);
            _soundButtonView.Initialize(soundButtonPresenter);
            _popUpNotificationView.Initialize(popUpNotificationController);
            _popUpTutorialView.Initialize(popUpTutorialController);
            _leaderboardButtonView.Initialize(leaderboardController);
            _restartGameView.Initialize(restartGameController);
        }
    }
}