using System;
using UnityEngine;

namespace Sсripts.Dice
{
    public class FaceDetector : MonoBehaviour
    {
        public event Action<int> DiceNumberDetected;

        private void OnTriggerEnter(Collider other)
        {
            int number = int.Parse(name);
            DiceNumberDetected?.Invoke(number);
            Debug.Log("Invoke DiceNumberDetected = " + number);
        }
    }
}