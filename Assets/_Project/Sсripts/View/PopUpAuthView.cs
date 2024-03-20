using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class PopUpAuthView : MonoBehaviour
    {
        [SerializeField] [Required] private Button _buttonYes;
        [SerializeField] [Required] private Button _buttonNo;

        public event Action<bool> Clicked;

        private void OnEnable()
        {
            _buttonYes.onClick.AddListener(() => Click(true));
            _buttonNo.onClick.AddListener(() => Click(false));
        }

        private void OnDisable()
        {
            _buttonYes.onClick.RemoveListener(() => Click(true));
            _buttonNo.onClick.RemoveListener(() => Click(false));
        }

        private void Click(bool value)
        {
            Clicked?.Invoke(value);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}