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

        private async void OnEnable()
        {
            Debug.Log("CompositeRoot started");

            InitSaver();
            YandexGameReady();

            // DOMAIN

            CellsManager cellsManager = new CellsManager(_cells, _cellsSettings);
            cellsManager.FillWithEffects();

            _diceRoller.Initialize();

            Defence playerDefence = new Defence(_coefficients.PlayerStartDefence);
            Health playerHealth = new Health(_coefficients.PlayerStartHealth, _coefficients.PlayerMaxHealth,
                playerDefence);
            Damage playerDamage = new Damage(_coefficients.PlayerStartDamage);
            Mana playerMana = new Mana(_coefficients.PlayerStartMana, _coefficients.PlayerMaxMana, _spellsSettings);

            EnemyLevel enemyLevel = new EnemyLevel();
            int enemyLevelValue = enemyLevel.Value;
            Defence enemyDefence = new Defence(_enemyProgression.GetDefence(enemyLevelValue));
            int healthValue = _enemyProgression.GetHealth(enemyLevelValue);
            Health enemyHealth = new Health(healthValue, healthValue, enemyDefence);
            Damage enemyDamage = new Damage(_enemyProgression.GetDamage(enemyLevelValue));

            _enemyModel.Initialize(enemyLevel);

            AvailableSpells availableSpells = new AvailableSpells();
            availableSpells.Add(SpellName.UpDamage);

            Dictionary<SpellName, Spell> playerSpells = new Dictionary<SpellName, Spell>();
            FullHealthSpell fullHealthSpell = new FullHealthSpell(playerHealth);
            UpDamageSpell upDamageSpell = new UpDamageSpell(playerDamage, _coefficients);
            UpMaxHealthSpell upMaxHealthSpell = new UpMaxHealthSpell(playerHealth, _coefficients);
            UpDefenceSpell upDefenceSpell = new UpDefenceSpell(playerDefence, _coefficients);
            UpMaxManaSpell upMaxManaSpell = new UpMaxManaSpell(playerMana, _coefficients);
            playerSpells.Add(SpellName.FullHealth, fullHealthSpell);
            playerSpells.Add(SpellName.UpDamage, upDamageSpell);
            playerSpells.Add(SpellName.UpMaxHealth, upMaxHealthSpell);
            playerSpells.Add(SpellName.UpDefence, upDefenceSpell);
            playerSpells.Add(SpellName.UpMaxMana, upMaxManaSpell);

            SpellActivator spellActivator = new SpellActivator(playerSpells);

            List<EffectName> availableEffectNames = new List<EffectName>
                { EffectName.Swords, EffectName.Health, EffectName.Mana };
            List<SpellName> availableSpellNames = new List<SpellName>
            {
                SpellName.FullHealth, SpellName.UpDamage, SpellName.UpDefence, SpellName.UpMaxHealth,
                SpellName.UpMaxMana
            };

            EnemyTargetController enemyTargetController = new EnemyTargetController(cellsManager, _enemyAims);
            enemyTargetController.SetAimToNewRandomTargetCell();
            PlayerJumper playerJumper = new PlayerJumper(_player.transform, _enemyModel.transform, _coefficients);
            EnemyJumper enemyJumper = new EnemyJumper(_enemyModel.transform, _playerMovement, _coefficients,
                enemyTargetController);
            PlayerPortal playerPortal = new PlayerPortal(playerJumper, cellsManager, _playerMovement);
            _vfxRoot.Initialize(playerHealth, playerMana, playerPortal, enemyHealth);

            // CONTROLLERS

            PopUpChoiceEffectController choiceEffectController =
                new PopUpChoiceEffectController(_uiRoot.PopUpChoiceView, availableEffectNames, _playerMovement,
                    _cellsSettings);
            SpellBarController spellBarController =
                new SpellBarController(availableSpells, _uiRoot.SpellBarView, playerMana, spellActivator,
                    _uiRoot.SpellBarShaker);
            PopUpChoiceSpellController choiceSpellController =
                new PopUpChoiceSpellController(_uiRoot.PopUpChoiceView, availableSpellNames,
                    spellBarController, _spellsSettings);
            PopUpNotificationController popUpPlayerWin = new PopUpNotificationController(_uiRoot.PopUpNotificationView,
                new PopUpNotificationModel(UiTextKeys.NotificationPlayerWinTitleKey,
                    UiTextKeys.NotificationPlayerWinInfoKey));
            PopUpNotificationController popUpPlayerDefeat = new PopUpNotificationController(
                _uiRoot.PopUpNotificationView, new PopUpNotificationModel(UiTextKeys.NotificationPlayerDefeatTitleKey,
                    UiTextKeys.NotificationPlayerDefeatInfoKey));
            _popUpTutorialController = new PopUpTutorialController(_uiRoot.PopUpTutorialView);

            LevelRestarter levelRestarter = new LevelRestarter(cellsManager, playerDefence, playerHealth,
                playerDamage, playerMana, _enemyProgression, enemyDefence, enemyHealth, enemyDamage, availableSpells,
                _playerMovement, enemyTargetController, enemyLevel);

            // PRESENTATIONS & FRAMEWORKS

            _uiRoot.Initialize(playerHealth, playerMana, enemyHealth, playerDamage, enemyDamage, playerDefence,
                enemyDefence, availableSpells, enemyLevel);

            _playerEffects.Add(EffectName.Swords,
                new PlayerSwords(playerJumper, enemyHealth, playerDamage));
            _playerEffects.Add(EffectName.Health, new PlayerHealth(playerHealth, playerJumper, _coefficients));
            _playerEffects.Add(EffectName.Mana, new PlayerMana(playerMana, playerJumper, _coefficients));
            _playerEffects.Add(EffectName.Portal, playerPortal);
            _playerEffects.Add(EffectName.Question, new PlayerQuestion(playerJumper, choiceEffectController));
            _playerEffects.Add(EffectName.SpellBook, new PlayerSpellBook(playerJumper, choiceSpellController));

            _playerMovement.Initialize(cellsManager, _playerEffects, _coefficients, playerJumper);

            _enemyEffects.Add(EffectName.Swords, new EnemySwords(enemyJumper, playerHealth, enemyDamage));
            _enemyEffects.Add(EffectName.Health,
                new EnemyHealth(enemyJumper, enemyHealth, _coefficients, enemyTargetController));
            _enemyEffects.Add(EffectName.Mana, new EnemyMana(enemyJumper));
            _enemyEffects.Add(EffectName.Portal, new EnemyPortal(enemyJumper));
            _enemyEffects.Add(EffectName.Question, new EnemyQuestion(enemyJumper));
            _enemyEffects.Add(EffectName.SpellBook, new EnemySpellBook(enemyJumper));

            _enemyMovement.Initialize(_enemyEffects, enemyTargetController, enemyJumper, playerHealth, enemyDamage,
                _playerMovement);

            // INFRASTRUCTURE

            Advertising advertising = new Advertising();

            FiniteStateMachine stateMachine = new FiniteStateMachine();
            GameInitializeFsmState gameInitializeFsmState = new GameInitializeFsmState(stateMachine, advertising);
            PlayerTurnSpellFsmState playerTurnSpellFsmState =
                new PlayerTurnSpellFsmState(stateMachine, spellBarController, _popUpTutorialController);
            PlayerTurnMoveFsmState playerTurnMoveFsmState = new PlayerTurnMoveFsmState(stateMachine, _diceRoller,
                _playerMovement, enemyHealth, _popUpTutorialController);
            PlayerWinFsmState playerWinFsmState =
                new PlayerWinFsmState(stateMachine, popUpPlayerWin, levelRestarter);
            EnemyTurnFsmState enemyTurnFsmState = new EnemyTurnFsmState(stateMachine, _enemyMovement, playerHealth,
                _popUpTutorialController);
            PlayerDefeatFsmState playerDefeatFsmState = new PlayerDefeatFsmState(stateMachine, popUpPlayerDefeat,
                levelRestarter);

            stateMachine.AddState(gameInitializeFsmState);
            stateMachine.AddState(playerTurnSpellFsmState);
            stateMachine.AddState(playerTurnMoveFsmState);
            stateMachine.AddState(playerWinFsmState);
            stateMachine.AddState(enemyTurnFsmState);
            stateMachine.AddState(playerDefeatFsmState);

            Localization localization = new Localization(_leanLocalization);
            localization.SetLanguageFromYandex();
            LanguageController languageController = new LanguageController(_uiRoot.LanguageView, localization);

            // === SOUNDS ===

            _soundController = new SoundController(_soundView);
            GameSounds gameSounds = new GameSounds(_soundSettings, _audioSource, _backgroundAudioSource);

            playerHealth.DamageReceived += gameSounds.PlaySwordsAttack;
            playerHealth.Replenished += gameSounds.PlayHealthReplenish;
            playerMana.Replenished += gameSounds.PlayManaReplenish;

            enemyHealth.DamageReceived += gameSounds.PlaySwordsAttack;
            enemyHealth.Replenished += gameSounds.PlayHealthReplenish;

            fullHealthSpell.SpellCast += gameSounds.PlaySpellCast;
            upDamageSpell.SpellCast += gameSounds.PlaySpellCast;
            upMaxHealthSpell.SpellCast += gameSounds.PlaySpellCast;
            upDefenceSpell.SpellCast += gameSounds.PlaySpellCast;
            upMaxManaSpell.SpellCast += gameSounds.PlaySpellCast;

            choiceEffectController.Showed += gameSounds.PlayPopUp;
            choiceEffectController.Clicked += gameSounds.PlayClickButton;
            spellBarController.SpellSkipped += gameSounds.PlayClickButton;

            choiceSpellController.Showed += gameSounds.PlayPopUp;
            choiceSpellController.Clicked += gameSounds.PlayClickButton;

            playerWinFsmState.Win += gameSounds.PlayPlayerWin;
            playerDefeatFsmState.Defeat += gameSounds.PlayPlayerDefeat;

            _focusTracking.SwitchSound += value => gameSounds.SwitchByFocus(value);
            advertising.SwitchSound += value => gameSounds.SwitchByAdv(value);
            _soundController.SwitchSound += value => gameSounds.SwitchByButton(value);

            _diceRoller.DiceFall += gameSounds.PlayDiceFall;

            playerJumper.Teleported += gameSounds.PlayTeleport;
            playerJumper.MadeStep += gameSounds.PlayPlayerStep;

            enemyJumper.MadeStep += gameSounds.PlayEnemyStep;

            // TODO: сделать отписку

            // === SAVE / RESTART ===

            _yandexLeaderBoard = new YandexLeaderBoard(enemyLevel, playerDefeatFsmState);
            LeaderboardController leaderboardController = new LeaderboardController(_uiRoot.LeaderboardButtonView,
                _uiRoot.LeaderboardPopupView, _yandexLeaderBoard, _coefficients);

            SaveService saveService = new SaveService(playerDamage, playerDefence, playerHealth, playerMana,
                availableSpells, _playerMovement, enemyLevel, enemyDamage, enemyDefence, enemyHealth,
                enemyTargetController, cellsManager, stateMachine, _popUpTutorialController, _saver);
            _saveController.Initialize(saveService, stateMachine);

            _restartGameController =
                new RestartGameController(_uiRoot.RestartGameView, levelRestarter, stateMachine, saveService);

#if UNITY_WEBGL && !UNITY_EDITOR
            await _yandexLeaderBoard.Fill();
            _yandexLeaderBoard.GetCurrentPublicName();
#endif

            // === START GAME ===

            StartGame(saveService, stateMachine);
        }

        private void InitSaver()
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
        }
    }
}