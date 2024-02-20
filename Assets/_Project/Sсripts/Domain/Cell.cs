using _Project.Sсripts.Services.Effects;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Domain
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] [Required] private Transform _centerTransform;
        [SerializeField] [Required] private SpriteRenderer _spriteRenderer;

        public EffectName EffectName { get; private set; }

        public Vector3 Center()
        {
            return _centerTransform.position;
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