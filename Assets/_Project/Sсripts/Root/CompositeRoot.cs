using System.Collections.Generic;
using Agava.YandexGames;
using Config;
using Controllers;
using Controllers.PopupChoice;
using Controllers.Sound;
using Controllers.StateMachine;
using Controllers.StateMachine.States;
using Domain;
using Domain.Effects;
using Domain.Effects.Player;
using Domain.Movement;
using Domain.Spells;
using Lean.Localization;
using NaughtyAttributes;
using SDK;
using SDK.Leader;
using Services;
using Services.Save;
using UnityEngine;
using View;

namespace Root
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
        [SerializeField] [Required] private SoundsView _soundsView;
        [SerializeField] [Required] private EnemyProgression _enemyProgression;
        [SerializeField] [Required] private UiRoot _uiRoot;
        [SerializeField] [Required] private VfxRoot _vfxRoot;
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
        [SerializeField] [Required] private EnemyBase _enemyBase;
        [SerializeField] private EnemyAim[] _enemyAims;

        private async void OnEnable()
        {
            Debug.Log("CompositeRoot started");

            YandexGameReady();
            ISaver saver = GetGameSaver();

            GameSoundsPresenter gameSoundsPresenter = new GameSoundsPresenter(_soundSettings, _soundsView);

            _focusTracking.Initialize(gameSoundsPresenter);

            Defence playerDefence = new Defence(_coefficients.PlayerStartDefence);
            Health playerHealth = new Health(_coefficients.PlayerStartHealth, _coefficients.PlayerMaxHealth,
                playerDefence, gameSoundsPresenter);
            Damage playerDamage = new Damage(_coefficients.PlayerStartDamage);
            Mana playerMana = new Mana(_coefficients.PlayerStartMana, _coefficients.PlayerMaxMana, _spellsSettings,
                gameSoundsPresenter);

            EnemyLevel enemyLevel = new EnemyLevel();
            int enemyLevelValue = enemyLevel.Value;
            Defence enemyDefence = new Defence(_enemyProgression.GetDefence(enemyLevelValue));
            int healthValue = _enemyProgression.GetHealth(enemyLevelValue);
            Health enemyHealth = new Health(healthValue, healthValue, enemyDefence, gameSoundsPresenter);
            Damage enemyDamage = new Damage(_enemyProgression.GetDamage(enemyLevelValue));

            AvailableSpells availableSpells = new AvailableSpells();
            availableSpells.Add(SpellName.UpDamage);

            Dictionary<SpellName, Effect> playerSpells = EffectsFactory.CreatePlayerSpellsDictionary(
                playerHealth, playerDamage, _coefficients, playerDefence, playerMana);

            SpellActivator spellActivator = new SpellActivator(playerSpells, gameSoundsPresenter);

            CellsManager cellsController = new CellsManager(_cells, _cellsSettings);
            cellsController.FillWithEffects();

            EnemyTargetController enemyTargetController = new EnemyTargetController(cellsController, _enemyAims);
            enemyTargetController.SetAimToNewRandomTargetCell();

            PlayerJumper playerJumper = new PlayerJumper(_player.transform, _enemyModel.transform, _coefficients,
                gameSoundsPresenter);
            EnemyJumper enemyJumper = new EnemyJumper(_enemyModel.transform, _playerMovement, _coefficients,
                enemyTargetController, gameSoundsPresenter, _enemyBase.transform.position);
            PlayerPortal playerPortal = new PlayerPortal(playerJumper, cellsController, _playerMovement);

            SpellBarController spellBarController = new SpellBarController(availableSpells, _uiRoot.SpellBarView,
                playerMana, spellActivator, _uiRoot.SpellBarFameScaler, gameSoundsPresenter);
            List<EffectName> availableEffectNamesForEffect = new List<EffectName>
                { EffectName.Swords, EffectName.Health, EffectName.Mana };
            List<SpellName> availableSpellNamesForEffect = new List<SpellName>
            {
                SpellName.FullHealth, SpellName.UpDamage, SpellName.UpDefence, SpellName.UpMaxHealth,
                SpellName.UpMaxMana
            };
            PopUpChoiceEffectPresenter choiceEffectPresenter = new PopUpChoiceEffectPresenter(_uiRoot.PopUpChoiceView,
                availableEffectNamesForEffect, _playerMovement, _cellsSettings, gameSoundsPresenter);
            PopUpChoiceSpellPresenter choiceSpellPresenter = new PopUpChoiceSpellPresenter(_uiRoot.PopUpChoiceView,
                availableSpellNamesForEffect, spellBarController, _spellsSettings, gameSoundsPresenter);

            Dictionary<EffectName, Effect> playerEffects = EffectsFactory.CreatePlayerEffectsDictionary(
                playerHealth, playerDamage, playerMana, playerJumper, enemyHealth, _coefficients, playerPortal,
                choiceEffectPresenter, choiceSpellPresenter);

            Dictionary<EffectName, Effect> enemyEffects = EffectsFactory.CreateEnemyEffectsDictionary(
                enemyJumper, playerHealth, enemyDamage, enemyHealth, _coefficients, enemyTargetController);

            _playerMovement.Initialize(cellsController, playerEffects, _coefficients, playerJumper);
            _enemyMovement.Initialize(enemyEffects, enemyTargetController, enemyJumper, playerHealth, enemyDamage,
                _playerMovement);

            _diceRoller.Initialize(gameSoundsPresenter);

            PopUpNotificationController popUpNotificationController = new PopUpNotificationController(
                _uiRoot.PopUpNotificationView,
                new PopUpNotificationModel(UiTextKeys.NotificationPlayerWinTitleKey,
                    UiTextKeys.NotificationPlayerWinInfoKey));
            PopUpNotificationController popUpPlayerDefeat = new PopUpNotificationController(
                _uiRoot.PopUpNotificationView,
                new PopUpNotificationModel(UiTextKeys.NotificationPlayerDefeatTitleKey,
                    UiTextKeys.NotificationPlayerDefeatInfoKey));
            PopUpTutorialController popUpTutorialController = new PopUpTutorialController(_uiRoot.PopUpTutorialView);

            LevelRestarter levelRestarter = new LevelRestarter(cellsController, playerDefence, playerHealth,
                playerDamage, playerMana, _enemyProgression, enemyDefence, enemyHealth, enemyDamage,
                availableSpells, _playerMovement, enemyTargetController, enemyLevel, _enemyMovement,
                choiceEffectPresenter, choiceSpellPresenter, _diceRoller);

            Localization localization = new Localization(_leanLocalization);
            localization.SetLanguageFromYandex();
            LanguageButtonController languageButtonController =
                new LanguageButtonController(_uiRoot.LanguageButtonView, localization);

            SoundButtonPresenter soundButtonPresenter =
                new SoundButtonPresenter(_uiRoot.SoundButtonView, gameSoundsPresenter);

            YandexLeaderBoard yandexLeaderBoard = new YandexLeaderBoard(enemyLevel);
            LeaderboardController leaderboardController = new LeaderboardController(_uiRoot.LeaderboardButtonView,
                _uiRoot.LeaderboardPopupView, yandexLeaderBoard, _coefficients);

            _enemyModel.Initialize(enemyLevel);
            _vfxRoot.Initialize(playerHealth, playerMana, playerPortal, enemyHealth);

            Advertising advertising = new Advertising(gameSoundsPresenter);

            FiniteStateMachine stateMachine = StateMachineFactory.CreateStateMachine(spellBarController,
                popUpTutorialController, popUpNotificationController, popUpPlayerDefeat, advertising, _diceRoller,
                _playerMovement, playerHealth, enemyHealth, levelRestarter, gameSoundsPresenter, _enemyMovement,
                yandexLeaderBoard
            );

            RestartGameController restartGameController =
                new RestartGameController(_uiRoot.RestartGameView, levelRestarter, stateMachine);

            _uiRoot.Initialize(playerHealth, playerMana, enemyHealth, playerDamage, enemyDamage, playerDefence,
                enemyDefence, availableSpells, enemyLevel, languageButtonController, soundButtonPresenter,
                popUpNotificationController, popUpTutorialController, leaderboardController, restartGameController);

            SaveService saveService = new SaveService(playerDamage, playerDefence, playerHealth, playerMana,
                availableSpells, _playerMovement, enemyLevel, enemyDamage, enemyDefence, enemyHealth,
                enemyTargetController, cellsController, stateMachine, popUpTutorialController, saver);
            _saveController.Initialize(saveService, stateMachine);

#if UNITY_WEBGL && !UNITY_EDITOR
            await yandexLeaderBoard.Fill();
            yandexLeaderBoard.GetCurrentPublicName();
#endif

            StartGame(saveService, stateMachine);
        }

        private ISaver GetGameSaver()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return new CloudSaver();
#else
            return new LocalSaver();
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
    }
}