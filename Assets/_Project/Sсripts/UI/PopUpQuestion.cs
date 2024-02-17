using _Project.Sсripts.Model.Effects;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Sсripts.UI
{
    public class PopUpQuestion : MonoBehaviour
    {
        [SerializeField] private Button[] _buttons;

        public Button[] Buttons => _buttons;

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

        public void SetSprite(int index, Sprite sprite)
        {
            _buttons[index].GetComponentInChildren<QuestionImage>().GetComponent<Image>().sprite = sprite;
        }
    }
}