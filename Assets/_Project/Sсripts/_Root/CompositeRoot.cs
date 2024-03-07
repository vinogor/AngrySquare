using System.Collections.Generic;
using _Project.Config;
using _Project.Controllers;
using _Project.Controllers.PopupChoice;
using _Project.Controllers.Sound;
using _Project.Controllers.StateMachine;
using _Project.Controllers.StateMachine.States;
using _Project.Domain;
using _Project.Domain.Effects;
using _Project.Domain.Effects.Enemy;
using _Project.Domain.Effects.Player;
using _Project.Domain.Movement;
using _Project.Domain.Spells;
using _Project.SDK;
using _Project.SDK.Leader;
using _Project.Services;
using _Project.Services.Save;
using _Project.View;
using Agava.YandexGames;
using Lean.Localization;
using NaughtyAttributes;
using UnityEngine;

namespace _Project._Root
{
    public class CompositeRoot : MonoBehaviour
    {
        [Header("Common")]
        [SerializeField] [Required] private Coefficients _coefficients;
        [SerializeField] [Required] private DiceRoller _diceRoller;
        [SerializeField] private Cell[] _cells;
        [SerializeField] [Required] private CellsSettings _cellsSettings;
        [SerializeField] [Required] private SpellsSettings _spellsSettings;
        [SerializeField] [Required] private SoundSettings _soundSettings;
        [SerializeField] [Required] private EnemyProgression _enemyProgression;
        [SerializeField] [Required] private UiRoot _uiRoot;
        [SerializeField] [Required] private VfxRoot _vfxRoot;
        [SerializeField] [Required] private SoundView _soundView;
        [SerializeField] [Required] private AudioSource _audioSource;
        [SerializeField] [Required] private AudioSource _backgroundAudioSource;
        [SerializeField] [Required] private SaveController _saveController;
        [SerializeField] [Required] private LeanLocalization _leanLocalization;
        [SerializeField] [Required] private FocusTracking _focusTracking;

        [Space(10)]
        [Header("Player")]
        [SerializeField] [Required] private Player _player;
        [SerializeField] [Required] private PlayerMovement _playerMovement;

        [Space(10)]
        [Header("Enemy")]
        [SerializeField] [Required] private EnemyModel _enemyModel;
        [SerializeField] [Required] private EnemyMovement _enemyMovement;
        [SerializeField] private EnemyAim[] _enemyAims;

        private readonly Dictionary<EffectName, Effect> _playerEffects = new();
        private readonly Dictionary<EffectName, Effect> _enemyEffects = new();

        private SoundController _soundController;
        private RestartGameController _restartGameController;
        private ISaver _saver;
        private PopUpTutorialController _popUpTutorialController;
        private YandexLeaderBoard _yandexLeaderBoard;
        private Defence _playerDefence;
        private Health _playerHealth;
        private Damage _playerDamage;
        private Mana _playerMana;
        private EnemyLevel _enemyLevel;
        private int _enemyLevelValue;
        private Defence _enemyDefence;
        private int _healthValue;
        private Health _enemyHealth;
        private Damage _enemyDamage;
        private AvailableSpells _availableSpells;
        private SpellActivator _spellActivator;
        private Advertising _advertising;
        private GameSounds _gameSounds;
        private FiniteStateMachine _stateMachine;
        private GameInitializeFsmState _gameInitializeFsmState;
        private PlayerTurnSpellFsmState _playerTurnSpellFsmState;
        private PlayerTurnMoveFsmState _playerTurnMoveFsmState;
        private PlayerWinFsmState _playerWinFsmState;
        private EnemyTurnFsmState _enemyTurnFsmState;
        private PlayerDefeatFsmState _playerDefeatFsmState;
        private SpellBarController _spellBarController;
        private PopUpNotificationController _popUpPlayerWin;
        private LevelRestarter _levelRestarter;
        private PopUpNotificationController _popUpPlayerDefeat;
        private EnemyTargetController _enemyTargetController;
        private PopUpChoiceEffectController _choiceEffectController;
        private PopUpChoiceSpellController _choiceSpellController;
        private PlayerJumper _playerJumper;
        private EnemyJumper _enemyJumper;

