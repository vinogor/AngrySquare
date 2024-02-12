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

        [SerializeField] private EnemyAim enemyAim;

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
            Assert.IsNotNull(enemyAim);

            // TODO: вынести числа в константы

            FiniteStateMachine stateMachine = new FiniteStateMachine();
            // stateMachine.AddState(new InitializeFsmState(stateMachine));
            stateMachine.AddState(new PlayerTurnFsmState(stateMachine, _diceRoller, _playerMovement));
            // stateMachine.AddState(new EnemyDefeatFsmState(stateMachine));
            stateMachine.AddState(new EnemyTurnFsmState(stateMachine, _enemyMovement));
            // stateMachine.AddState(new PlayerDefeatFsmState(stateMachine));
            stateMachine.AddState(new EndOfGameFsmState(stateMachine));

            Health playerHealth = new Health(10, 10);
            Damage playerDamage = new Damage(2);

            Health enemyHealth = new Health(5, 5);
            Damage enemyDamage = new Damage(1);
            Vector3 enemyPosition = _enemy.GetComponentInChildren<Center>().transform.position;

            _playerHealthBar.Initialize(playerHealth);
            _enemyHealthBar.Initialize(enemyHealth);

            List<CellInfo> cellInfos = Resources.LoadAll<CellInfo>("CellsInfo").ToList();

            _cells.ForEach(cell => cell.Initialized());
            _diceRoller.Initialize();

            EnemyAimToCellMover enemyAimToCellMover = new EnemyAimToCellMover(_cells, enemyAim);
            Cell cellForAim = enemyAimToCellMover.SetToNewRandomCell();

            // наполнение эффектами
            EffectName swordsEffectName = EffectName.Swords;
            _playerEffects.Add(swordsEffectName,
                new PlayerSwords(enemyHealth, playerDamage, _player.transform, enemyPosition, _baseSettings));
            _playerMovement.Initialize(_cells, _playerEffects, _baseSettings);

            EnemySwords enemySwords = new EnemySwords(_enemy.transform, cellForAim, playerHealth, enemyDamage,
                _playerMovement, _baseSettings);
            enemySwords.Initialize();
            _enemyEffects.Add(swordsEffectName, enemySwords);
            _enemyMovement.Initialize(cellForAim, _enemyEffects, enemyAimToCellMover);

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