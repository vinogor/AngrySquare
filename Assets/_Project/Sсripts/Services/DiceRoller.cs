using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace _Project.Services
{
    public class DiceRoller : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private float _maxRandomTorqueForce = 300f;
        [SerializeField] private float _upForce = 300f;
        [SerializeField] [Required] private Camera _camera;
        [SerializeField] [Required] private Rigidbody _rigidbody;
        [SerializeField] [Required] private ParticleSystem _vfx;

        private DiceFaceDetector[] _detectors;
        private bool _canPlayerThrow = false;
        private bool _isDiceThrown = false;
        private int _lastDetectedDiceNumber;

        public void Initialize()
        {
            _detectors = GetComponentsInChildren<DiceFaceDetector>();
            Assert.AreEqual(6, _detectors.Length);

            foreach (DiceFaceDetector detector in _detectors)
            {
                Assert.IsNotNull(detector);
                detector.DiceNumberDetected += OnDiceNumberDetected;
            }

            DetectorsSetActive(false);
        }

        public event Action<int> PlayerMoveAmountSet;
        public event Action DiceFall;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_canPlayerThrow == false)
            {
                return;
            }

            _vfx.Stop();

            Ray ray = _camera.ScreenPointToRay(eventData.position);

            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                if (raycastHit.collider.gameObject == gameObject)
                {
                    RollDice();
                    DetectorsSetActive(true);
                    StartCoroutine(SetIsDiceThrownTrueWithDelay());
                }
            }
        }

        public void MakeAvailable()
        {
            _vfx.Play();
            _canPlayerThrow = true;
        }

        public void MakeUnavailable()
        {
            _vfx.Stop();
            _canPlayerThrow = false;
        }

        private void OnDestroy()
        {
            foreach (DiceFaceDetector detector in _detectors)
            {
                detector.DiceNumberDetected -= OnDiceNumberDetected;
            }
        }

        private void OnDiceNumberDetected(int number)
        {
            DiceFall?.Invoke();
            _lastDetectedDiceNumber = number;
        }

        private void Update()
        {
            if (_isDiceThrown && _rigidbody.velocity == Vector3.zero)
            {
                DetectorsSetActive(false);
                PlayerMoveAmountSet?.Invoke(_lastDetectedDiceNumber);
                _isDiceThrown = false;
            }
        }

        private IEnumerator SetIsDiceThrownTrueWithDelay()
        {
            yield return new WaitForSeconds(1f);
            _isDiceThrown = true;
        }

        private void RollDice()
        {
            float forceX = Random.Range(0, _maxRandomTorqueForce);
            float forceY = Random.Range(0, _maxRandomTorqueForce);
            float forceZ = Random.Range(0, _maxRandomTorqueForce);

            _rigidbody.AddForce(Vector3.up * _upForce);
            _rigidbody.AddTorque(forceX, forceY, forceZ);
        }

        private void DetectorsSetActive(bool value)
        {
            foreach (DiceFaceDetector detector in _detectors)
            {
                detector.gameObject.SetActive(value);
            }
        }
    }
}