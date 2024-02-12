using System;
using _Project.Sсripts.Model.Effects;
using _Project.Sсripts.Scriptable;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Model
{
    public class Cell : MonoBehaviour
    {
        private bool _isPlayerStand;
        private Center _center;
        private SpriteRenderer _spriteRenderer;

        public void Initialized()
        {
            _center = GetComponentInChildren<Center>();
            Assert.IsNotNull(_center);
            _spriteRenderer = _center.GetComponentInChildren<EffectIcon>().GetComponentInChildren<SpriteRenderer>();
            Assert.IsNotNull(_spriteRenderer);
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
            if (IsEffectSet() == false)
                EffectName = effectName;
        }

        public void SetSprite(Sprite sprite)
        {
            Assert.IsNotNull(sprite);

            _spriteRenderer.sprite = sprite;
        }
    }
}