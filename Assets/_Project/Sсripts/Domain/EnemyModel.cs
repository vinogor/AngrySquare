using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Domain
{
    public class EnemyModel : MonoBehaviour
    {
        [SerializeField] private Transform[] _transforms;

        private Transform _currentTransform;
        private EnemyLevel _enemyLevel;

        public void Initialize(EnemyLevel enemyLevel)
        {
            Assert.IsNotNull(enemyLevel);
            _enemyLevel = enemyLevel;

            foreach (Transform enemyModel in _transforms)
            {
                enemyModel.gameObject.SetActive(false);
            }

            ActivateNewModel();

            _enemyLevel.Changed += SetNextModel;
        }

        private void SetNextModel()
        {
            DeactivateOldModel();
            ActivateNewModel();
        }

        private void DeactivateOldModel()
        {
            _currentTransform.gameObject.SetActive(false);
        }

        private void ActivateNewModel()
        {
            int enemyModelNumber = (_enemyLevel.Value - 1) % _transforms.Length;
            _currentTransform = _transforms[enemyModelNumber];
            _currentTransform.gameObject.SetActive(true);
        }

        private void SetDefaultModel()
        {
            DeactivateOldModel();
            _currentTransform = _transforms[0];
            _currentTransform.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            _enemyLevel.Changed -= SetNextModel;
        }
    }
}