using System.Collections.Generic;
using System.Linq;
using _Project.Sсripts.Animation;
using _Project.Sсripts.Dice;
using _Project.Sсripts.Dmg;
using _Project.Sсripts.Hp;
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

namespace _Project.Sсripts
{
    public class CompositeRoot : MonoBehaviour
    {
        // TODO: разделить на группы поля

        [SerializeField] private BaseSettings _baseSettings;

        [SerializeField] private Player _player;
        [SerializeField] private Enemy _enemy;

        [SerializeField] private HealthBar _playerHealthBar;
        [SerializeField] private HealthBar _enemyHealthBar;
        [SerializeField] private DiceRoller _diceRoller;
        [SerializeField] private List<Cell> _cells = new();
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private EnemyMovement _enemyMovement;

        [SerializeField] private DamageTaker _playerDamageTaker;
        [SerializeField] private DamageTaker _enemyDamageTaker;

        [SerializeField] private EnemyAim _enemyAim;

        [SerializeField] private PopUp _popUp;

        private Dictionary<EffectName, Effect> _playerEffects = new();
        private Dictionary<EffectName, Effect> _enemyEffects = new();

        private void Start()
        {
            Debug.Log("CompositeRoot started");

            Assert.IsNotNull(_baseSettings);
            Assert.IsNotNull(_player);
            Assert.IsNotNull(_enemy);
            Assert.IsNotNull(_playerHealthBar);
            Assert.IsNotNull(_enemyHealthBar);
            Assert.IsNotNull(_diceRoller);
            Assert.IsNotNull(_playerMovement);
            Assert.IsNotNull(_enemyMovement);
            Assert.IsNotNull(_playerDamageTaker);
            Assert.IsNotNull(_enemyDamageTaker);
            Assert.IsNotNull(_enemyAim);
            Assert.IsNotNull(_popUp);

            Health playerHealth = new Health(_baseSettings.PlayerStartHealth, _baseSettings.PlayerMaxHealth);
            Damage playerDamage = new Damage(_baseSettings.PlayerDamageValue);

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
            _enemyHealthBar.Initialize(enemyHealth);

            List<CellInfo> cellInfos = Resources.LoadAll<CellInfo>("CellsInfo").ToList();

            _cells.ForEach(cell => cell.Initialized());
            _diceRoller.Initialize();

            EnemyTargetController enemyTargetController = new EnemyTargetController(_cells, _enemyAim);
            enemyTargetController.SetAimToNewRandomTargetCell();
            Cell enemyTargetCell = enemyTargetController.GetCurrentTargetCell();

            // наполнение эффектами

            PlayerJumper playerJumper = new PlayerJumper(_player.transform, _enemy.transform, _baseSettings);
            EnemyJumper enemyJumper =
                new EnemyJumper(_enemy.transform, _playerMovement, _baseSettings, enemyTargetController);

            EffectName swordsEffectName = EffectName.Swords;

            PlayerSwords playerSwords = new PlayerSwords(playerJumper, enemyHealth, playerDamage);
            _playerEffects.Add(swordsEffectName, playerSwords);
            _playerMovement.Initialize(_cells, _playerEffects, _baseSettings, playerJumper);

            EnemySwords enemySwords = new EnemySwords(enemyJumper, playerHealth, enemyDamage);
            _enemyEffects.Add(swordsEffectName, enemySwords);
            _enemyMovement.Initialize(enemyTargetCell, _enemyEffects, enemyTargetController,
                enemyJumper, _playerMovement, playerHealth, enemyDamage);

            CellInfo cellInfo = cellInfos[0];
            Sprite swordsSprite = cellInfo.Sprite;
            int attackAmount = cellInfo.Amount;

            // потом в цикле сделать для каждого эффекта 
            CellsWithoutEffect(_cells)
                .Shuffle()
                .Take(attackAmount)
                .ToList()
                .ForEach(cell =>
                {
                    cell.SetEffectName(swordsEffectName);
                    cell.SetSprite(swordsSprite);
                });

            _playerDamageTaker.Initialize(playerHealth);
            _enemyDamageTaker.Initialize(enemyHealth);

            // в самом конце 

            stateMachine.SetState<PlayerTurnFsmState>();
        }

        private static List<Cell> CellsWithoutEffect(List<Cell> cells)
        {
            return cells.Where(cell => cell.IsEffectSet() == false).ToList();
        }
    }
}