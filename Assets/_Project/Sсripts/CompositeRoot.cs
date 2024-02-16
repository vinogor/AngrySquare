using System.Collections.Generic;
using System.Linq;
using _Project.Sсripts.Animation;
using _Project.Sсripts.DamageAndDefence;
using _Project.Sсripts.Dice;
using _Project.Sсripts.HealthAndMana;
using _Project.Sсripts.Model;
using _Project.Sсripts.Model.Effects;
using _Project.Sсripts.Model.Effects.Enemy;
using _Project.Sсripts.Model.Effects.Player;
using _Project.Sсripts.Movement;
using _Project.Sсripts.Scriptable;
using _Project.Sсripts.StateMachine;
using _Project.Sсripts.StateMachine.States;
using _Project.Sсripts.UI;
using _Project.Sсripts.Utility;
using NaughtyAttributes;
using UnityEngine;

namespace _Project.Sсripts
{
    public class CompositeRoot : MonoBehaviour
    {
        [Header("Common")]
        [SerializeField] [Required] private Coefficients _coefficients;
        [SerializeField] [Required] private DiceRoller _diceRoller;
        [SerializeField] private List<Cell> _cells;
        [SerializeField] [Required] private CellsAndSpellsSettings _cellsAndSpellsSettings;
        [SerializeField] [Required] private UiRoot _uiRoot;
        [SerializeField] [Required] private VfxRoot _vfxRoot;

        [Space(10)]
        [Header("Player")]
        [SerializeField] [Required] private Player _player;
        [SerializeField] [Required] private PlayerMovement _playerMovement;

        [Space(10)]
        [Header("Enemy")]
        [SerializeField] [Required] private Enemy _enemy;
        [SerializeField] [Required] private EnemyMovement _enemyMovement;
        [SerializeField] [Required] private EnemyAim _enemyAim;

        private readonly Dictionary<EffectName, Effect> _playerEffects = new();
        private readonly Dictionary<EffectName, Effect> _enemyEffects = new();

        private void Start()
        {
            Debug.Log("CompositeRoot started");

            Defence playerDefence = new Defence(_coefficients.PlayerStartDefence);
            Health playerHealth = new Health(_coefficients.PlayerStartHealth, _coefficients.PlayerMaxHealth,
                playerDefence);
            Damage playerDamage = new Damage(_coefficients.PlayerStartDamage);
            Mana playerMana = new Mana(_coefficients.PlayerStartMana, _coefficients.PlayerMaxMana);

            Defence enemyDefence = new Defence(_coefficients.EnemyStartDefence);
            Health enemyHealth = new Health(_coefficients.EnemyStartHealth, _coefficients.EnemyMaxHealth, enemyDefence);
            Damage enemyDamage = new Damage(_coefficients.EnemyStartDamage);

            FiniteStateMachine stateMachine = new FiniteStateMachine();

            // TODO: перекрёстная ссылка!
            PopUpWinDefeatController popUpWinDefeatController =
                new PopUpWinDefeatController(_uiRoot.PopUpWinDefeat, stateMachine);

            List<EffectName> availableEffectNames = new()
            {
                EffectName.Swords,
                EffectName.Health,
                EffectName.Mana
            };
            PopUpQuestionController popUpQuestionController =
                new PopUpQuestionController(_uiRoot.PopUpQuestion, _cellsAndSpellsSettings, availableEffectNames,
                    _playerMovement);

            // stateMachine.AddState(new InitializeFsmState(stateMachine));
            stateMachine.AddState(new PlayerTurnFsmState(stateMachine, _diceRoller, _playerMovement, enemyHealth));
            stateMachine.AddState(new EnemyDefeatFsmState(stateMachine, popUpWinDefeatController));
            stateMachine.AddState(new EnemyTurnFsmState(stateMachine, _enemyMovement, playerHealth));
            stateMachine.AddState(new PlayerDefeatFsmState(stateMachine, popUpWinDefeatController));
            stateMachine.AddState(new EndOfGameFsmState(stateMachine));

            _cells.ForEach(cell => cell.Initialized());
            _diceRoller.Initialize();

            EnemyTargetController enemyTargetController = new EnemyTargetController(_cells, _enemyAim);
            enemyTargetController.SetAimToNewRandomTargetCell();

            PlayerJumper playerJumper = new PlayerJumper(_player.transform, _enemy.transform, _coefficients);
            EnemyJumper enemyJumper =
                new EnemyJumper(_enemy.transform, _playerMovement, _coefficients, enemyTargetController);

            // === UI ===
            _uiRoot.Initialize(playerHealth, playerMana, enemyHealth, playerDamage, enemyDamage, playerDefence,
                enemyDefence);

            // === ЭФФЕКТЫ ===

            FillCellsWithEffects();

            List<Cell> portalCells = FindCellsByEffectName(EffectName.Portal);
            PlayerPortal playerPortal = new PlayerPortal(playerJumper, portalCells, _playerMovement);

            _vfxRoot.Initialize(playerHealth, playerMana, playerPortal, enemyHealth);

            CellEffectsInitialize(playerJumper, enemyHealth, playerDamage, playerHealth, playerMana, playerPortal,
                enemyJumper, enemyDamage, enemyTargetController, popUpQuestionController);

            // в самом конце 
            stateMachine.SetState<PlayerTurnFsmState>();
        }

