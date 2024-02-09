using System;
using Sсripts.Model.Effects;
using Sсripts.Scriptable;
using UnityEngine;

namespace Sсripts.Model
{
    public class Cell : MonoBehaviour
    {
        private bool _isPlayerStand;
        private Center _center;
        private SpriteRenderer _spriteRenderer;

        public void Initialized()
        {
            _center = GetComponentInChildren<Center>();
            _spriteRenderer = _center.GetComponentInChildren<EffectIcon>().GetComponentInChildren<SpriteRenderer>();
        }

        public EffectName EffectName { get; private set; }

        public Vector3 Center()
        {
            return _center.transform.position;
        }

        public bool IsEffectSet()
        {
            return EffectName != EffectName.NotSet;
        }

        public void SetEffectName(EffectName effectName)
        {
            // if (effect == null)
            //     throw new NullReferenceException();

            if (IsEffectSet() == false)
                EffectName = effectName;
        }

        public void SetSprite(Sprite sprite)
        {
            if (sprite == null)
                throw new NullReferenceException();

            _spriteRenderer.sprite = sprite;
        }
    }
}