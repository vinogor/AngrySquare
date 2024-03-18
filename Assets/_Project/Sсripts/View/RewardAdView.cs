using System;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class RewardAdView : MonoBehaviour
    {
        [SerializeField] private Button _button;

        public event Action Clicked;

        private void OnEnable()
        {
            _button.onClick.AddListener(Click);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Click);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Click()
        {
            Clicked?.Invoke();
        }
    }
}