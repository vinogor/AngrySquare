using System;
using UnityEngine;

namespace Services
{
    public class DiceFaceDetector : MonoBehaviour
    {
        public event Action<int> DiceNumberDetected;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("FloorForDice"))
            {
                int number = int.Parse(name);
                DiceNumberDetected?.Invoke(number);
            }  
        }
    }
}