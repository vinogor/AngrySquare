using UnityEngine;

namespace _Project.Config
{
    [CreateAssetMenu(fileName = "Coefficients", menuName = "Gameplay/Coefficients")]
    public class Coefficients : ScriptableObject
    {
        [Header("Common")]
        [field: SerializeField] public float JumpPower = 1f;
        [field: SerializeField] public float JumpDuration = 0.5f;
        [field: SerializeField] public float AnimationCellDuration = 0.2f;
        [field: SerializeField] public float EnemyHeight = 0.5f;

        [Space(10)]
        [Header("Player")]
        [field: SerializeField] public int PlayerStartHealth = 10;
        [field: SerializeField] public int PlayerMaxHealth = 10;
        [field: SerializeField] public int PlayerStartMana = 2;
        [field: SerializeField] public int PlayerMaxMana = 10;
        [field: SerializeField] public int PlayerStartDamage = 2;
        [field: SerializeField] public int PlayerStartDefence = 0;

        [Space(10)]
        [Header("Players Spells")]
        [field: SerializeField] public int DamageIncreaseValue = 1;
        [field: SerializeField] public int DefenceIncreaseValue = 1;
        [field: SerializeField] public int MaxHealthIncreaseValue = 1;
        [field: SerializeField] public int MaxManaIncreaseValue = 1;

        [field: SerializeField] public float DelayAfterVfxSeconds = 1.5f;

        // TODO: наверное этот блок больше не нужен 
        [Space(10)]
        [Header("Enemy")]
        [field: SerializeField] public int EnemyStartHealth = 5;
        [field: SerializeField] public int EnemyMaxHealth = 5;
        [field: SerializeField] public int EnemyStartDamage = 1;
        [field: SerializeField] public int EnemyStartDefence = 0;
    }
}