        private async void OnEnable()
        {
            Debug.Log("CompositeRoot started");

            InitializeGameSaver();
            YandexGameReady();

            // DOMAIN

            InitializePlayersCharacteristics();
            InitializeSpells();

            CellsManager cellsController = new CellsManager(_cells, _cellsSettings);
            cellsController.FillWithEffects();

            _enemyTargetController = new EnemyTargetController(cellsController, _enemyAims);
            _enemyTargetController.SetAimToNewRandomTargetCell();

            _playerJumper = new PlayerJumper(_player.transform, _enemyModel.transform, _coefficients);
            _enemyJumper = new EnemyJumper(_enemyModel.transform, _playerMovement, _coefficients,
                _enemyTargetController);
            PlayerPortal playerPortal = new PlayerPortal(_playerJumper, cellsController, _playerMovement);

            _spellBarController = new SpellBarController(_availableSpells, _uiRoot.SpellBarView, _playerMana,
                _spellActivator, _uiRoot.SpellBarShaker);
            List<EffectName> availableEffectNamesForEffect = new List<EffectName>
                { EffectName.Swords, EffectName.Health, EffectName.Mana };
            List<SpellName> availableSpellNamesForEffect = new List<SpellName>
            {
                SpellName.FullHealth, SpellName.UpDamage, SpellName.UpDefence, SpellName.UpMaxHealth,
                SpellName.UpMaxMana
            };
            _choiceEffectController = new PopUpChoiceEffectController(_uiRoot.PopUpChoiceView,
                availableEffectNamesForEffect, _playerMovement, _cellsSettings);
            _choiceSpellController = new PopUpChoiceSpellController(_uiRoot.PopUpChoiceView,
                availableSpellNamesForEffect, _spellBarController, _spellsSettings);

            FillPlayerEffects(_playerJumper, playerPortal, _choiceEffectController, _choiceSpellController);
            FillEnemyEffects(_enemyJumper, _enemyTargetController);

            _playerMovement.Initialize(cellsController, _playerEffects, _coefficients, _playerJumper);
            _enemyMovement.Initialize(_enemyEffects, _enemyTargetController, _enemyJumper, _playerHealth, _enemyDamage,
                _playerMovement);

            // CONTROLLERS

            _diceRoller.Initialize();

            _popUpPlayerWin = new PopUpNotificationController(_uiRoot.PopUpNotificationView,
                new PopUpNotificationModel(UiTextKeys.NotificationPlayerWinTitleKey,
                    UiTextKeys.NotificationPlayerWinInfoKey));
            _popUpPlayerDefeat = new PopUpNotificationController(_uiRoot.PopUpNotificationView,
                new PopUpNotificationModel(UiTextKeys.NotificationPlayerDefeatTitleKey,
                    UiTextKeys.NotificationPlayerDefeatInfoKey));
            _popUpTutorialController = new PopUpTutorialController(_uiRoot.PopUpTutorialView);

            _levelRestarter = new LevelRestarter(cellsController, _playerDefence, _playerHealth,
                _playerDamage, _playerMana, _enemyProgression, _enemyDefence, _enemyHealth, _enemyDamage,
                _availableSpells,
                _playerMovement, _enemyTargetController, _enemyLevel);

            // PRESENTATIONS & FRAMEWORKS

            _enemyModel.Initialize(_enemyLevel);
            _vfxRoot.Initialize(_playerHealth, _playerMana, playerPortal, _enemyHealth);
            _uiRoot.Initialize(_playerHealth, _playerMana, _enemyHealth, _playerDamage, _enemyDamage, _playerDefence,
                _enemyDefence, _availableSpells, _enemyLevel);
            _advertising = new Advertising();
            _soundController = new SoundController(_soundView);
            _gameSounds = new GameSounds(_soundSettings, _audioSource, _backgroundAudioSource);
            InitializeLocalization();

            // INFRASTRUCTURE

            InitializeStateMachine();
            InitializeLeaderboard();

            SaveService saveService = new SaveService(_playerDamage, _playerDefence, _playerHealth, _playerMana,
                _availableSpells, _playerMovement, _enemyLevel, _enemyDamage, _enemyDefence, _enemyHealth,
                _enemyTargetController, cellsController, _stateMachine, _popUpTutorialController, _saver);
            _saveController.Initialize(saveService, _stateMachine);

            _restartGameController =
                new RestartGameController(_uiRoot.RestartGameView, _levelRestarter, _stateMachine, saveService);

            InitializeGameSounds();

#if UNITY_WEBGL && !UNITY_EDITOR
            await _yandexLeaderBoard.Fill();
            _yandexLeaderBoard.GetCurrentPublicName();
#endif

            StartGame(saveService, _stateMachine);
        }

