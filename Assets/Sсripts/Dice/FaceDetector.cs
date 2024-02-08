using System;
using UnityEngine;

namespace Sсripts.Dice
{
    public class FaceDetector : MonoBehaviour
    {
        public event Action<int> DiceNumberDetected;

        // OnTriggerEnter - в момент срабатывания скорость не нулевая, поэтому OnTriggerStay
        //                   + отключение детекторов после срабатывания

        private void OnTriggerStay(Collider other)
        {
            int number = int.Parse(name);
            DiceNumberDetected?.Invoke(number);
            Debug.Log("Invoke DiceNumberDetected = " + number);
        }
    }
}