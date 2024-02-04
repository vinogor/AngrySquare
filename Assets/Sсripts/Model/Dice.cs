using UnityEngine;

public class Dice : MonoBehaviour
{
    public int GetRandomNumber()
    {
        return Random.Range(0, 7);
    }
}