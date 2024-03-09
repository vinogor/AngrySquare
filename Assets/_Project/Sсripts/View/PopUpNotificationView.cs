using System;
using NaughtyAttributes;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace View
{
    public class PopUpNotificationView : MonoBehaviour
    {
        [SerializeField] [Required] private Button _button;
        [SerializeField] [Required] private TextMeshProUGUI _title;
        [SerializeField] [Required] private TextMeshProUGUI _info;

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

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetContent(string title, string info)
        {
            _title.SetText(title);
            _info.SetText(info);
        }
    }
}