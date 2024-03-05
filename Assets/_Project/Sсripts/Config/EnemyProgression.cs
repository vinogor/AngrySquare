using UnityEngine;

namespace _Project.Config
{
    [CreateAssetMenu(fileName = "EnemyProgression", menuName = "Gameplay/EnemyProgression")]
    public class EnemyProgression : ScriptableObject
    {
        [SerializeField] private AnimationCurve _health;
        [SerializeField] private AnimationCurve _defence;
        [SerializeField] private AnimationCurve _damage;

        public int GetHealth(int forLevel) => (int)_health.Evaluate(forLevel);
        public int GetDefence(int forLevel) => (int)_defence.Evaluate(forLevel);
        public int GetDamage(int forLevel) => (int)_damage.Evaluate(forLevel);
    }
}