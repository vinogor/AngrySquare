using DG.Tweening;
using UnityEngine;

namespace _Project.Sсripts.View
{
    public class SpellBarShaker : MonoBehaviour
    {
        [SerializeField] private Transform[] _spellsTransforms;
        [SerializeField] private Transform _skipButtonTransform;

        private Sequence[] _spellSequences = new Sequence[5];
        private Sequence _skipButtonSequence;

        private int _shakingAngle = 3;
        private float _shakeDuration = 0.2f;

        public void Initialize()
        {
            _skipButtonSequence = CreateSequence(_skipButtonTransform);

            for (var i = 0; i < _spellsTransforms.Length; i++)
            {
                _spellSequences[i] = CreateSequence(_spellsTransforms[i]);
            }
        }

        public void Enable(int amountAvailableSpells)
        {
            // TODO: ??? как запустить несколько анимаций одноврменно? 

            _skipButtonSequence.Play();

            for (var i = 0; i < amountAvailableSpells; i++)
            {
                _spellSequences[i].Play();
            }
        }

        public void Disable()
        {
            _skipButtonSequence.Pause();

            foreach (Sequence sequence in _spellSequences)
            {
                sequence.Pause();
            }
        }

        private Sequence CreateSequence(Transform transformToShake)
        {
            return DOTween.Sequence()
                .Append(transformToShake
                    .DORotate(new Vector3(0, 0, -_shakingAngle), _shakeDuration, RotateMode.LocalAxisAdd)
                    .SetEase(Ease.Linear)
                )
                .Append(transformToShake
                    .DORotate(new Vector3(0, 0, _shakingAngle), _shakeDuration, RotateMode.LocalAxisAdd)
                    .SetEase(Ease.Linear)
                )
                .OnPause(() => transformToShake.localRotation = Quaternion.Euler(new Vector3(0, 0, 0)))
                .SetLoops(-1)
                .Pause();
        }
    }
}