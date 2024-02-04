using UnityEngine;

public class Cell : MonoBehaviour
{
    private bool _isPlayerStand;
    private Effect _effect;
    private CenterCell _center;

    private void Awake()
    {
        _center = GetComponentInChildren<CenterCell>();
    }

    public Vector3 Center()
    {
        return _center.transform.position;
    }
}