        private void InitializeGameSounds()
        {
            _playerHealth.DamageReceived += _gameSounds.PlaySwordsAttack;
            _playerHealth.Replenished += _gameSounds.PlayHealthReplenish;
            _playerMana.Replenished += _gameSounds.PlayManaReplenish;

            _enemyHealth.DamageReceived += _gameSounds.PlaySwordsAttack;
            _enemyHealth.Replenished += _gameSounds.PlayHealthReplenish;

            _spellActivator.SpellCast += _gameSounds.PlaySpellCast;

            _choiceEffectController.Showed += _gameSounds.PlayPopUp;
            _choiceEffectController.Clicked += _gameSounds.PlayClickButton;
            _spellBarController.SpellSkipped += _gameSounds.PlayClickButton;

            _choiceSpellController.Showed += _gameSounds.PlayPopUp;
            _choiceSpellController.Clicked += _gameSounds.PlayClickButton;

            _playerWinFsmState.Win += _gameSounds.PlayPlayerWin;
            _playerDefeatFsmState.Defeat += _gameSounds.PlayPlayerDefeat;

            _focusTracking.SwitchSound += value => _gameSounds.SwitchByFocus(value);
            _advertising.SwitchSound += value => _gameSounds.SwitchByAdv(value);
            _soundController.SwitchSound += value => _gameSounds.SwitchByButton(value);

            _diceRoller.DiceFall += _gameSounds.PlayDiceFall;

            _playerJumper.Teleported += _gameSounds.PlayTeleport;
            _playerJumper.MadeStep += _gameSounds.PlayPlayerStep;

            _enemyJumper.MadeStep += _gameSounds.PlayEnemyStep;
        }

        private void InitializeStateMachine()
        {
            _stateMachine = new FiniteStateMachine();
            _gameInitializeFsmState = new GameInitializeFsmState(_stateMachine, _advertising);
            _playerTurnSpellFsmState =
                new PlayerTurnSpellFsmState(_stateMachine, _spellBarController, _popUpTutorialController);
            _playerTurnMoveFsmState = new PlayerTurnMoveFsmState(_stateMachine, _diceRoller,
                _playerMovement, _enemyHealth, _popUpTutorialController);
            _playerWinFsmState = new PlayerWinFsmState(_stateMachine, _popUpPlayerWin, _levelRestarter);
            _enemyTurnFsmState = new EnemyTurnFsmState(_stateMachine, _enemyMovement, _playerHealth,
                _popUpTutorialController);
            _playerDefeatFsmState = new PlayerDefeatFsmState(_stateMachine, _popUpPlayerDefeat,
                _levelRestarter);

            _stateMachine.AddState(_gameInitializeFsmState);
            _stateMachine.AddState(_playerTurnSpellFsmState);
            _stateMachine.AddState(_playerTurnMoveFsmState);
            _stateMachine.AddState(_playerWinFsmState);
            _stateMachine.AddState(_enemyTurnFsmState);
            _stateMachine.AddState(_playerDefeatFsmState);
        }

        private void FillEnemyEffects(EnemyJumper enemyJumper, EnemyTargetController enemyTargetController)
        {
            _enemyEffects.Add(EffectName.Swords, new EnemySwords(enemyJumper, _playerHealth, _enemyDamage));
            _enemyEffects.Add(EffectName.Health,
                new EnemyHealth(enemyJumper, _enemyHealth, _coefficients, enemyTargetController));
            _enemyEffects.Add(EffectName.Mana, new EnemyMana(enemyJumper));
            _enemyEffects.Add(EffectName.Portal, new EnemyPortal(enemyJumper));
            _enemyEffects.Add(EffectName.Question, new EnemyQuestion(enemyJumper));
            _enemyEffects.Add(EffectName.SpellBook, new EnemySpellBook(enemyJumper));
        }

        private void FillPlayerEffects(PlayerJumper playerJumper, PlayerPortal playerPortal,
            PopUpChoiceEffectController choiceEffectController, PopUpChoiceSpellController choiceSpellController)
        {
            _playerEffects.Add(EffectName.Swords, new PlayerSwords(playerJumper, _enemyHealth, _playerDamage));
            _playerEffects.Add(EffectName.Health, new PlayerHealth(_playerHealth, playerJumper, _coefficients));
            _playerEffects.Add(EffectName.Mana, new PlayerMana(_playerMana, playerJumper, _coefficients));
            _playerEffects.Add(EffectName.Portal, playerPortal);
            _playerEffects.Add(EffectName.Question, new PlayerQuestion(playerJumper, choiceEffectController));
            _playerEffects.Add(EffectName.SpellBook, new PlayerSpellBook(playerJumper, choiceSpellController));
        }

        private void InitializeLeaderboard()
        {
            _yandexLeaderBoard = new YandexLeaderBoard(_enemyLevel, _playerDefeatFsmState);
            LeaderboardController leaderboardController = new LeaderboardController(_uiRoot.LeaderboardButtonView,
                _uiRoot.LeaderboardPopupView, _yandexLeaderBoard, _coefficients);
        }

        private void InitializeLocalization()
        {
            Localization localization = new Localization(_leanLocalization);
            localization.SetLanguageFromYandex();
            LanguageController languageController = new LanguageController(_uiRoot.LanguageView, localization);
        }

