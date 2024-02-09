using System.Collections.Generic;
using System.Linq;
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
        private Dictionary<EffectName, Effect> _playerEffects = new();
        private Dictionary<EffectName, Effect> _enemyEffects = new();

        private void Start()
        {
            Debug.Log("CompositeRoot started");

            // TODO: вынести числа в константы

            Player player = FindObjectOfType<Player>();
            Health playerHealth = new Health(10, 10);
            Damage playerDamage = new Damage(2);

            Enemy enemy = FindObjectOfType<Enemy>();
            Health enemyHealth = new Health(5, 5);
            Damage enemyDamage = new Damage(1);
            Vector3 enemyPosition = enemy.GetComponentInChildren<Center>().transform.position;

            DiceRoller diceRoller = FindObjectOfType<DiceRoller>();

            PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();

            List<HealthBar> healthBars = FindObjectsByType<HealthBar>(FindObjectsSortMode.None).ToList();
            List<Cell> cells = FindObjectsByType<Cell>(FindObjectsSortMode.None).ToList();
            List<CellInfo> cellInfos = Resources.LoadAll<CellInfo>("CellsInfo").ToList();

            healthBars.ForEach(healthBar =>
            {
                switch (healthBar)
                {
                    case PlayerHealthBar:
                        healthBar.Initialize(playerHealth);
                        break;

                    case EnemyHealthBar:
                        healthBar.Initialize(enemyHealth);
                        break;
                }
            });

            cells.ForEach(cell => cell.Initialized());
            diceRoller.Initialize();
            

            // наполнение эффектами

            EffectName swordsEffectName = EffectName.Swords;
            _playerEffects.Add(swordsEffectName, new Swords(enemyHealth, playerDamage, player.transform, enemyPosition));
            playerMovement.Initialize(_playerEffects, _enemyEffects);
            
            CellInfo cellInfo = cellInfos[0];
            Sprite swordsSprite = cellInfo.Sprite;
            int attackAmount = cellInfo.Amount;

            // потом в цикле сделать для каждого эффекта 
            CellsWithoutEffect(cells)
                .Shuffle()
                .Take(attackAmount)
                .ToList()
                .ForEach(cell =>
                {
                    cell.SetEffectName(swordsEffectName);
                    cell.SetSprite(swordsSprite);
                });

            // в самом конце 
            diceRoller.MakeAvailable();
        }

        private static List<Cell> CellsWithoutEffect(List<Cell> cells)
        {
            return cells.Where(cell => cell.IsEffectSet() == false).ToList();
        }
    }
}