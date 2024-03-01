using System.Collections.Generic;
using _Project.Sсripts.Config;
using _Project.Sсripts.Controllers;
using _Project.Sсripts.Controllers.PopupChoice;
using _Project.Sсripts.Controllers.Sound;
using _Project.Sсripts.Controllers.StateMachine;
using _Project.Sсripts.Controllers.StateMachine.States;
using _Project.Sсripts.Domain;
using _Project.Sсripts.Domain.Effects;
using _Project.Sсripts.Domain.Effects.Enemy;
using _Project.Sсripts.Domain.Effects.Player;
using _Project.Sсripts.Domain.Movement;
using _Project.Sсripts.Domain.Spells;
using _Project.Sсripts.Services;
using _Project.Sсripts.View;
using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts._Root
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
        [SerializeField] [Required] private SaveController _saveController;

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

        private async void Start()
        {
            Debug.LogError("CompositeRoot started");
            
            Debug.LogError("YandexGamesSdk.GameReady() - STARTED");
            YandexGamesSdk.GameReady();

            Assert.AreEqual(16, _cells.Length);
            Assert.AreEqual(3, _enemyAims.Length);

            GameSounds gameSounds = new GameSounds(_soundSettings, _audioSource);

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
            EnemyLevel enemyLevel = new EnemyLevel();
            _enemyModel.Initialize(enemyLevel);

            AvailableSpells availableSpells = new AvailableSpells();

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
                new PopUpNotificationModel("Player Win", "It's time to fight a new opponent!"));
            PopUpNotificationController popUpPlayerDefeat = new PopUpNotificationController(
                _uiRoot.PopUpNotificationView,
                new PopUpNotificationModel("Player Lose", "You lost the game! Press 'OK' to restart."));

            EnemyTargetController enemyTargetController = new EnemyTargetController(cellsManager, _enemyAims);

            LevelRestarter levelRestarter = new LevelRestarter(cellsManager, playerDefence, playerHealth,
                playerDamage, playerMana, enemyDefence, enemyHealth, enemyDamage, availableSpells, _playerMovement,
                enemyTargetController, enemyLevel);

            // === STATE MACHINE ===
            FiniteStateMachine stateMachine = new FiniteStateMachine();
            // stateMachine.AddState(new RestartFsmState(stateMachine, levelRestarter));
            stateMachine.AddState(new PlayerTurnMoveFsmState(stateMachine, _diceRoller, _playerMovement, enemyHealth));
            stateMachine.AddState(new PlayerTurnSpellFsmState(stateMachine, spellBarController));
            stateMachine.AddState(new PlayerWinFsmState(stateMachine, popUpPlayerWin, levelRestarter, gameSounds));
            stateMachine.AddState(new EnemyTurnFsmState(stateMachine, _enemyMovement, playerHealth));
            stateMachine.AddState(new PlayerDefeatFsmState(stateMachine, popUpPlayerDefeat, levelRestarter,
                gameSounds));
            // stateMachine.AddState(new EndOfGameFsmState(stateMachine));

            _diceRoller.Initialize(gameSounds);

            enemyTargetController.SetAimToNewRandomTargetCell();

            PlayerJumper playerJumper =
                new PlayerJumper(_player.transform, _enemyModel.transform, _coefficients, gameSounds);
            EnemyJumper enemyJumper =
                new EnemyJumper(_enemyModel.transform, _playerMovement, _coefficients, enemyTargetController,
                    gameSounds);

            // === ЭФФЕКТЫ ЯЧЕЕК ===

            cellsManager.FillWithEffects();

            Cell[] portalCells = cellsManager.Find(EffectName.Portal);
            PlayerPortal playerPortal = new PlayerPortal(playerJumper, portalCells, _playerMovement, gameSounds);

            _vfxRoot.Initialize(playerHealth, playerMana, playerPortal, enemyHealth);

            CellEffectsInitialize(playerJumper, enemyHealth, playerDamage, playerHealth, playerMana, playerPortal,
                enemyJumper, enemyDamage, enemyTargetController, choiceEffectController, choiceSpellController,
                cellsManager);

            // === SAVE ===

            SaveService saveService = new SaveService(playerDamage, playerDefence, playerHealth, playerMana,
                availableSpells, _playerMovement, enemyLevel, enemyDamage, enemyDefence, enemyHealth,
                enemyTargetController, cellsManager, stateMachine);
            _saveController.Initialize(saveService, stateMachine);

            // в самом конце

            saveService.Load();
            await UniTask.WaitUntil(() => saveService.LoadComplete);

            if (saveService.IsSaveExist == false)
                stateMachine.SetState<PlayerTurnSpellFsmState>();

#if UNITY_WEBGL && !UNITY_EDITOR
            YandexAuthorize();
#endif
        }

        private void YandexAuthorize()
        {
            // Debug.LogError("YandexGamesSdk.GameReady() - STARTED");
            // YandexGamesSdk.GameReady();

            Debug.LogError("PlayerAccount.Authorize() - STARTED");
            PlayerAccount.Authorize();

            if (PlayerAccount.IsAuthorized)
            {
                Debug.LogError("PlayerAccount.RequestPersonalProfileDataPermission() - STARTED");
                PlayerAccount.RequestPersonalProfileDataPermission();
            }

            if (PlayerAccount.IsAuthorized == false)
            {
                Debug.LogError("PlayerAccount.IsAuthorized == false");
            }
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
        }
    }
}