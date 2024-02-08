using System;
using Sсripts.Model.Effects;
using UnityEngine;

namespace Sсripts.Model
{
    public class Cell : MonoBehaviour
    {
        private bool _isPlayerStand;
        private Effect _effect;
        private Center _center;
        private SpriteRenderer _spriteRenderer;

    
        public void Initialized()
        {
            _center = GetComponentInChildren<Center>();
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
}