        private void InitializeSpells()
        {
            _availableSpells = new AvailableSpells();
            _availableSpells.Add(SpellName.UpDamage);

            Dictionary<SpellName, Effect> playerSpells = new Dictionary<SpellName, Effect>();
            FullHealthSpell fullHealthSpell = new FullHealthSpell(_playerHealth);
            UpDamageSpell upDamageSpell = new UpDamageSpell(_playerDamage, _coefficients);
            UpMaxHealthSpell upMaxHealthSpell = new UpMaxHealthSpell(_playerHealth, _coefficients);
            UpDefenceSpell upDefenceSpell = new UpDefenceSpell(_playerDefence, _coefficients);
            UpMaxManaSpell upMaxManaSpell = new UpMaxManaSpell(_playerMana, _coefficients);
            playerSpells.Add(SpellName.FullHealth, fullHealthSpell);
            playerSpells.Add(SpellName.UpDamage, upDamageSpell);
            playerSpells.Add(SpellName.UpMaxHealth, upMaxHealthSpell);
            playerSpells.Add(SpellName.UpDefence, upDefenceSpell);
            playerSpells.Add(SpellName.UpMaxMana, upMaxManaSpell);

            _spellActivator = new SpellActivator(playerSpells);
        }

        private void InitializePlayersCharacteristics()
        {
            _playerDefence = new Defence(_coefficients.PlayerStartDefence);
            _playerHealth = new Health(_coefficients.PlayerStartHealth, _coefficients.PlayerMaxHealth,
                _playerDefence);
            _playerDamage = new Damage(_coefficients.PlayerStartDamage);
            _playerMana = new Mana(_coefficients.PlayerStartMana, _coefficients.PlayerMaxMana, _spellsSettings);

            _enemyLevel = new EnemyLevel();
            _enemyLevelValue = _enemyLevel.Value;
            _enemyDefence = new Defence(_enemyProgression.GetDefence(_enemyLevelValue));
            _healthValue = _enemyProgression.GetHealth(_enemyLevelValue);
            _enemyHealth = new Health(_healthValue, _healthValue, _enemyDefence);
            _enemyDamage = new Damage(_enemyProgression.GetDamage(_enemyLevelValue));
        }

        private void InitializeGameSaver()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            _saver = new CloudSaver();
#else
            _saver = new LocalSaver();
#endif
        }

        private void YandexGameReady()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            GameReady();
#endif
        }

        private void GameReady()
        {
            Debug.Log("YandexGamesSdk.GameReady() - STARTED");
            YandexGamesSdk.GameReady();
        }

        private static void StartGame(SaveService saveService, FiniteStateMachine stateMachine)
        {
            saveService.Load(() =>
            {
                Debug.Log("CompositeRoot - IsSaveExist " + saveService.IsSaveExist);

                if (saveService.IsSaveExist == false)
                {
                    stateMachine.SetState<GameInitializeFsmState>();
                }
            });
        }

        private void OnDestroy()
        {
            _soundController.Dispose();
            _restartGameController.Dispose();
            _popUpTutorialController.Dispose();
            _yandexLeaderBoard.Dispose();

            _playerHealth.DamageReceived -= _gameSounds.PlaySwordsAttack;
            _playerHealth.Replenished -= _gameSounds.PlayHealthReplenish;
            _playerMana.Replenished -= _gameSounds.PlayManaReplenish;

            _enemyHealth.DamageReceived -= _gameSounds.PlaySwordsAttack;
            _enemyHealth.Replenished -= _gameSounds.PlayHealthReplenish;

            _spellActivator.SpellCast -= _gameSounds.PlaySpellCast;

            _choiceEffectController.Showed -= _gameSounds.PlayPopUp;
            _choiceEffectController.Clicked -= _gameSounds.PlayClickButton;
            _spellBarController.SpellSkipped -= _gameSounds.PlayClickButton;

            _choiceSpellController.Showed -= _gameSounds.PlayPopUp;
            _choiceSpellController.Clicked -= _gameSounds.PlayClickButton;

            _playerWinFsmState.Win -= _gameSounds.PlayPlayerWin;
            _playerDefeatFsmState.Defeat -= _gameSounds.PlayPlayerDefeat;

            _focusTracking.SwitchSound -= value => _gameSounds.SwitchByFocus(value);
            _advertising.SwitchSound -= value => _gameSounds.SwitchByAdv(value);
            _soundController.SwitchSound -= value => _gameSounds.SwitchByButton(value);

            _diceRoller.DiceFall -= _gameSounds.PlayDiceFall;

            _playerJumper.Teleported -= _gameSounds.PlayTeleport;
            _playerJumper.MadeStep -= _gameSounds.PlayPlayerStep;

            _enemyJumper.MadeStep -= _gameSounds.PlayEnemyStep;
        }
    }
}