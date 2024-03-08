using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "SoundSettings", menuName = "Gameplay/SoundSettings")]
    public class SoundSettings : ScriptableObject
    {
        [field: SerializeField] public SoundInfo[] Configs { get; private set; }
    }
}