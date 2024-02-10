using UnityEngine;

namespace _Project.Sсripts.Scriptable
{
    [CreateAssetMenu(fileName = "BaseSettings", menuName = "Gameplay/BaseSettings")]
    public class BaseSettings : ScriptableObject
    {
        [field: SerializeField] public float JumpPower = 1f;
        [field: SerializeField] public float JumpDuration = 0.5f;
        
        [field: SerializeField] public float AnimationCellDuration = 0.2f;

        [field: SerializeField] public float EnemyHeight = 0.5f; // пока это радиус сферы
    }
}