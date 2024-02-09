using System.Collections.Generic;
using System.Linq;
using Sсripts.Animation;
using Sсripts.Dice;
using Sсripts.Dmg;
using Sсripts.Hp;
using Sсripts.Model;
using Sсripts.Model.Effects;
using Sсripts.Scriptable;
using Sсripts.Utility;
using UnityEngine;

namespace Sсripts
{
    public class CompositeRoot : MonoBehaviour
    {
        // TODO: разделить на группы поля

        [SerializeField] private Player _player;
        [SerializeField] private Enemy _enemy;
        
        [SerializeField] private HealthBar _playerHealthBar;
        [SerializeField] private HealthBar _enemyHealthBar;
        [SerializeField] private DiceRoller _diceRoller;
        [SerializeField] private List<Cell> _cells = new();
        [SerializeField] private PlayerMovement _playerMovement;

        [SerializeField] private DamageTaker _playerDamageTaker;
        [SerializeField] private DamageTaker _enemyDamageTaker;
        [SerializeField] private ParticleSystem _playerDamageParticleSystem;
        [SerializeField] private ParticleSystem _enemyDamageParticleSystem;

        private Dictionary<EffectName, Effect> _playerEffects = new();
        private Dictionary<EffectName, Effect> _enemyEffects = new();

        private void Start()
        {
            Debug.Log("CompositeRoot started");

            // TODO: вынести числа в константы

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

            // наполнение эффектами
            EffectName swordsEffectName = EffectName.Swords;
            _playerEffects.Add(swordsEffectName,
                new Swords(enemyHealth, playerDamage, _player.transform, enemyPosition));
            _playerMovement.Initialize(_cells, _playerEffects, _enemyEffects);

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

            _playerDamageTaker.Initialize(playerHealth, _playerDamageParticleSystem);
            _enemyDamageTaker.Initialize(enemyHealth, _enemyDamageParticleSystem);

            // в самом конце 
            _diceRoller.MakeAvailable();
        }

        private static List<Cell> CellsWithoutEffect(List<Cell> cells)
        {
            return cells.Where(cell => cell.IsEffectSet() == false).ToList();
        }
    }
}