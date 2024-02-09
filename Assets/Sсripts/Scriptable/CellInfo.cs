using UnityEngine;

namespace Sсripts.Scriptable
{
    [CreateAssetMenu(fileName = "CellInfo", menuName = "Gameplay/CellInfo")]
    public class CellInfo : ScriptableObject
    {
        [field: SerializeField] public EffectName EffectName { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public int Amount { get; private set; }
    }

    public enum EffectName
    {
        NotSet,
        Swords,
        Book,
        Portal,
        Question,
        Drop,
        Plus
    }
}