using DG.Tweening;
using UnityEngine;

namespace Controllers
{
    public class EnemyAimRotator : MonoBehaviour
    {
        private readonly Vector3 _targetRotationValue = new(90f, 90f, 0f);
        private const float Duration = 1f;
        private const float Scale = 0.9f;

        private void Start()
        {
            transform.DORotate(_targetRotationValue, Duration)
                .SetLoops(-1, LoopType.Incremental)
                .SetEase(Ease.Linear);

            transform.DOScale(new Vector3(Scale, Scale, Scale), Duration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
    }
}