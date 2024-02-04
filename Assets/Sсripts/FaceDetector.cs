using System;
using UnityEngine;

public class FaceDetector : MonoBehaviour
{
    public event Action<int> DiceNumberSet;

    private DiceRoller _diceRoller;

    private void Awake()
    {
        _diceRoller = GetComponentInParent<DiceRoller>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (_diceRoller != null)
        {
            if (_diceRoller.GetComponent<Rigidbody>().velocity == Vector3.zero)
            {
                int number = int.Parse(name);
                Debug.Log("Invoke DiceNumberSet = " + number);
                DiceNumberSet?.Invoke(number);
            }
        }
    }
}