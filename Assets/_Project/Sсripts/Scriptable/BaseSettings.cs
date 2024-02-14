using UnityEngine;

namespace _Project.Sсripts.Scriptable
{
    [CreateAssetMenu(fileName = "BaseSettings", menuName = "Gameplay/BaseSettings")]
    public class BaseSettings : ScriptableObject
    {
        [Header("Common")]
        [field: SerializeField] public float JumpPower = 1f;
        [field: SerializeField] public float JumpDuration = 0.5f;
        [field: SerializeField] public float AnimationCellDuration = 0.2f;
        [field: SerializeField] public float EnemyHeight = 0.5f; // пока это радиус сферы

        [Space(10)]
        [Header("Player")] 
        [field: SerializeField] public int PlayerStartHealth = 10;
        [field: SerializeField] public int PlayerMaxHealth = 10;
        [field: SerializeField] public int PlayerStartMana = 2;
        [field: SerializeField] public int PlayerMaxMana = 10;
        [field: SerializeField] public int PlayerStartDamage = 2;
        [field: SerializeField] public int PlayerStartDefence = 0;
        
        [Space(10)]
        [Header("Player")] 
        [field: SerializeField] public int EnemyStartHealth = 5;
        [field: SerializeField] public int EnemyMaxHealth = 5;
        [field: SerializeField] public int EnemyStartDamage = 1;
        [field: SerializeField] public int EnemyStartDefence = 0;
    }
}