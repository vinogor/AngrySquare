using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Sсripts.Animation;
using _Project.Sсripts.DamageAndDefence;
using _Project.Sсripts.Dice;
using _Project.Sсripts.HealthAndMana;
using _Project.Sсripts.Model;
using _Project.Sсripts.Model.Effects.Player;
using _Project.Sсripts.Scriptable;
using _Project.Sсripts.StateMachine.States;
using _Project.Sсripts.UI.PopupChoice;
using _Project.Sсripts.UI.SpellCast;
using _Project.Sсripts.Utility;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts
{
    public class CompositeRoot : MonoBehaviour
    {
        [Header("Common")]
        [SerializeField] [Required] private Coefficients _coefficients;
        [SerializeField] [Required] private DiceRoller _diceRoller;
        [SerializeField] private Cell[] _cells;
        [SerializeField] [Required] private CellsSettings _cellsSettings;
        [SerializeField] [Required] private SpellsSettings _spellsSettings;
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

            Assert.AreEqual(16, _cells.Length);

            Defence playerDefence = new Defence(_coefficients.PlayerStartDefence);
            Health playerHealth = new Health(_coefficients.PlayerStartHealth, _coefficients.PlayerMaxHealth,
                playerDefence);
            Damage playerDamage = new Damage(_coefficients.PlayerStartDamage);
            Mana playerMana = new Mana(_coefficients.PlayerStartMana, _coefficients.PlayerMaxMana, _spellsSettings);

            Defence enemyDefence = new Defence(_coefficients.EnemyStartDefence);
            Health enemyHealth = new Health(_coefficients.EnemyStartHealth, _coefficients.EnemyMaxHealth, enemyDefence);
            Damage enemyDamage = new Damage(_coefficients.EnemyStartDamage);

            Spells spells = new Spells();

            Dictionary<SpellName, Spell> playerSpells = new Dictionary<SpellName, Spell>();
            playerSpells.Add(SpellName.FullHealth, new FullHealthSpell(playerHealth));
            playerSpells.Add(SpellName.UpDamage, new UpDamageSpell(playerDamage, _coefficients));
            playerSpells.Add(SpellName.UpMaxHealth, new UpMaxHealthSpell(playerHealth, _coefficients));
            playerSpells.Add(SpellName.UpDefence, new UpDefenceSpell(playerDefence, _coefficients));
            playerSpells.Add(SpellName.UpMaxMana, new UpMaxManaSpell(playerMana, _coefficients));

            SpellActivator spellActivator = new SpellActivator(playerSpells);

            // === UI ===
            _uiRoot.Initialize(playerHealth, playerMana, enemyHealth, playerDamage, enemyDamage, playerDefence,
                enemyDefence, spells);
            List<EffectName> availableEffectNames = new List<EffectName>
                { EffectName.Swords, EffectName.Health, EffectName.Mana };
            List<SpellName> availableSpellNames = new List<SpellName>
            {
                SpellName.FullHealth, SpellName.UpDamage, SpellName.UpDefence, SpellName.UpMaxHealth,
                SpellName.UpMaxMana
            };

            PopUpChoiceEffectController choiceEffectController =
                new PopUpChoiceEffectController(_uiRoot.PopUpChoiceView, availableEffectNames, _playerMovement,
                    _cellsSettings);

            SpellBarController spellBarController =
                new SpellBarController(spells, _uiRoot.SpellBarView, playerMana, spellActivator);

            PopUpChoiceSpellController choiceSpellController =
                new PopUpChoiceSpellController(_uiRoot.PopUpChoiceView, availableSpellNames,
                    spellBarController, _spellsSettings);

            // === STATE MACHINE ===
            FiniteStateMachine stateMachine = new FiniteStateMachine();
            // stateMachine.AddState(new InitializeFsmState(stateMachine));
            stateMachine.AddState(new PlayerTurnMoveFsmState(stateMachine, _diceRoller, _playerMovement, enemyHealth));
            stateMachine.AddState(new PlayerTurnSpellFsmState(stateMachine, spellBarController));
            stateMachine.AddState(new PlayerWinFsmState(stateMachine, _uiRoot.PopUpPlayerWinNotificationController));
            stateMachine.AddState(new EnemyTurnFsmState(stateMachine, _enemyMovement, playerHealth));
            stateMachine.AddState(new PlayerDefeatFsmState(stateMachine,
                _uiRoot.PopPlayerDefeatNotificationController));
            stateMachine.AddState(new EndOfGameFsmState(stateMachine));

            _diceRoller.Initialize();

            EnemyTargetController enemyTargetController = new EnemyTargetController(_cells, _enemyAim);
            enemyTargetController.SetAimToNewRandomTargetCell();

            PlayerJumper playerJumper = new PlayerJumper(_player.transform, _enemy.transform, _coefficients);
            EnemyJumper enemyJumper =
                new EnemyJumper(_enemy.transform, _playerMovement, _coefficients, enemyTargetController);

            // === ЭФФЕКТЫ ===

            FillCellsWithEffects();

            Cell[] portalCells = FindCellsByEffectName(EffectName.Portal);
            PlayerPortal playerPortal = new PlayerPortal(playerJumper, portalCells, _playerMovement);

            _vfxRoot.Initialize(playerHealth, playerMana, playerPortal, enemyHealth);

            CellEffectsInitialize(playerJumper, enemyHealth, playerDamage, playerHealth, playerMana, playerPortal,
                enemyJumper, enemyDamage, enemyTargetController, choiceEffectController, choiceSpellController);

            // в самом конце 
            stateMachine.SetState<PlayerTurnSpellFsmState>();
        }

        private void CellEffectsInitialize(PlayerJumper playerJumper, Health enemyHealth, Damage playerDamage,
            Health playerHealth, Mana playerMana, PlayerPortal playerPortal, EnemyJumper enemyJumper,
            Damage enemyDamage, EnemyTargetController enemyTargetController,
            PopUpChoiceEffectController choiceEffectController, PopUpChoiceSpellController choiceSpellController)
        {
            _playerEffects.Add(EffectName.Swords, new PlayerSwords(playerJumper, enemyHealth, playerDamage));
            _playerEffects.Add(EffectName.Health, new PlayerHealth(playerHealth, playerJumper));
            _playerEffects.Add(EffectName.Mana, new PlayerMana(playerMana, playerJumper));
            _playerEffects.Add(EffectName.Portal, playerPortal);
            _playerEffects.Add(EffectName.Question, new PlayerQuestion(playerJumper, choiceEffectController));
            _playerEffects.Add(EffectName.SpellBook, new PlayerSpellBook(playerJumper, choiceSpellController));

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
            Array.ForEach(_cellsSettings.CellInfos, FillCells);
        }

        private Cell[] FindCellsByEffectName(EffectName effectName)
        {
            return _cells.Where(cell => cell.EffectName == effectName).ToArray();
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

        private Cell[] CellsWithoutEffect(Cell[] cells)
        {
            return cells.Where(cell => cell.IsEffectSet() == false).ToArray();
        }
    }
}