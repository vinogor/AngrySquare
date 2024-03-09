using System;
using Services;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace View
{
    public class RestartGameView : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private IPresenter _presenter;

        public void Initialize(IPresenter presenter)
        {
            Assert.IsNotNull(presenter);
            _presenter = presenter;
        }

        public event Action Clicked;

        private void OnEnable()
        {
            _presenter.Enable();
            _button.onClick.AddListener(Click);
        }

        private void OnDisable()
        {
            _presenter.Disable();
            _button.onClick.RemoveListener(Click);
        }

        private void Click()
        {
            Clicked?.Invoke();
        }
    }
}