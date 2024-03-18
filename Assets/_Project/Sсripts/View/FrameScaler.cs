using DG.Tweening;
using UnityEngine;

namespace View
{
    public class FrameScaler : MonoBehaviour
    {
        [SerializeField] private Transform _frameTransform;

        private const float Scale = 1.04f;
        private const float ScalingDuration = 0.5f;
        private Sequence _sequence;

        public void Enable()
        {
            Disable();
            _sequence = DOTween.Sequence()
                .Append(_frameTransform.DOScale(new Vector3(Scale, Scale, Scale), ScalingDuration)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine)
                )
                .Play();

            _frameTransform.gameObject.SetActive(true);
        }

        public void Disable()
        {
            _sequence.Kill();
            _frameTransform.localScale = Vector3.one;
            _frameTransform.gameObject.SetActive(false);
        }
    }
}