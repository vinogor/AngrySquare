using UnityEngine;

namespace Sсripts.Dmg
{
    public class Damage : MonoBehaviour
    {
        [field: SerializeField] public int Value { get; private set; }

        private void OnValidate()
        {
            if (Value < 1)
            {
                Value = 1;
            }
        }
    }
}