        private void CellEffectsInitialize(PlayerJumper playerJumper, Health enemyHealth, Damage playerDamage,
            Health playerHealth, Mana playerMana, PlayerPortal playerPortal, EnemyJumper enemyJumper,
            Damage enemyDamage, EnemyTargetController enemyTargetController,
            PopUpQuestionController popUpQuestionController)
        {
            _playerEffects.Add(EffectName.Swords, new PlayerSwords(playerJumper, enemyHealth, playerDamage));
            _playerEffects.Add(EffectName.Health, new PlayerHealth(playerHealth, playerJumper));
            _playerEffects.Add(EffectName.Mana, new PlayerMana(playerMana, playerJumper));
            _playerEffects.Add(EffectName.Portal, playerPortal);
            _playerEffects.Add(EffectName.Question, new PlayerQuestion(playerJumper, popUpQuestionController));
            _playerEffects.Add(EffectName.SpellBook, new PlayerSpellBook(playerJumper));

            _playerMovement.Initialize(_cells, _playerEffects, _coefficients, playerJumper);

            _enemyEffects.Add(EffectName.Swords, new EnemySwords(enemyJumper, playerHealth, enemyDamage));
            _enemyEffects.Add(EffectName.Health, new EnemyHealth(enemyJumper));
            _enemyEffects.Add(EffectName.Mana, new EnemyMana(enemyJumper));
            _enemyEffects.Add(EffectName.Portal, new EnemyPortal(enemyJumper));
            _enemyEffects.Add(EffectName.Question, new EnemyQuestion(enemyJumper));
            _enemyEffects.Add(EffectName.SpellBook, new EnemySpellBook(enemyJumper));

            _enemyMovement.Initialize(_enemyEffects, enemyTargetController, enemyJumper,
                _playerMovement, playerHealth, enemyDamage);
        }

        private void FillCellsWithEffects()
        {
            _cellsAndSpellsSettings.CellInfos.ForEach(FillCells);
        }

        private List<Cell> FindCellsByEffectName(EffectName effectName)
        {
            return _cells.Where(cell => cell.EffectName == effectName).ToList();
        }

        private void FillCells(CellInfo cellInfo)
        {
            EffectName effectName = cellInfo.EffectName;
            Sprite sprite = cellInfo.Sprite;
            int amount = cellInfo.Amount;

            var cellsWithoutEffect = CellsWithoutEffect(_cells);

            cellsWithoutEffect
                .Shuffle()
                .Take(amount)
                .ToList()
                .ForEach(cell =>
                {
                    cell.SetEffectName(effectName);
                    cell.SetSprite(sprite);
                });
        }

        private List<Cell> CellsWithoutEffect(List<Cell> cells)
        {
            return cells.Where(cell => cell.IsEffectSet() == false).ToList();
        }
    }
}