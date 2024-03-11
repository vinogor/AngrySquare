using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace View
{
    public class SpellBarFameScaler : MonoBehaviour
    {
        [SerializeField] private Transform _frameTransform;

        private const float Scale = 1.04f;
        private const float ScalingDuration = 0.5f;
        private TweenerCore<Vector3, Vector3, VectorOptions> _frameTweener;

        public void Enable()
        {
            _frameTransform.gameObject.SetActive(true);
            _frameTransform.DOScale(new Vector3(Scale, Scale, Scale), ScalingDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

        public void Disable()
        {
            _frameTransform.gameObject.SetActive(false);
        }
    }
}