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
using UnityEngine.Assertions;

namespace _Project._Root
{
    public class CompositeRoot : MonoBehaviour
    {
        // TODO: сгруппировать по root классам по аналогии с папками

        [Header("Common")]
        [SerializeField] [Required] private Coefficients _coefficients;
        [SerializeField] [Required] private DiceRoller _diceRoller;
        [SerializeField] private Cell[] _cells;
        [SerializeField] [Required] private CellsSettings _cellsSettings;
        [SerializeField] [Required] private SpellsSettings _spellsSettings;
        [SerializeField] [Required] private SoundSettings _soundSettings;
        [SerializeField] [Required] private UiRoot _uiRoot;
        [SerializeField] [Required] private VfxRoot _vfxRoot;
        [SerializeField] [Required] private SoundView _soundView;
        [SerializeField] [Required] private AudioSource _audioSource;
        [SerializeField] [Required] private AudioSource _backgroundAudioSource;
        [SerializeField] [Required] private SaveController _saveController;
        [SerializeField] [Required] private LeanLocalization _leanLocalization;

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

        private async void Start()
        {
            Debug.Log("CompositeRoot started");

#if UNITY_WEBGL && !UNITY_EDITOR
            _saver = new CloudSaver();
            Debug.Log("YandexGamesSdk.GameReady() - STARTED");
            YandexGamesSdk.GameReady();
#else
            _saver = new LocalSaver();
#endif

            Assert.AreEqual(16, _cells.Length);
            Assert.AreEqual(3, _enemyAims.Length);

            GameSounds gameSounds = new GameSounds(_soundSettings, _audioSource, _backgroundAudioSource);
            YandexLeaderBoard yandexLeaderBoard = new YandexLeaderBoard();

            _soundController = new SoundController(_soundView, gameSounds);

            CellsManager cellsManager = new CellsManager(_cells, _cellsSettings);

            Defence playerDefence = new Defence(_coefficients.PlayerStartDefence);
            Health playerHealth = new Health(_coefficients.PlayerStartHealth, _coefficients.PlayerMaxHealth,
                playerDefence, gameSounds);
            Damage playerDamage = new Damage(_coefficients.PlayerStartDamage);
            Mana playerMana = new Mana(_coefficients.PlayerStartMana, _coefficients.PlayerMaxMana, _spellsSettings,
                gameSounds);

            Defence enemyDefence = new Defence(_coefficients.EnemyStartDefence);
            Health enemyHealth = new Health(_coefficients.EnemyStartHealth, _coefficients.EnemyMaxHealth, enemyDefence,
                gameSounds);
            Damage enemyDamage = new Damage(_coefficients.EnemyStartDamage);
            EnemyLevel enemyLevel = new EnemyLevel(yandexLeaderBoard);
            _enemyModel.Initialize(enemyLevel);

            AvailableSpells availableSpells = new AvailableSpells();
            availableSpells.Add(SpellName.UpDamage);

            Dictionary<SpellName, Spell> playerSpells = new Dictionary<SpellName, Spell>();
            playerSpells.Add(SpellName.FullHealth, new FullHealthSpell(gameSounds, playerHealth));
            playerSpells.Add(SpellName.UpDamage, new UpDamageSpell(gameSounds, playerDamage, _coefficients));
            playerSpells.Add(SpellName.UpMaxHealth, new UpMaxHealthSpell(gameSounds, playerHealth, _coefficients));
            playerSpells.Add(SpellName.UpDefence, new UpDefenceSpell(gameSounds, playerDefence, _coefficients));
            playerSpells.Add(SpellName.UpMaxMana, new UpMaxManaSpell(gameSounds, playerMana, _coefficients));

            SpellActivator spellActivator = new SpellActivator(playerSpells);

            // === UI ===

            _uiRoot.Initialize(playerHealth, playerMana, enemyHealth, playerDamage, enemyDamage, playerDefence,
                enemyDefence, availableSpells, enemyLevel);

            LeaderboardController leaderboardController = new LeaderboardController(_uiRoot.LeaderboardButtonView,
                _uiRoot.LeaderboardPopupView, yandexLeaderBoard);

            List<EffectName> availableEffectNames = new List<EffectName>
                { EffectName.Swords, EffectName.Health, EffectName.Mana };
            List<SpellName> availableSpellNames = new List<SpellName>
            {
                SpellName.FullHealth, SpellName.UpDamage, SpellName.UpDefence, SpellName.UpMaxHealth,
                SpellName.UpMaxMana
            };

            PopUpChoiceEffectController choiceEffectController =
                new PopUpChoiceEffectController(_uiRoot.PopUpChoiceView, availableEffectNames, _playerMovement,
                    _cellsSettings, gameSounds);

            SpellBarController spellBarController =
                new SpellBarController(availableSpells, _uiRoot.SpellBarView, playerMana, spellActivator,
                    _uiRoot.SpellBarShaker, gameSounds);

            PopUpChoiceSpellController choiceSpellController =
                new PopUpChoiceSpellController(_uiRoot.PopUpChoiceView, availableSpellNames,
                    spellBarController, _spellsSettings, gameSounds);

            PopUpNotificationController popUpPlayerWin = new PopUpNotificationController(_uiRoot.PopUpNotificationView,
                new PopUpNotificationModel(UiTextKeys.NotificationPlayerWinTitleKey,
                    UiTextKeys.NotificationPlayerWinInfoKey));
            PopUpNotificationController popUpPlayerDefeat = new PopUpNotificationController(
                _uiRoot.PopUpNotificationView,
                new PopUpNotificationModel(UiTextKeys.NotificationPlayerDefeatTitleKey,
                    UiTextKeys.NotificationPlayerDefeatInfoKey));

            _popUpTutorialController = new PopUpTutorialController(_uiRoot.PopUpTutorialView);

            EnemyTargetController enemyTargetController = new EnemyTargetController(cellsManager, _enemyAims);

            LevelRestarter levelRestarter = new LevelRestarter(cellsManager, playerDefence, playerHealth,
                playerDamage, playerMana, enemyDefence, enemyHealth, enemyDamage, availableSpells, _playerMovement,
                enemyTargetController, enemyLevel);

            // === ADS ===

            Advertising advertising = new Advertising(gameSounds);

            // === STATE MACHINE ===
            FiniteStateMachine stateMachine = new FiniteStateMachine();
            stateMachine.AddState(new GameInitializeFsmState(stateMachine, advertising));
            stateMachine.AddState(
                new PlayerTurnSpellFsmState(stateMachine, spellBarController, _popUpTutorialController));
            stateMachine.AddState(new PlayerTurnMoveFsmState(stateMachine, _diceRoller, _playerMovement, enemyHealth,
                _popUpTutorialController));
            stateMachine.AddState(new PlayerWinFsmState(stateMachine, popUpPlayerWin, levelRestarter, gameSounds));
            stateMachine.AddState(new EnemyTurnFsmState(stateMachine, _enemyMovement, playerHealth,
                _popUpTutorialController));
            stateMachine.AddState(new PlayerDefeatFsmState(stateMachine, popUpPlayerDefeat, levelRestarter,
                gameSounds));

            _diceRoller.Initialize(gameSounds);

            enemyTargetController.SetAimToNewRandomTargetCell();

            PlayerJumper playerJumper =
                new PlayerJumper(_player.transform, _enemyModel.transform, _coefficients, gameSounds);
            EnemyJumper enemyJumper =
                new EnemyJumper(_enemyModel.transform, _playerMovement, _coefficients, enemyTargetController,
                    gameSounds);

            // === ЭФФЕКТЫ ЯЧЕЕК ===

            cellsManager.FillWithEffects();

            PlayerPortal playerPortal = new PlayerPortal(playerJumper, cellsManager, _playerMovement);

            _vfxRoot.Initialize(playerHealth, playerMana, playerPortal, enemyHealth);

            CellEffectsInitialize(playerJumper, enemyHealth, playerDamage, playerHealth, playerMana, playerPortal,
                enemyJumper, enemyDamage, enemyTargetController, choiceEffectController, choiceSpellController,
                cellsManager);

            // === SAVE ===

            SaveService saveService = new SaveService(playerDamage, playerDefence, playerHealth, playerMana,
                availableSpells, _playerMovement, enemyLevel, enemyDamage, enemyDefence, enemyHealth,
                enemyTargetController, cellsManager, stateMachine, _popUpTutorialController, _saver);
            _saveController.Initialize(saveService, stateMachine);

            // ========

            _restartGameController =
                new RestartGameController(_uiRoot.RestartGameView, levelRestarter, stateMachine, saveService);

            Localization localization = new Localization(_leanLocalization);
            localization.SetLanguageFromYandex();

            LanguageController languageController = new LanguageController(_uiRoot.LanguageView, localization);

            // в самом конце
            saveService.Load(() =>
            {
                Debug.Log("CompositeRoot - IsSaveExist " + saveService.IsSaveExist);

                if (saveService.IsSaveExist == false)
                {
                    enemyLevel.SetPlayer();
                    stateMachine.SetState<GameInitializeFsmState>();
                }
            });
        }

        private void CellEffectsInitialize(PlayerJumper playerJumper, Health enemyHealth, Damage playerDamage,
            Health playerHealth, Mana playerMana, PlayerPortal playerPortal, EnemyJumper enemyJumper,
            Damage enemyDamage, EnemyTargetController enemyTargetController,
            PopUpChoiceEffectController choiceEffectController, PopUpChoiceSpellController choiceSpellController,
            CellsManager cellsManager)
        {
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
        }

        private void OnDestroy()
        {
            _soundController.Dispose();
            _restartGameController.Dispose();
            _popUpTutorialController.Dispose();
        }
    }
}