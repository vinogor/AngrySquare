using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Sсripts.UI.PopUpNotification
{
    public class PopUpNotification : MonoBehaviour
    {
        [SerializeField] [Required] private Button _button;
        [SerializeField] [Required] private TextMeshProUGUI _title;
        [SerializeField] [Required] private TextMeshProUGUI _info;

        public Button Button => _button;

        private void Awake()
        {
            Hide();
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