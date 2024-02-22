using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace _Project.Sсripts.Services
{
    public class DiceRoller : MonoBehaviour, IPointerDownHandler
    {
        public event Action<int> PlayerMoveAmountSet;

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

        private void OnDestroy()
        {
            foreach (DiceFaceDetector detector in _detectors)
            {
                detector.DiceNumberDetected -= OnDiceNumberDetected;
            }
        }

        private void OnDiceNumberDetected(int number)
        {
            _lastDetectedDiceNumber = number;
            // Debug.Log("lastDetectedDiceNumber " + _lastDetectedDiceNumber);
        }

        private void Update()
        {
            if (_isDiceThrown && _rigidbody.velocity == Vector3.zero)
            {
                // Debug.Log("get dice number " + _lastDetectedDiceNumber);
                DetectorsSetActive(false);
                PlayerMoveAmountSet?.Invoke(_lastDetectedDiceNumber);
                _isDiceThrown = false;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // можно кликнуть на кубик пока он в полёте, но возможно это не проблема

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
                    // пришлось сделать задержку, т.к. в самом начале когда детекторы включаются,
                    // rb ещё неподвижно, и число уже отправляется
                    StartCoroutine(SetIsDiceThrownTrueWithDelay());
                }
            }
        }

        private IEnumerator SetIsDiceThrownTrueWithDelay()
        {
            yield return new WaitForSeconds(1f);
            _isDiceThrown = true;
        }

        public void MakeAvailable()
        {
            _vfx.Play();
            _canPlayerThrow = true;
        }

        public void MakeUnavailable()
        {
            _canPlayerThrow = false;
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

            // Debug.Log("detectors set active = " + value);
        }
    }
}