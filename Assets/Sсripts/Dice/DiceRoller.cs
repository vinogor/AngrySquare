using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Sсripts.Dice
{
    public class DiceRoller : MonoBehaviour, IPointerDownHandler
    {
        public event Action<int> PlayerMoveAmountSet;

        [SerializeField] private float _maxRandomTorqueForce = 300f;
        [SerializeField] private float _upForce = 300f;
        [SerializeField] private Camera _camera;

        private List<FaceDetector> _detectors;
        private Rigidbody _rigidbody;
        private bool _isAvailable = false;

        public void Initialize()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _detectors = GetComponentsInChildren<FaceDetector>().ToList();

            foreach (FaceDetector detector in _detectors)
            {
                detector.DiceNumberDetected += OnDiceNumberDetected;
            }

            DetectorsSetActive(false);
        }

        private void OnDestroy()
        {
            foreach (FaceDetector detector in _detectors)
            {
                detector.DiceNumberDetected -= OnDiceNumberDetected;
            }
        }

        private void OnDiceNumberDetected(int number)
        {
            // Debug.Log("OnDiceNumberDetected " + number + ", velocity = " + _rigidbody.velocity);
            
            if (_rigidbody.velocity == Vector3.zero)
            {
                Debug.Log("get dice number " + number);
                DetectorsSetActive(false);
                PlayerMoveAmountSet?.Invoke(number);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isAvailable == false)
            {
                return;
            }

            Ray ray = _camera.ScreenPointToRay(eventData.position);

            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                if (raycastHit.collider.gameObject == gameObject)
                {
                    RollDice();
                    DetectorsSetActive(true);
                }
            }
        }

        public void MakeAvailable()
        {
            _isAvailable = true;
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
            foreach (FaceDetector detector in _detectors)
            {
                detector.gameObject.SetActive(value);
            }

            // Debug.Log("detectors set active = " + value);
        }
    }
}