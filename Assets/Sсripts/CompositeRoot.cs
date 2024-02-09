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
        private void Start()
        {
            Debug.Log("CompositeRoot started");

            Player player = FindObjectOfType<Player>();
            Health playerHealth = new Health(10, 10);
            Damage playerDamage = player.GetComponent<Damage>();

            Enemy enemy = FindObjectOfType<Enemy>();
            Health enemyHealth = new Health(5, 5);
            Damage enemyDamage = enemy.GetComponent<Damage>();
            Vector3 enemyPosition = enemy.GetComponentInChildren<Center>().transform.position;

            DiceRoller diceRoller = FindObjectOfType<DiceRoller>();

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
            CellInfo cellInfo = cellInfos[0];

            Effect effect = null;

            if (cellInfo.EffectName == EffectName.Swords)
            {
                effect = new Swords(enemyHealth, playerDamage, player.transform, enemyPosition);
            }

            Sprite attackSprite = cellInfo.Sprite;
            int attackAmount = cellInfo.Amount;

            // потом в цикле сделать для каждого эффекта 
            CellsWithoutEffect(cells)
                .Shuffle()
                .Take(attackAmount)
                .ToList()
                .ForEach(cell =>
                {
                    cell.SetEffect(effect);
                    cell.SetSprite(attackSprite);
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