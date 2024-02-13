using System.Collections.Generic;
using System.Linq;
using _Project.Sсripts.Animation;
using _Project.Sсripts.Dice;
using _Project.Sсripts.Dmg;
using _Project.Sсripts.HealthAndMana;
using _Project.Sсripts.Model;
using _Project.Sсripts.Model.Effects;
using _Project.Sсripts.Movement;
using _Project.Sсripts.Scriptable;
using _Project.Sсripts.StateMachine;
using _Project.Sсripts.StateMachine.States;
using _Project.Sсripts.UI;
using _Project.Sсripts.Utility;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace _Project.Sсripts
{
    public class CompositeRoot : MonoBehaviour
    {
        // TODO: разделить на группы поля

        [SerializeField] private BaseSettings _baseSettings;

        [SerializeField] private Player _player;
        [SerializeField] private Enemy _enemy;

        [SerializeField] private HealthBar _playerHealthBar;
        [SerializeField] private ManaBar _playerManaBar;
        [SerializeField] private HealthBar _enemyHealthBar;
        [SerializeField] private DiceRoller _diceRoller;
        [SerializeField] private List<Cell> _cells;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private EnemyMovement _enemyMovement;

        [SerializeField] private DamageEffect playerDamageEffect;
        [SerializeField] private DamageEffect enemyDamageEffect;
        [SerializeField] private HealthReplenishEffect playerHealthReplenishEffect;

        [SerializeField] private EnemyAim _enemyAim;

        [SerializeField] private PopUp _popUp;

        [SerializeField] private List<CellInfo> _cellInfos;

        private Dictionary<EffectName, Effect> _playerEffects = new();
        private Dictionary<EffectName, Effect> _enemyEffects = new();

        private void Start()
        {
            Debug.Log("CompositeRoot started");

            Assert.IsNotNull(_baseSettings);
            Assert.IsNotNull(_player);
            Assert.IsNotNull(_enemy);
            Assert.IsNotNull(_playerHealthBar);
            Assert.IsNotNull(_playerManaBar);
            Assert.IsNotNull(_enemyHealthBar);
            Assert.IsNotNull(_diceRoller);
            Assert.IsNotNull(_playerMovement);
            Assert.IsNotNull(_enemyMovement);
            Assert.IsNotNull(playerDamageEffect);
            Assert.IsNotNull(enemyDamageEffect);
            Assert.IsNotNull(_enemyAim);
            Assert.IsNotNull(_popUp);
            Assert.IsNotNull(_cellInfos);

            Health playerHealth = new Health(_baseSettings.PlayerStartHealth, _baseSettings.PlayerMaxHealth);
            Damage playerDamage = new Damage(_baseSettings.PlayerDamageValue);
            Mana playerMana = new Mana(_baseSettings.PlayerStartMana, _baseSettings.PlayerMaxMana);

            Health enemyHealth = new Health(_baseSettings.EnemyStartHealth, _baseSettings.EnemyMaxHealth);
            Damage enemyDamage = new Damage(_baseSettings.EnemyDamageValue);
            Vector3 enemyPosition = _enemy.GetComponentInChildren<Center>().transform.position;

            FiniteStateMachine stateMachine = new FiniteStateMachine();
            // stateMachine.AddState(new InitializeFsmState(stateMachine));
            stateMachine.AddState(new PlayerTurnFsmState(stateMachine, _diceRoller, _playerMovement, enemyHealth));
            stateMachine.AddState(new EnemyDefeatFsmState(stateMachine, _popUp));
            stateMachine.AddState(new EnemyTurnFsmState(stateMachine, _enemyMovement, playerHealth));
            stateMachine.AddState(new PlayerDefeatFsmState(stateMachine, _popUp));
            stateMachine.AddState(new EndOfGameFsmState(stateMachine));

            _playerHealthBar.Initialize(playerHealth);
            _playerManaBar.Initialize(playerMana);
            _enemyHealthBar.Initialize(enemyHealth);

            _cells.ForEach(cell => cell.Initialized());
            _diceRoller.Initialize();

            EnemyTargetController enemyTargetController = new EnemyTargetController(_cells, _enemyAim);
            enemyTargetController.SetAimToNewRandomTargetCell();
            Cell enemyTargetCell = enemyTargetController.GetCurrentTargetCell();

            PlayerJumper playerJumper = new PlayerJumper(_player.transform, _enemy.transform, _baseSettings);
            EnemyJumper enemyJumper =
                new EnemyJumper(_enemy.transform, _playerMovement, _baseSettings, enemyTargetController);

            // === ЭФФЕКТЫ ===

            // наполнение словарей с эффектами 

            _playerEffects.Add(EffectName.Swords, new PlayerSwords(playerJumper, enemyHealth, playerDamage));
            _playerEffects.Add(EffectName.Health, new PlayerHealth(playerHealth, playerJumper));
            
            _playerMovement.Initialize(_cells, _playerEffects, _baseSettings, playerJumper);

            _enemyEffects.Add(EffectName.Swords, new EnemySwords(enemyJumper, playerHealth, enemyDamage));
            _enemyEffects.Add(EffectName.Health, new EnemyHealth(enemyJumper));
            
            _enemyMovement.Initialize(_enemyEffects, enemyTargetController, enemyJumper,
                _playerMovement, playerHealth, enemyDamage);

            // наполнение ячеек эффектами

            // TODO: добавить проверку что кол-во ячеек с эффектами равно общему кол-ву ячеек

            FillCellsWithEffects();
           

            playerDamageEffect.Initialize(playerHealth);
            enemyDamageEffect.Initialize(enemyHealth);
            playerHealthReplenishEffect.Initialize(playerHealth);

            // в самом конце 

            stateMachine.SetState<PlayerTurnFsmState>();
        }

        private void FillCellsWithEffects()
        {
            _cellInfos.ForEach(FillCells);
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