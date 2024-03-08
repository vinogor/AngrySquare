using DG.Tweening;
using UnityEngine;

namespace View
{
    public class SpellBarShaker : MonoBehaviour
    {
        [SerializeField] private Transform[] _spellsTransforms;
        [SerializeField] private Transform _skipButtonTransform;

        private Sequence _sequence;

        private const int ShakingAngle = 3;
        private const float ShakeDuration = 0.2f;

        public void Enable(int amountAvailableSpells)
        {
            _sequence = DOTween.Sequence();
            _sequence.Join(CreateSequence(_skipButtonTransform));

            for (var i = 0; i < amountAvailableSpells; i++)
                _sequence.Join(CreateSequence(_spellsTransforms[i]));

            _sequence.Play();
        }

        public void Disable()
        {
            _sequence.Pause();
            ResetRotation();
        }

        private void ResetRotation()
        {
            Quaternion zeroQuaternion = Quaternion.Euler(Vector3.zero);
            _skipButtonTransform.localRotation = zeroQuaternion;

            foreach (Transform spellsTransform in _spellsTransforms)
            {
                spellsTransform.localRotation = zeroQuaternion;
            }
        }

        private Sequence CreateSequence(Transform transformToShake)
        {
            return DOTween.Sequence()
                .Append(transformToShake
                    .DORotate(new Vector3(0, 0, -ShakingAngle), ShakeDuration, RotateMode.LocalAxisAdd)
                    .SetEase(Ease.Linear)
                )
                .Append(transformToShake
                    .DORotate(new Vector3(0, 0, ShakingAngle), ShakeDuration, RotateMode.LocalAxisAdd)
                    .SetEase(Ease.Linear)
                )
                .SetLoops(-1)
                .Pause();
        }
    }
}