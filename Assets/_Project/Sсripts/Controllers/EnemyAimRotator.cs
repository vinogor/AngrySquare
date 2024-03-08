using DG.Tweening;
using UnityEngine;

namespace Controllers
{
    public class EnemyAimRotator : MonoBehaviour
    {
        private readonly float _duration = 1f;
        private readonly float _scale = 0.9f;

        private void Start()
        {
            transform.DORotate(new Vector3(90f, 90f, 0f), _duration)
                .SetLoops(-1, LoopType.Incremental)
                .SetEase(Ease.Linear);

            transform.DOScale(new Vector3(_scale, _scale, _scale), _duration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
    }
}