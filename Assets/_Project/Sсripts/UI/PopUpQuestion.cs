using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Sсripts.UI
{
    public class PopUpQuestion : MonoBehaviour
    {
        [SerializeField] private List<Button> _buttons;

        public List<Button> Buttons => _buttons;

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
    }
}