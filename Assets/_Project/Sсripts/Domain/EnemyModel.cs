using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Domain
{
    public class EnemyModel : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private Material _material;

        private EnemyLevel _enemyLevel;

        private Vector3 _defaultLocalScale;
        private Color _defaultMaterialColor;

        private float _duration = 3f;

        public void Initialize(EnemyLevel enemyLevel)
        {
            _defaultLocalScale = _transform.localScale;
            _defaultMaterialColor = new Color(_material.color.r, _material.color.g, _material.color.b, 1f);

            Assert.IsNotNull(enemyLevel);
            _enemyLevel = enemyLevel;

            _enemyLevel.Increased += SetNewModel;
            _enemyLevel.SetDefault += SetDefaultModel;
        }

        private void SetNewModel()
        {
            _transform.DOScale(_transform.localScale + new Vector3(0.7f, 0.7f, 0.7f), _duration);
            _material.color = new Color(Random.value, Random.value, Random.value);
        }

        private void SetDefaultModel()
        {
            _transform.DOScale(_defaultLocalScale, 0.001f) ;
            _material.color = _defaultMaterialColor;
        }

        private void OnDestroy()
        {
            _enemyLevel.Increased -= SetNewModel;
            _enemyLevel.SetDefault -= SetDefaultModel;
        }
    }
}