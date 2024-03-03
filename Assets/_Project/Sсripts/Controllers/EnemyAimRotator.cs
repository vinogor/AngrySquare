using DG.Tweening;
using UnityEngine;

namespace _Project.Controllers{
    public class EnemyAimRotator : MonoBehaviour
    {
        // TODO: вынести числа в настройки
        
        private void Start()
        {
            float duration = 1f;
            float scale = 0.4f;

            transform.DORotate(new Vector3(90f, 90f, 0f), duration)
                .SetLoops(-1, LoopType.Incremental)
                .SetEase(Ease.Linear);

            transform.DOScale(new Vector3(scale, scale, scale), duration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
    }
}