using System;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private bool _isPlayerStand;
    private Effect _effect;
    private CenterCell _center;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _center = GetComponentInChildren<CenterCell>();
        _spriteRenderer = _center.GetComponentInChildren<EffectIcon>().GetComponentInChildren<SpriteRenderer>();
    }

    public Vector3 Center()
    {
        return _center.transform.position;
    }

    public bool IsEffectSet()
    {
        return _effect != null;
    }

    public void SetEffect(Effect effect)
    {
        // if (effect == null)
        //     throw new NullReferenceException();

        if (IsEffectSet() == false)
            _effect = effect;
    }

    public void SetSprite(Sprite sprite)
    {
        if (sprite == null)
            throw new NullReferenceException();

        _spriteRenderer.sprite = sprite;
    }

    public void ActivateEffect()
    {
        if (_effect == null)
        {
            // throw new NullReferenceException();
            Debug.Log("try to activate effect, but _effect == null");
            return;
        }

        _effect.Activate();
    }
}