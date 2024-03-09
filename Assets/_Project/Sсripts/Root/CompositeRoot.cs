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
using Domain.Effects.Enemy;
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

        private async void OnEnable()
        {
            Debug.Log("CompositeRoot started");

            YandexGameReady();
            ISaver saver = GetGameSaver();

            // DOMAIN

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

            GameSounds gameSounds = new GameSounds(_soundSettings, _audioSource, _backgroundAudioSource);

            AvailableSpells availableSpells = new AvailableSpells();
            availableSpells.Add(SpellName.UpDamage);

            Dictionary<SpellName, Effect> playerSpells = new Dictionary<SpellName, Effect>();
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

            CellsManager cellsController = new CellsManager(_cells, _cellsSettings);
            cellsController.FillWithEffects();

            EnemyTargetController enemyTargetController = new EnemyTargetController(cellsController, _enemyAims);
            enemyTargetController.SetAimToNewRandomTargetCell();

            PlayerJumper playerJumper = new PlayerJumper(_player.transform, _enemyModel.transform, _coefficients);
            EnemyJumper enemyJumper = new EnemyJumper(_enemyModel.transform, _playerMovement, _coefficients,
                enemyTargetController);
            PlayerPortal playerPortal = new PlayerPortal(playerJumper, cellsController, _playerMovement);

            SpellBarController spellBarController = new SpellBarController(availableSpells, _uiRoot.SpellBarView,
                playerMana,
                spellActivator, _uiRoot.SpellBarShaker);
            List<EffectName> availableEffectNamesForEffect = new List<EffectName>
                { EffectName.Swords, EffectName.Health, EffectName.Mana };
            List<SpellName> availableSpellNamesForEffect = new List<SpellName>
            {
                SpellName.FullHealth, SpellName.UpDamage, SpellName.UpDefence, SpellName.UpMaxHealth,
                SpellName.UpMaxMana
            };
            PopUpChoiceEffectPresenter choiceEffectPresenter = new PopUpChoiceEffectPresenter(_uiRoot.PopUpChoiceView,
                availableEffectNamesForEffect, _playerMovement, _cellsSettings, gameSounds);
            PopUpChoiceSpellPresenter choiceSpellPresenter = new PopUpChoiceSpellPresenter(_uiRoot.PopUpChoiceView,
                availableSpellNamesForEffect, spellBarController, _spellsSettings);

            Dictionary<EffectName, Effect> playerEffects = new()
            {
                { EffectName.Swords, new PlayerSwords(playerJumper, enemyHealth, playerDamage) },
                { EffectName.Health, new PlayerHealth(playerHealth, playerJumper, _coefficients) },
                { EffectName.Mana, new PlayerMana(playerMana, playerJumper, _coefficients) },
                { EffectName.Portal, playerPortal },
                { EffectName.Question, new PlayerQuestion(playerJumper, choiceEffectPresenter) },
                { EffectName.SpellBook, new PlayerSpellBook(playerJumper, choiceSpellPresenter) }
            };

            Dictionary<EffectName, Effect> enemyEffects = new()
            {
                { EffectName.Swords, new EnemySwords(enemyJumper, playerHealth, enemyDamage) },
                { EffectName.Health, new EnemyHealth(enemyJumper, enemyHealth, _coefficients, enemyTargetController) },
                { EffectName.Mana, new EnemyMana(enemyJumper) },
                { EffectName.Portal, new EnemyPortal(enemyJumper) },
                { EffectName.Question, new EnemyQuestion(enemyJumper) },
                { EffectName.SpellBook, new EnemySpellBook(enemyJumper) }
            };

            _playerMovement.Initialize(cellsController, playerEffects, _coefficients, playerJumper);
            _enemyMovement.Initialize(enemyEffects, enemyTargetController, enemyJumper, playerHealth, enemyDamage,
                _playerMovement);

            // CONTROLLERS

            _diceRoller.Initialize();

            PopUpNotificationController popUpPlayerWin = new PopUpNotificationController(_uiRoot.PopUpNotificationView,
                new PopUpNotificationModel(UiTextKeys.NotificationPlayerWinTitleKey,
                    UiTextKeys.NotificationPlayerWinInfoKey));
            PopUpNotificationController popUpPlayerDefeat = new PopUpNotificationController(
                _uiRoot.PopUpNotificationView,
                new PopUpNotificationModel(UiTextKeys.NotificationPlayerDefeatTitleKey,
                    UiTextKeys.NotificationPlayerDefeatInfoKey));
            PopUpTutorialController popUpTutorialController = new PopUpTutorialController(_uiRoot.PopUpTutorialView);

            LevelRestarter levelRestarter = new LevelRestarter(cellsController, playerDefence, playerHealth,
                playerDamage, playerMana, _enemyProgression, enemyDefence, enemyHealth, enemyDamage,
                availableSpells,
                _playerMovement, enemyTargetController, enemyLevel);

            // PRESENTATIONS & FRAMEWORKS

            _enemyModel.Initialize(enemyLevel);
            _vfxRoot.Initialize(playerHealth, playerMana, playerPortal, enemyHealth);
            _uiRoot.Initialize(playerHealth, playerMana, enemyHealth, playerDamage, enemyDamage, playerDefence,
                enemyDefence, availableSpells, enemyLevel);
            Advertising advertising = new Advertising();
            SoundController soundController = new SoundController(_soundView);

            Localization localization = new Localization(_leanLocalization);
            localization.SetLanguageFromYandex();
            LanguageController languageController = new LanguageController(_uiRoot.LanguageView, localization);

            // INFRASTRUCTURE

            FiniteStateMachine stateMachine = new FiniteStateMachine();
            GameInitializeFsmState gameInitializeFsmState = new GameInitializeFsmState(stateMachine, advertising);
            PlayerTurnSpellFsmState playerTurnSpellFsmState =
                new PlayerTurnSpellFsmState(stateMachine, spellBarController, popUpTutorialController);
            PlayerTurnMoveFsmState playerTurnMoveFsmState = new PlayerTurnMoveFsmState(stateMachine, _diceRoller,
                _playerMovement, enemyHealth, popUpTutorialController);
            PlayerWinFsmState playerWinFsmState = new PlayerWinFsmState(stateMachine, popUpPlayerWin, levelRestarter);
            EnemyTurnFsmState enemyTurnFsmState = new EnemyTurnFsmState(stateMachine, _enemyMovement, playerHealth,
                popUpTutorialController);
            PlayerDefeatFsmState playerDefeatFsmState = new PlayerDefeatFsmState(stateMachine, popUpPlayerDefeat,
                levelRestarter);

            stateMachine.AddState(gameInitializeFsmState);
            stateMachine.AddState(playerTurnSpellFsmState);
            stateMachine.AddState(playerTurnMoveFsmState);
            stateMachine.AddState(playerWinFsmState);
            stateMachine.AddState(enemyTurnFsmState);
            stateMachine.AddState(playerDefeatFsmState);

            YandexLeaderBoard yandexLeaderBoard = new YandexLeaderBoard(enemyLevel, playerDefeatFsmState);
            LeaderboardController leaderboardController = new LeaderboardController(_uiRoot.LeaderboardButtonView,
                _uiRoot.LeaderboardPopupView, yandexLeaderBoard, _coefficients);

            SaveService saveService = new SaveService(playerDamage, playerDefence, playerHealth, playerMana,
                availableSpells, _playerMovement, enemyLevel, enemyDamage, enemyDefence, enemyHealth,
                enemyTargetController, cellsController, stateMachine, popUpTutorialController, saver);
            _saveController.Initialize(saveService, stateMachine);

            RestartGameController restartGameController =
                new RestartGameController(_uiRoot.RestartGameView, levelRestarter, stateMachine);

            // InitializeGameSounds
            playerHealth.DamageReceived += gameSounds.PlaySwordsAttack;
            playerHealth.Replenished += gameSounds.PlayHealthReplenish;
            playerMana.Replenished += gameSounds.PlayManaReplenish;

            enemyHealth.DamageReceived += gameSounds.PlaySwordsAttack;
            enemyHealth.Replenished += gameSounds.PlayHealthReplenish;

            spellActivator.SpellCast += gameSounds.PlaySpellCast;

            // _choiceEffectPresenter.Showed += _gameSounds.PlayPopUpShowed;
            // _choiceEffectPresenter.Clicked += _gameSounds.PlayClickButton;
            spellBarController.SpellSkipped += gameSounds.PlayClickButton;

            choiceSpellPresenter.Showed += gameSounds.PlayPopUpShowed;
            choiceSpellPresenter.Clicked += gameSounds.PlayClickButton;

            playerWinFsmState.Win += gameSounds.PlayPlayerWin;
            playerDefeatFsmState.Defeat += gameSounds.PlayPlayerDefeat;

            _focusTracking.FocusSwitched += value => gameSounds.SwitchByFocus(value);
            advertising.SwitchSound += value => gameSounds.SwitchByAdv(value);
            soundController.SwitchSound += value => gameSounds.SwitchByButton(value);

            _diceRoller.DiceFall += gameSounds.PlayDiceFall;

            playerJumper.Teleported += gameSounds.PlayTeleport;
            playerJumper.MadeStep += gameSounds.PlayPlayerStep;

            enemyJumper.MadeStep += gameSounds.PlayEnemyStep;

#if UNITY_WEBGL && !UNITY_EDITOR
            await _yandexLeaderBoard.Fill();
            _yandexLeaderBoard.GetCurrentPublicName();
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

        private void OnDestroy()
        {
            // это всё ради отписки от событий 
            // _soundController.Dispose();
            // _restartGameController.Dispose();
            // _popUpTutorialController.Dispose();
            // _yandexLeaderBoard.Dispose();

            // _playerHealth.DamageReceived -= _gameSounds.PlaySwordsAttack;
            // _playerHealth.Replenished -= _gameSounds.PlayHealthReplenish;
            // _playerMana.Replenished -= _gameSounds.PlayManaReplenish;
            //
            // _enemyHealth.DamageReceived -= _gameSounds.PlaySwordsAttack;
            // _enemyHealth.Replenished -= _gameSounds.PlayHealthReplenish;
            //
            // _spellActivator.SpellCast -= _gameSounds.PlaySpellCast;
            //
            // // _choiceEffectPresenter.Showed -= _gameSounds.PlayPopUpShowed;
            // // _choiceEffectPresenter.Clicked -= _gameSounds.PlayClickButton;
            // _spellBarController.SpellSkipped -= _gameSounds.PlayClickButton;
            //
            // _choiceSpellPresenter.Showed -= _gameSounds.PlayPopUpShowed;
            // _choiceSpellPresenter.Clicked -= _gameSounds.PlayClickButton;
            //
            // _playerWinFsmState.Win -= _gameSounds.PlayPlayerWin;
            // _playerDefeatFsmState.Defeat -= _gameSounds.PlayPlayerDefeat;
            //
            // // это не будет работать 
            // _focusTracking.FocusSwitched -= value => _gameSounds.SwitchByFocus(value);
            // _advertising.SwitchSound -= value => _gameSounds.SwitchByAdv(value);
            // _soundController.SwitchSound -= value => _gameSounds.SwitchByButton(value);
            //
            // _diceRoller.DiceFall -= _gameSounds.PlayDiceFall;
            //
            // _playerJumper.Teleported -= _gameSounds.PlayTeleport;
            // _playerJumper.MadeStep -= _gameSounds.PlayPlayerStep;
            //
            // _enemyJumper.MadeStep -= _gameSounds.PlayEnemyStep;
        }
    }
}